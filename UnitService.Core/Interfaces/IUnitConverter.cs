using UnitService.Core.Models;

namespace UnitService.Core.Interfaces;

public interface IUnitConverter
{
    // Single conversion
    Temperature Convert(Temperature source, TemperatureUnit targetUnit);
    
    // Bulk conversion
    IEnumerable<Temperature> ConvertBulk(IEnumerable<Temperature> temperatures, TemperatureUnit targetUnit);
    
    // Get information about supported units
    IReadOnlyDictionary<TemperatureUnit, UnitRange> GetSupportedUnits();
}