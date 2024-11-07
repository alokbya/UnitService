namespace UnitService.Core.Constants;

public static class UnitConstants
{
    public static class Temperature
    {
        public const string Celsius = "Celsius";
        public const string Fahrenheit = "Fahrenheit";
        public const string Kelvin = "Kelvin";
        
        // Physical constants
        public const double AbsoluteZeroCelsius = -273.15;
        public const double AbsoluteZeroFahrenheit = -459.67;
        public const double AbsoluteZeroKelvin = 0;
    }
    
    public static class Length
    {
        public const string Meters = "Meters";
        public const string Feet = "Feet";
    }
}