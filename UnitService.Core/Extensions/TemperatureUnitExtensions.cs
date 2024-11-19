using UnitService.Core.Models;

namespace UnitService.Core.Extensions;

public static class TemperatureUnitExtensions
{
    public static TemperatureUnit ParseUnit(string unit)
    {
        // Normalize input: trim, lowercase
        var normalized = unit.Trim().ToLowerInvariant();
        
        return normalized switch
        {
            "c" or "celsius" => TemperatureUnit.Celsius,
            "f" or "fahrenheit" => TemperatureUnit.Fahrenheit,
            "k" or "kelvin" => TemperatureUnit.Kelvin,
            _ => throw new ArgumentException($"Unsupported temperature unit: {unit}")
        };
    }
}