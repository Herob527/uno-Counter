using Counter.Enums;
using Counter.Records;
using Counter.Services;
using Uno;

namespace Counter;

public partial record MainModel
{
    public ILanguageService LanguageService { get; }

    public IState<bool> IsDark { get; }

    public IState<CounterState> Counter { get; }

    public IState<string> CurrentLanguage { get; }

    public IFeed<string> Translation(string key) => CurrentLanguage.Select(_ => LanguageService.GetString(key));

    // Localized strings as reactive feeds
    public IFeed<string> AppTitle => CurrentLanguage.Select(_ => LanguageService["common_app_title"]);
    public IFeed<string> StepSettingsLabel => CurrentLanguage.Select(_ => LanguageService["step_settings"]);
    public IFeed<string> ClearButtonLabel => CurrentLanguage.Select(_ => LanguageService["common_clear"]);
    public IFeed<string> LanguageLabel => CurrentLanguage.Select(_ => LanguageService["language_label"]);

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

    public MainModel(IThemeService themeService, ILanguageService languageService)
    {
        ArgumentNullException.ThrowIfNull(themeService);
        ArgumentNullException.ThrowIfNull(languageService);

        LanguageService = languageService;
        Counter = State.Value(this, () => new CounterState());
        IsDark = State.Value(this, () => themeService.IsDark);
        CurrentLanguage = State.Value(this, () => languageService.CurrentLanguage);

        themeService.ThemeChanged += async (_, _) =>
        {
            // Retrieve the IsDark property whilst still on the UI thread
            var isDark = themeService.IsDark;
            await IsDark.Update(_ => isDark, CancellationToken.None);
        };

        IsDark.ForEachAsync(async (dark, ct)
            => await themeService.SetThemeAsync(dark ? AppTheme.Dark : AppTheme.Light));

        languageService.LanguageChanged += async (newLang) =>
        {
            await CurrentLanguage.Update(_ => newLang, CancellationToken.None);
        };
    }
}
