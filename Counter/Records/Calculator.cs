using Counter.Enums;

namespace Counter.Records;

public record Calculator
{
    private Operator Operator { get; init; }
    private double CurrentValue { get; init; }
    private double PreviousValue { get; init; }
    private double Result { get; init; }

}
