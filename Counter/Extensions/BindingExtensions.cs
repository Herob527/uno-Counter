using Counter.Services;
using Uno.Extensions.Markup;

namespace Counter.Extensions;

public static class BindingExtensions
{
    private static readonly Lazy<(LocalizationViewModel ViewModel, ILanguageService Service)> _localization = new(() =>
    {
        var service = new LanguageService();
        return (new LocalizationViewModel(service), service);
    });

    public static ILanguageService LanguageService => _localization.Value.Service;

    public static BindingBuilder<string, string> Localized(
        this IDependencyPropertyBuilder<string> builder,
        string resourceKey)
    {
        var vm = _localization.Value.ViewModel;
        return builder
            .Binding(() => vm.CurrentLanguage)
            .Convert(_ => vm.Model.GetString(resourceKey));
    }

    public static BindingBuilder<string, object> Localized(
        this IDependencyPropertyBuilder<object> builder,
        string resourceKey)
    {
        var vm = _localization.Value.ViewModel;
        return builder
            .Binding(() => vm.CurrentLanguage)
            .Convert(_ => vm.Model.GetString(resourceKey));
    }
}
