using Counter.Services;
using Uno.Extensions.Markup;

namespace Counter.Extensions;

public static class BindingExtensions
{
    private static LocalizationViewModel? _localization;

    public static void Initialize(ILanguageService languageService)
    {
        _localization = new LocalizationViewModel(languageService);
    }

    public static BindingBuilder<string, string> Localized(
        this IDependencyPropertyBuilder<string> builder,
        string resourceKey)
    {
        if (_localization is null)
            throw new InvalidOperationException("Call BindingExtensions.Initialize() first");

        return builder
            .Binding(() => _localization.CurrentLanguage)
            .Convert(_ => _localization.Model.GetString(resourceKey));
    }

    public static BindingBuilder<string, object> Localized(
        this IDependencyPropertyBuilder<object> builder,
        string resourceKey)
    {
        if (_localization is null)
            throw new InvalidOperationException("Call BindingExtensions.Initialize() first");

        return builder
            .Binding(() => _localization.CurrentLanguage)
            .Convert(_ => _localization.Model.GetString(resourceKey));
    }
}
