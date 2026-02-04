using Counter.Services;

namespace Counter;

public partial record LocalizationModel
{
    private readonly ILanguageService _languageService;

    public IState<string> CurrentLanguage { get; }

    public LocalizationModel(ILanguageService languageService)
    {
        _languageService = languageService;
        CurrentLanguage = State.Value(this, () => languageService.CurrentLanguage);

        languageService.LanguageChanged += async (newLang) =>
        {
            await CurrentLanguage.Update(_ => newLang, CancellationToken.None);
        };
    }

    public string GetString(string key) => _languageService.GetString(key);
}
