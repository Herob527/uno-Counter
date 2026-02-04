using Counter.Enums;
using Counter.Records;
using Uno;

namespace Counter;

public partial record MainModel
{
    public IState<bool> IsDark { get; }

    public IState<CounterState> Counter { get; }

    public IFeed<string> CounterStatus => Counter.Select((state) => state.CounterStatus);

    public ValueTask InputCommand(CounterOperation key, CancellationToken ct)
            => Counter.Update(state => state?.Input(key), ct);

    public ValueTask IncrementStepCommand(CancellationToken ct)
            => Counter.Update(state => state?.ChangeStep(state.Step + 1), ct);

    public ValueTask DecrementStepCommand(CancellationToken ct) => Counter.Update(state =>
        (state?.Step - 1) switch
            {
                < 1 => state,
                _ => state?.ChangeStep(state.Step - 1),
            }
        , ct);
    public MainModel(IThemeService themeService)
    {
        ArgumentNullException.ThrowIfNull(themeService);
        Counter = State.Value(this, () => new CounterState());
        IsDark = State.Value(this, () => themeService.IsDark);

        themeService.ThemeChanged += async (_, _) =>
        {
            // Retrieve the IsDark property whilst still on the UI thread
            var isDark = themeService.IsDark;
            await IsDark.Update(_ => isDark, CancellationToken.None);
        };

        IsDark.ForEachAsync(async (dark, ct)
            => await themeService.SetThemeAsync(dark ? AppTheme.Dark : AppTheme.Light));
    }
}
