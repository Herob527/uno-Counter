using Counter.Enums;

namespace Counter;

public record CounterState
{
    private int Count { get; init; }
    private int Step { get; init; } = 1;

    private CounterState Increment() => this with { Count = Count + Step };
    private CounterState Decrement() => this with { Count = Count - Step };
    private CounterState Clear() => this with { Count = 0 };

    public string Result => $"{Count}";

    public string CounterStatus => Count switch
    {
        > 0 => "Positive",
        < 0 => "Negative",
        _ => "Zero"
    };

    public CounterState Input(CounterOperation operation)
    {
        Console.WriteLine($"Input: {operation}");
        return operation switch
        {
            CounterOperation.Add => Increment(),
            CounterOperation.Subtract => Decrement(),
            CounterOperation.Clear => Clear(),
            _ => this
        };
    }

    public CounterState ChangeStep(int step) => this with { Step = step };
}
