namespace UnitService.Core.Models;

public enum TemperatureUnit
{
    Celsius,
    Fahrenheit,
    Kelvin
}

public class Temperature
{
    public double Value { get; }
    public TemperatureUnit Unit { get; }

    public Temperature(double value, TemperatureUnit unit)
    {
        if (unit == TemperatureUnit.Kelvin && value < 0)
        {
            throw new ArgumentException("Kelvin temperature must be greater than or equal to 0");
        }

        Value = value;
        Unit = unit;
    }
    
    public static Temperature FromCelsius(double value) => new(value, TemperatureUnit.Celsius);
    public static Temperature FromFahrenheit(double value) => new(value, TemperatureUnit.Fahrenheit);
    public static Temperature FromKelvin(double value) => new(value, TemperatureUnit.Kelvin);
}