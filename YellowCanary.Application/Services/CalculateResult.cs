namespace YellowCanary.Application.Services;

public class CalculateResult
{
    public string Quarter { get; init; }

    public int EmployedId { get; init; }

    public double TotalOte { get; init; }
    
    public double TotalNoOte { get; init; }

    public double TotalDisbursement { get; init; }

    public double SuperPayable { get; init; }
}