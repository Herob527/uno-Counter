using System.ComponentModel;
using System.Globalization;
using Windows.ApplicationModel.Resources;

namespace Counter.Services;

public interface ILanguageService : INotifyPropertyChanged
{
    string CurrentLanguage { get; }
    string this[string key] { get; }
    void SetLanguage(string languageCode);
    string GetString(string key);
}

public class LanguageService : ILanguageService
{
    private ResourceLoader _resourceLoader;
    private string _currentLanguage;

    public event PropertyChangedEventHandler? PropertyChanged;

    public LanguageService()
    {
        _currentLanguage = Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride;
        if (string.IsNullOrEmpty(_currentLanguage))
        {
            _currentLanguage = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
        }
        _resourceLoader = ResourceLoader.GetForViewIndependentUse();
    }

    public string CurrentLanguage => _currentLanguage;

    public string this[string key] => GetString(key);

    public string GetString(string key)
    {
        try
        {
            return _resourceLoader.GetString(key);
        }
        catch
        {
            return key;
        }
    }

    public void SetLanguage(string languageCode)
    {
        if (_currentLanguage == languageCode)
            return;

        _currentLanguage = languageCode;
        Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = languageCode;

        // Reload resource loader for new language
        _resourceLoader = ResourceLoader.GetForViewIndependentUse();

        // Notify all subscribers that strings have changed
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentLanguage)));
    }
}
