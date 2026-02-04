using System.ComponentModel;

namespace Counter.Services;

public interface ILanguageService : INotifyPropertyChanged
{
    string CurrentLanguage { get; }
    string this[string key] { get; }
    void SetLanguage(string languageCode);
    string GetString(string key);
    event Action<string>? LanguageChanged;
}

public class LanguageService : ILanguageService
{
    private string _currentLanguage;

    private static readonly Dictionary<string, Dictionary<string, string>> _resources = new()
    {
        ["en"] = new()
        {
            ["common_app_title"] = "Counter app",
            ["step_settings"] = "Step settings",
            ["counter_label"] = "Counter",
            ["common_clear"] = "Clear",
            ["language_label"] = "Language"
        },
        ["pl"] = new()
        {
            ["common_app_title"] = "Licznik app",
            ["step_settings"] = "Ustawienia kroku licznika",
            ["counter_label"] = "Licznik",
            ["common_clear"] = "Wyczyść",
            ["language_label"] = "Język"
        }
    };

    public event PropertyChangedEventHandler? PropertyChanged;
    public event Action<string>? LanguageChanged;

    public LanguageService()
    {
        var savedLanguage = Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride;
        _currentLanguage = string.IsNullOrEmpty(savedLanguage) ? "en" : savedLanguage;
    }

    public string CurrentLanguage => _currentLanguage;

    public string this[string key] => GetString(key);

    public string GetString(string key)
    {
        if (_resources.TryGetValue(_currentLanguage, out var langDict) &&
            langDict.TryGetValue(key, out var value))
        {
            return value;
        }

        // Fallback to English
        if (_resources.TryGetValue("en", out var fallbackDict) &&
            fallbackDict.TryGetValue(key, out var fallbackValue))
        {
            return fallbackValue;
        }

        return key;
    }

    public void SetLanguage(string languageCode)
    {
        if (_currentLanguage == languageCode)
            return;

        _currentLanguage = languageCode;
        Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = languageCode;

        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentLanguage)));
        LanguageChanged?.Invoke(languageCode);
    }
}
