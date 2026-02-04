using Counter.Enums;
using Counter.Services;
using Microsoft.UI.Text;
using Uno.Extensions.Markup;
using Uno.Material;
using Uno.Themes.Markup;
using Uno.Toolkit.UI;

namespace Counter;

public sealed partial class MainPage : Page
{
    private readonly ILanguageService _languageService;

    public MainPage()
    {
        _languageService = new LanguageService();
        BindingExtensions.Initialize(_languageService);

        this.DataContext(
            new MainViewModel(this.GetThemeService(), _languageService),
            (page, vm) =>
                page.Background(Theme.Brushes.Background.Default)
                    .Content(
                        new Border()
                            .Background(Theme.Brushes.Secondary.Container.Default)
                            .SafeArea(SafeArea.InsetMask.VisibleBounds)
                            .Child(
                                new StackPanel()
                                    .Spacing(16)
                                    .HorizontalAlignment(HorizontalAlignment.Center)
                                    .Orientation(Orientation.Vertical)
                                    .Children(
                                        new TextBlock()
                                            .Text(x => x.Binding(() => vm.AppTitle))
                                            .HorizontalAlignment(HorizontalAlignment.Center)
                                            .FontSize(48)
                                            .FontWeight(FontWeights.Bold)
                                            .Margin(0, 32, 0, 32),
                                        LanguageSwitcher(vm),
                                        new TextBlock()
                                            .Text(x => x.Binding(() => vm.StepSettingsLabel))
                                            .HorizontalAlignment(HorizontalAlignment.Center)
                                            .FontSize(32),
                                        new StackPanel()
                                            .Orientation(Orientation.Horizontal)
                                            .HorizontalAlignment(HorizontalAlignment.Center)
                                            .Spacing(16)
                                            .Children(
                                                AddStep(vm),
                                                new TextBlock()
                                                    .Text(x => x.Binding(() => vm.Counter.Step))
                                                    .FontSize(64),
                                                SubtractStep(vm)
                                            ),
                                        new StackPanel()
                                            .Orientation(Orientation.Vertical)
                                            .HorizontalAlignment(HorizontalAlignment.Center)
                                            .Margin(0, 16, 0, 0)
                                            .Children(
                                                new TextBlock()
                                                    .Text(x => x.Localized("counter_label"))
                                                    .HorizontalAlignment(HorizontalAlignment.Center)
                                                    .FontSize(32),
                                                new TextBlock()
                                                    .Text(x => x.Binding(() => vm.Counter.Result))
                                                    .FontSize(64)
                                                    .HorizontalAlignment(
                                                        HorizontalAlignment.Center
                                                    ),
                                                new StackPanel()
                                                    .Orientation(Orientation.Horizontal)
                                                    .Spacing(16)
                                                    .HorizontalAlignment(HorizontalAlignment.Center)
                                                    .Children(
                                                        AddCountButton(vm),
                                                        SubtractCountButton(vm),
                                                        ClearCountButton(vm)
                                                    )
                                            )
                                    )
                            )
                    )
        );
    }

    private StackPanel LanguageSwitcher(MainViewModel vm)
    {
        var toggle = new ToggleSwitch()
            .OffContent("EN")
            .OnContent("PL")
            .IsOn(x => x.Binding(() => vm.CurrentLanguage).Convert(lang => lang == "pl"));

        toggle.Toggled += (sender, args) =>
        {
            if (sender is ToggleSwitch ts)
            {
                var newLang = ts.IsOn ? "pl" : "en";
                _languageService.SetLanguage(newLang);
            }
        };

        return new StackPanel()
            .Orientation(Orientation.Horizontal)
            .HorizontalAlignment(HorizontalAlignment.Center)
            .Spacing(8)
            .Children(
                new TextBlock()
                    .Text(x => x.Binding(() => vm.LanguageLabel))
                    .VerticalAlignment(VerticalAlignment.Center)
                    .FontSize(16),
                toggle
            );
    }

    private Button BaseButton() =>
        new Button()
            .FontSize(32)
            .HorizontalAlignment(HorizontalAlignment.Stretch)
            .VerticalAlignment(VerticalAlignment.Stretch)
            .ControlExtensions(elevation: 0)
            .Style(Theme.Button.Styles.Elevated);

    private Button ActionButton(string content) => BaseButton().Content(content);

    private Button ActionButton(Action<IDependencyPropertyBuilder<object>> configureBinding) =>
        BaseButton().Content(x => configureBinding(x));

    private Button AddStep(MainViewModel vm) =>
        ActionButton("+1").Command(() => vm.IncrementStepCommand);

    private Button SubtractStep(MainViewModel vm) =>
        ActionButton("-1").Command(() => vm.DecrementStepCommand);

    private Button AddCountButton(MainViewModel vm) =>
        ActionButton(x => x.Binding(() => vm.Counter.Step).Convert(step => $"+{step}"))
            .Command(() => vm.InputCommand)
            .CommandParameter(CounterOperation.Add);

    private Button SubtractCountButton(MainViewModel vm) =>
        ActionButton(x => x.Binding(() => vm.Counter.Step).Convert(step => $"-{step}"))
            .Command(() => vm.InputCommand)
            .CommandParameter(CounterOperation.Subtract);

    private Button ClearCountButton(MainViewModel vm) =>
        ActionButton(x => x.Binding(() => vm.ClearButtonLabel))
            .Command(() => vm.InputCommand)
            .CommandParameter(CounterOperation.Clear);
}
