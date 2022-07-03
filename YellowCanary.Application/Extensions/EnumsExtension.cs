using YellowCanary.Application.Services.DataSources.Elements;

namespace YellowCanary.Application.Extensions;

public static class EnumsExtension
{
    public static OteTreatment GetOteTreatment(this string code)
    {
        var codeUpper = code?.ToUpperInvariant();

        return codeUpper switch
        {
            "OTE" => OteTreatment.Ote,
            "NOT OTE" => OteTreatment.NoOte,
            _ => throw new ArgumentException("Invalid OTE Treatment code", nameof(code))
        };
    }
}