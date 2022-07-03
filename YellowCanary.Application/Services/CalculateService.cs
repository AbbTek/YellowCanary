using YellowCanary.Application.Extensions;
using YellowCanary.Application.Services.DataSources;
using YellowCanary.Application.Services.DataSources.Elements;

namespace YellowCanary.Application.Services;

public interface ICalculateService
{
    IEnumerable<CalculateResult> Calculate(string path);
}

public class CalculateService : ICalculateService
{
    private readonly IExcelReaderService _excelReaderService;

    public CalculateService(IExcelReaderService excelReaderService)
    {
        _excelReaderService = excelReaderService;
    }

    public IEnumerable<CalculateResult> Calculate(string path)
    {
        var (payCodes, payslips, disbursements) = _excelReaderService.ReadFile(path);

        var payslipsByQuarter = from payslip in payslips
            join payCode in payCodes on payslip.PayCodeId equals payCode.Id
            where payCode.Treatment == OteTreatment.Ote
            select new
            {
                Quarter = payslip.End.GetQuarter(),
                payslip.Amount,
                payslip.EmployeeId,
                payCode.Treatment
            };

        var payslipGroup = payslipsByQuarter.GroupBy(c => new
            {
                c.Quarter,
                c.EmployeeId
            })
            .Select(g => new
            {
                g.Key.Quarter,
                g.Key.EmployeeId,
                TotalOET = g.Sum(c => c.Amount)
            }).OrderBy(g => g.Quarter)
            .ThenBy(g => g.EmployeeId);

        var disbursementsByQuarter = from disbursement in disbursements
            select new
            {
                Quarter = disbursement.PeriodTo.GetQuarter(),
                disbursement.EmployeeId,
                disbursement.Amount
            };

        var disbursementsGroup = disbursementsByQuarter.GroupBy(d => new
            {
                d.Quarter,
                d.EmployeeId,
            }).Select(g => new
            {
                g.Key.Quarter,
                g.Key.EmployeeId,
                TotalDisbursement = g.Sum(c => c.Amount)
            }).OrderBy(g => g.Quarter)
            .ThenBy(g => g.EmployeeId);

        return (from payslip in payslipGroup
            join disbursement in disbursementsGroup on new { payslip.EmployeeId, payslip.Quarter } equals new
                { disbursement.EmployeeId, disbursement.Quarter } into g
            from d in g.DefaultIfEmpty()
            select new CalculateResult
            {
                Quarter = $"Q{payslip.Quarter}",
                EmployedId = payslip.EmployeeId,
                TotalOte = payslip.TotalOET,
                TotalDisbursement = d?.TotalDisbursement ?? 0
            }).ToList();
    }
}