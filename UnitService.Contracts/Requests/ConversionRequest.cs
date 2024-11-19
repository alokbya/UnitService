namespace UnitService.Contracts.Requests;

public record ConversionRequest(
    double Value,
    string FromUnit,
    string ToUnit
);

public record BulkConversionRequest(
    IEnumerable<ConversionRequest> Conversions
);