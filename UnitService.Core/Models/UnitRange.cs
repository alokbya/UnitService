using UnitService.Core.Constants;

namespace UnitService.Core.Models;

public record UnitRange
{
    public double Minimum { get; }
    public double Maximum { get; }
    public string Unit { get; }

    public UnitRange(double minimum, double maximum, string unit)
    {
        if (maximum < minimum)
        {
            throw new ArgumentException("Maximum must be greater than minimum");
        }

        Minimum = minimum;
        Maximum = maximum;
        Unit = unit;
    }

    public bool IsInRange(double value) => value >= Minimum && value <= Maximum;
}

// Why was double.MaxValue used? Absolute hot is a theoretical upper limit of temperature which is much, much greater than the temperature of the sun's core: https://www.pbs.org/wgbh/nova/zero/scale.html  
// https://www.pbs.org/wgbh/nova/zero/hot.html
public static class TemperatureRanges
{
    public static readonly UnitRange Celsius = new(
        UnitConstants.Temperature.AbsoluteZeroCelsius, 
        double.MaxValue, 
        UnitConstants.Temperature.Celsius);
        
    public static readonly UnitRange Fahrenheit = new(
        UnitConstants.Temperature.AbsoluteZeroFahrenheit, 
        double.MaxValue, 
        UnitConstants.Temperature.Fahrenheit);
        
    public static readonly UnitRange Kelvin = new(
        UnitConstants.Temperature.AbsoluteZeroKelvin, 
        double.MaxValue, 
        UnitConstants.Temperature.Kelvin);

    public static UnitRange GetRange(TemperatureUnit unit) => unit switch
    {
        TemperatureUnit.Celsius => Celsius,
        TemperatureUnit.Fahrenheit => Fahrenheit,
        TemperatureUnit.Kelvin => Kelvin,
        _ => throw new ArgumentException($"Unsupported temperature unit: {unit}")
    };
}