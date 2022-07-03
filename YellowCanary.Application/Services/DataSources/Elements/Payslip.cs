using NodaTime;

namespace YellowCanary.Application.Services.DataSources.Elements;

public class Payslip
{
    public string Id { get; set; }

    public LocalDate End { get; set; }
    
    public int EmployeeId { get; set; }
    
    public string PayCodeId { get; set; }

    public double Amount { get; set; }
}