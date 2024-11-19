using UnitService.Core.Constants;
using UnitService.Core.Interfaces;
using UnitService.Core.Models;

namespace UnitService.Core.Services;

public class TemperatureConverter : IUnitConverter
{
    private readonly IReadOnlyDictionary<TemperatureUnit, UnitRange> _supportedUnits;

    public TemperatureConverter()
    {
        _supportedUnits = new Dictionary<TemperatureUnit, UnitRange>
        {
            [TemperatureUnit.Celsius] = TemperatureRanges.Celsius,
            [TemperatureUnit.Fahrenheit] = TemperatureRanges.Fahrenheit,
            [TemperatureUnit.Kelvin] = TemperatureRanges.Kelvin
        };
    }

    public Temperature Convert(Temperature source, TemperatureUnit targetUnit)
    {
        if (source.Unit == targetUnit)
        {
            return source;
        }

        var celsiusValue = ToCelsius(source);
        var targetValue = FromCelsius(celsiusValue, targetUnit);

        return new Temperature(targetValue, targetUnit);
    }

    public IEnumerable<Temperature> ConvertBulk(IEnumerable<Temperature> temperatures, TemperatureUnit targetUnit)
    {
        return temperatures.Select(t => Convert(t, targetUnit));
    }

    public IReadOnlyDictionary<TemperatureUnit, UnitRange> GetSupportedUnits() => _supportedUnits;

    private double ToCelsius(Temperature temperature) => temperature.Unit switch
    {
        TemperatureUnit.Celsius => temperature.Value,
        TemperatureUnit.Fahrenheit => (temperature.Value - 32) * 5 / 9,
        TemperatureUnit.Kelvin => temperature.Value - 273.15,
        _ => throw new ArgumentException($"Unsupported temperature unit: {temperature.Unit}")
    };

    private double FromCelsius(double celsius, TemperatureUnit targetUnit) => targetUnit switch
    {
        TemperatureUnit.Celsius => celsius,
        TemperatureUnit.Fahrenheit => (celsius * 9 / 5) + 32,
        TemperatureUnit.Kelvin => celsius + 273.15,
        _ => throw new ArgumentException($"Unsupported temperature unit: {targetUnit}")
    };
}