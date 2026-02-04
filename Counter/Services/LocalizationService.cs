using System.ComponentModel;
using Microsoft.Windows.ApplicationModel.Resources;

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
    private ResourceLoader _resourceLoader;

    public event PropertyChangedEventHandler? PropertyChanged;
    public event Action<string>? LanguageChanged;

    public LanguageService()
    {
        var savedLanguage = Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride;
        _currentLanguage = string.IsNullOrEmpty(savedLanguage) ? "en" : savedLanguage;
        _resourceLoader = new ResourceLoader();
    }

    public string CurrentLanguage => _currentLanguage;

    public string this[string key] => GetString(key);

    public string GetString(string key)
    {
        var value = _resourceLoader.GetString(key);
        return string.IsNullOrEmpty(value) ? key : value;
    }

    public void SetLanguage(string languageCode)
    {
        if (_currentLanguage == languageCode)
            return;

        _currentLanguage = languageCode;
        Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = languageCode;

        // Recreate ResourceLoader to pick up the new language
        _resourceLoader = new ResourceLoader();

        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentLanguage)));
        LanguageChanged?.Invoke(languageCode);
    }
}
