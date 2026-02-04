using Counter.Enums;
using Microsoft.UI.Text;
using Uno.Extensions.Markup;
using Uno.Material;
using Uno.Themes.Markup;
using Uno.Toolkit.UI;
using BrushBuilder = System.Action<Uno.Extensions.Markup.IDependencyPropertyBuilder<Microsoft.UI.Xaml.Media.Brush>>;

namespace Counter;

public sealed partial class MainPage : Page
{
    public MainPage()
    {
        this.DataContext(
            new MainViewModel(this.GetThemeService()),
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
                                            .Text("Licznik app")
                                            .HorizontalAlignment(HorizontalAlignment.Center)
                                            .FontSize(48)
                                            .FontWeight(FontWeights.Bold)
                                            .Margin(0, 32, 0, 32),
                                        new TextBlock()
                                            .Text("Ustawienia kroku licznika")
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
                                            .Children(

                                        new TextBlock()
                                            .Text("Licznik")
                                            .HorizontalAlignment(HorizontalAlignment.Center)
                                            .FontSize(32),
                                        new TextBlock()
                                            .Text((x) => x.Binding(() => vm.Counter.Result))
                                            .FontSize(64)
                                            .HorizontalAlignment(HorizontalAlignment.Center),
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
        ActionButton("Clear")
            .Command(() => vm.InputCommand)
            .CommandParameter(CounterOperation.Clear);
}
