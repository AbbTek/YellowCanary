using NodaTime;

namespace YellowCanary.Application.Extensions;

public static class LocalDateExtension
{
    public static int GetQuarter(this LocalDate date)
    {
        return date.Month switch
        {
            >= 4 and <= 6 => 1,
            >= 7 and <= 9 => 2,
            >= 10 and <= 12 => 3,
            _ => 4
        };
    }
}