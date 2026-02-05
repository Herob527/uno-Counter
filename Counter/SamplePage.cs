using Counter.Components;
using Counter.Services;
using Uno.Extensions.Markup;
using Uno.Themes.Markup;

namespace Counter;

public sealed partial class SamplePage : Page
{
    public SamplePage()
    {
        var languageService = BindingExtensions.LanguageService;

        this.DataContext(
            new MainViewModel(this.GetThemeService(), languageService),
            (page, vm) =>
                page.Background(Theme.Brushes.Background.Default)
                    .Content(
                        BaseLayout.Create(
                            page,
                            languageService,
                            new TextBlock()
                                .Text("This is a sample page")
                                .HorizontalAlignment(HorizontalAlignment.Center)
                                .FontSize(24),
                            new TextBlock()
                                .Text("Add your content here")
                                .HorizontalAlignment(HorizontalAlignment.Center)
                                .FontSize(16)
                                .Opacity(0.7)
                        )
                    )
        );
    }
}
