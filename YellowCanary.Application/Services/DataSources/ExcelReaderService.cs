using System.Data;
using System.Text;
using ExcelDataReader;
using Microsoft.Extensions.Logging;
using NodaTime;
using YellowCanary.Application.Extensions;
using YellowCanary.Application.Services.DataSources.Elements;

namespace YellowCanary.Application.Services.DataSources;

public interface IExcelReaderService
{
    (IList<PayCode> payCodes, IList<Payslip> payslips, IList<Disbursement> disbursements) ReadFile(string path);
}

public class ExcelReaderService : IExcelReaderService
{
    private readonly ILogger<ExcelReaderService> _logger;

    public ExcelReaderService(ILogger<ExcelReaderService> logger)
    {
        _logger = logger;
    }

    public (IList<PayCode> payCodes, IList<Payslip> payslips, IList<Disbursement> disbursements) ReadFile(string path)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        using var stream = File.Open(path, FileMode.Open, FileAccess.Read);
        using var reader = ExcelReaderFactory.CreateReader(stream);

        var result = reader.AsDataSet(new ExcelDataSetConfiguration
        {
            ConfigureDataTable = (_) => new ExcelDataTableConfiguration
            {
                UseHeaderRow = true
            }
        });

        var payCodes = ProcessPayCodeTable(result.Tables["PayCodes"]);
        var payslips = ProcessPayslipTable(result.Tables["Payslips"]);
        var disbursements = ProcessDisbursementTable(result.Tables["Disbursements"]);

        _logger.LogInformation("Exel read successfully");

        return (payCodes, payslips, disbursements);
    }

    private static IList<PayCode> ProcessPayCodeTable(DataTable table)
        => (from DataRow row in table.Rows
            select new PayCode
            {
                Id = row.Field<string>("pay_code"),
                // ReSharper disable once StringLiteralTypo typo error in excel test
                Treatment = row.Field<string>("ote_treament").GetOteTreatment()
            }).ToList();

    private static IList<Payslip> ProcessPayslipTable(DataTable table)
        => (from DataRow row in table.Rows
            select new Payslip
            {
                Id = Guid.Parse(row.Field<string>("payslip_id") ??
                                throw new ArgumentException("Invalid Payslip Id format")),
                End = LocalDate.FromDateTime(row.Field<DateTime>("end")),
                Amount = row.Field<double>("amount"),
                EmployeeId = Convert.ToInt32(row.Field<double>("employee_code")),
                PayCodeId = row.Field<string>("code")
            }).ToList();

    private static IList<Disbursement> ProcessDisbursementTable(DataTable table)
        => (from DataRow row in table.Rows
            select new Disbursement
            {
                EmployeeId = Convert.ToInt32(row.Field<double>("employee_code")),
                Amount = row.Field<double>("sgc_amount"),
                PaymentMade = row.Field<string>("payment_made").GetLocalDate(),
                PeriodFrom = row.Field<string>("pay_period_from").GetLocalDate(),
                PeriodTo = row.Field<string>("pay_period_to").GetLocalDate(),
            }).ToList();
}