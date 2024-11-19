namespace UnitService.Contracts.Responses;

public record ConversionResponse(
    double OriginalValue,
    string OriginalUnit,
    double ConvertedValue,
    string TargetUnit
);

public record UnitInfoResponse(
    string Unit,
    double MinimumValue,
    double MaximumValue
);