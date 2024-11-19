using UnitService.Core.Models;
using UnitService.Core.Services;
using Xunit;

namespace UnitService.UnitTests.Services;

public class TemperatureConverterTests
{
    private readonly TemperatureConverter _converter;

    public TemperatureConverterTests()
    {
        _converter = new TemperatureConverter();
    }

    [Theory]
    [InlineData(0, TemperatureUnit.Celsius, TemperatureUnit.Fahrenheit, 32)]
    [InlineData(100, TemperatureUnit.Celsius, TemperatureUnit.Fahrenheit, 212)]
    [InlineData(0, TemperatureUnit.Celsius, TemperatureUnit.Kelvin, 273.15)]
    [InlineData(100, TemperatureUnit.Fahrenheit, TemperatureUnit.Celsius, 37.7778)]
    [InlineData(0, TemperatureUnit.Kelvin, TemperatureUnit.Celsius, -273.15)]
    public void Convert_WithValidTemperature_ReturnsCorrectConversion(
        double sourceValue, 
        TemperatureUnit sourceUnit,
        TemperatureUnit targetUnit,
        double expectedValue)
    {
        // Arrange
        var source = new Temperature(sourceValue, sourceUnit);

        // Act
        var result = _converter.Convert(source, targetUnit);

        // Assert
        Assert.Equal(expectedValue, result.Value, precision: 4);
        Assert.Equal(targetUnit, result.Unit);
    }

    [Theory]
    [InlineData(-1, TemperatureUnit.Kelvin)]  // Kelvin can't be negative
    public void Convert_WithInvalidTemperature_ThrowsArgumentException(
        double sourceValue,
        TemperatureUnit sourceUnit)
    {
        // Assert
        Assert.Throws<ArgumentException>(() => new Temperature(sourceValue, sourceUnit));
    }

    [Fact]
    public void Convert_SameUnit_ReturnsSameValue()
    {
        // Arrange
        var source = new Temperature(100, TemperatureUnit.Celsius);

        // Act
        var result = _converter.Convert(source, TemperatureUnit.Celsius);

        // Assert
        Assert.Equal(source.Value, result.Value);
        Assert.Equal(source.Unit, result.Unit);
    }

    [Fact]
    public void ConvertBulk_WithValidTemperatures_ReturnsAllConversions()
    {
        // Arrange
        var temperatures = new[]
        {
            new Temperature(0, TemperatureUnit.Celsius),
            new Temperature(100, TemperatureUnit.Celsius)
        };

        // Act
        var results = _converter.ConvertBulk(temperatures, TemperatureUnit.Fahrenheit).ToList();

        // Assert
        Assert.Equal(2, results.Count);
        Assert.Equal(32, results[0].Value);
        Assert.Equal(212, results[1].Value);
    }

    [Fact]
    public void GetSupportedUnits_ReturnsAllUnits()
    {
        // Act
        var units = _converter.GetSupportedUnits();

        // Assert
        Assert.Equal(3, units.Count);  // Celsius, Fahrenheit, and Kelvin
        Assert.Contains(TemperatureUnit.Celsius, units.Keys);
        Assert.Contains(TemperatureUnit.Fahrenheit, units.Keys);
        Assert.Contains(TemperatureUnit.Kelvin, units.Keys);
    }
}