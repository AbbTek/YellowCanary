using System.Globalization;
using NodaTime;

namespace YellowCanary.Application.Extensions;

public static class StringExtension
{
    public static LocalDate GetLocalDate(this string date)
    {
        if (string.IsNullOrWhiteSpace(date))
        {
            throw new ArgumentException("Wrong date format", nameof(date));
        }

        return LocalDate.FromDateTime(DateTime.Parse(date, CultureInfo.InvariantCulture));
    }
}