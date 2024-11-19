using UnitService.Core.Extensions;
using UnitService.Core.Models;
using Xunit;

namespace UnitService.UnitTests.Extensions;

public class TemperatureUnitExtensionsTests
{
    [Theory]
    [InlineData("c", TemperatureUnit.Celsius)]
    [InlineData("C", TemperatureUnit.Celsius)]
    [InlineData("celsius", TemperatureUnit.Celsius)]
    [InlineData("CELSIUS", TemperatureUnit.Celsius)]
    [InlineData("f", TemperatureUnit.Fahrenheit)]
    [InlineData("F", TemperatureUnit.Fahrenheit)]
    [InlineData("fahrenheit", TemperatureUnit.Fahrenheit)]
    [InlineData("k", TemperatureUnit.Kelvin)]
    [InlineData("K", TemperatureUnit.Kelvin)]
    [InlineData("kelvin", TemperatureUnit.Kelvin)]
    [InlineData(" C ", TemperatureUnit.Celsius)]  // Tests trimming
    public void ParseUnit_WithValidInput_ReturnsCorrectUnit(string input, TemperatureUnit expected)
    {
        var result = TemperatureUnitExtensions.ParseUnit(input);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("x")]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("celsius2")]
    public void ParseUnit_WithInvalidInput_ThrowsArgumentException(string input)
    {
        Assert.Throws<ArgumentException>(() => TemperatureUnitExtensions.ParseUnit(input));
    }
}