using NodaTime;

namespace YellowCanary.Application.Services.DataSources.Elements;

public class Disbursement
{
    public int EmployeeId { get; set; }

    public LocalDate PeriodFrom { get; set; }

    public LocalDate PeriodTo { get; set; }

    public LocalDate PaymentMade { get; set; }

    public double Amount { get; set; }
}