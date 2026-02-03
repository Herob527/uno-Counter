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
            .Background(ThemeResource.Get<Brush>("ApplicationPageBackgroundThemeBrush"))
            .Content(new StackPanel()
            .VerticalAlignment(VerticalAlignment.Center)
            .HorizontalAlignment(HorizontalAlignment.Center)
            .Children(
                new TextBlock()
                    .Text("Hello Uno Platform!")
            )));
    }
}
