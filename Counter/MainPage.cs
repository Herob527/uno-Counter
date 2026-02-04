using Counter.Enums;
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
        this.DataContext(new MainViewModel(this.GetThemeService()), (page, vm) =>
        page
            .Background(Theme.Brushes.Background.Default)
            .Content(
                new Border().Background(Theme.Brushes.Secondary.Container.Default)
                .SafeArea(SafeArea.InsetMask.VisibleBounds)
                .Child(
                    new Grid()
                        .RowSpacing(16)
                        .ColumnDefinitions("*,*,*,*,*")
                        .RowDefinitions("*,*,*,*,*,*")
                        .Padding(16)
                        .ColumnSpacing(16)
                        .Children(
                            AddStep(vm)
                                .Grid(column: 1, row: 1),
                            new TextBlock()
                                .Text(x => x.Binding(() => vm.Counter.Step))
                                .HorizontalAlignment(HorizontalAlignment.Center)
                                .VerticalAlignment(VerticalAlignment.Center)
                                .Grid(column: 2, row: 1)
                                .FontSize(64),
                            SubtractStep(vm)
                                .Grid(column: 3, row: 1)
                            ,
                            new TextBlock()
                                .Text((x) => x.Binding(() => vm.Counter.Result))
                                .HorizontalAlignment(HorizontalAlignment.Center)
                                .VerticalAlignment(VerticalAlignment.Center)
                                .FontSize(64)
                                .Grid(column: 2, row: 3),
                            AddCountButton(vm)
                                .Grid(column: 1, row: 4),
                            SubtractCountButton(vm)
                                .Grid(column: 2, row: 4),
                            ClearCountButton(vm)
                                .Grid(column: 3, row: 4)
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

    private Button ActionButton(string content) =>
        BaseButton()
            .Content(content);

    private Button ActionButton(Action<IDependencyPropertyBuilder<object>> configureBinding) =>
        BaseButton()
            .Content(x => configureBinding(x));

    private Button AddStep(MainViewModel vm) => ActionButton("+1")
        .Command(() => vm.IncrementStepCommand);

    private Button SubtractStep(MainViewModel vm) => ActionButton("-1")
        .Command(() => vm.DecrementStepCommand);

    private Button AddCountButton(MainViewModel vm) =>
        ActionButton(x => x.Binding(() => vm.Counter.Step)
                           .Convert(step => $"+{step}"))
            .Command(() => vm.InputCommand)
            .CommandParameter(CounterOperation.Add);

    private Button SubtractCountButton(MainViewModel vm) =>
        ActionButton(x => x.Binding(() => vm.Counter.Step)
                           .Convert(step => $"-{step}"))
            .Command(() => vm.InputCommand)
            .CommandParameter(CounterOperation.Subtract);

    private Button ClearCountButton(MainViewModel vm) =>
        ActionButton("Clear")
            .Command(() => vm.InputCommand)
            .CommandParameter(CounterOperation.Clear);

}
