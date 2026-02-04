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
                        .ColumnDefinitions<Grid>("*,*,*")
                        .RowDefinitions<Grid>("*,*,*,*,*,*")
                        .Padding(16)
                        .Children(

                            new TextBlock()
                                .Text((x) => x.Binding(() => vm.CounterStatus))
                                .HorizontalAlignment(HorizontalAlignment.Center)
                                .VerticalAlignment(VerticalAlignment.Center)
                                .FontSize(64).Grid(column: 1, row: 0),
                            new TextBlock()
                                .Text((x) => x.Binding(() => vm.Counter.Result))
                                .HorizontalAlignment(HorizontalAlignment.Center)
                                .VerticalAlignment(VerticalAlignment.Center)
                                .FontSize(64).Grid(column: 0, row: 0),
                            AddButton(vm).Grid(row: 1),
                            SubtractButton(vm).Grid(row: 2),
                            ClearButton(vm).Grid(row: 3)
                        )

                    )
            )
        );
    }

    private Button ActionButton(string content) =>
        new Button().Content(content)
            .FontSize(32)
            .HorizontalAlignment(HorizontalAlignment.Stretch)
            .VerticalAlignment(VerticalAlignment.Stretch)
            .ControlExtensions(elevation: 0)
            .Style(Theme.Button.Styles.Elevated);

    private Button AddButton(MainViewModel vm) =>
        ActionButton("+")
            .Command(() => vm.InputCommand)
            .CommandParameter(CounterOperation.Add);

    private Button SubtractButton(MainViewModel vm) =>
        ActionButton("-")
            .Command(() => vm.InputCommand)
            .CommandParameter(CounterOperation.Subtract);

    private Button ClearButton(MainViewModel vm) =>
        ActionButton("Clear")
            .Command(() => vm.InputCommand)
            .CommandParameter(CounterOperation.Clear);

}
