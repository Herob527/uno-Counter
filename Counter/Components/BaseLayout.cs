using Counter.Extensions;
using Counter.Services;
using Microsoft.UI.Text;
using Uno.Extensions.Markup;
using Uno.Material;
using Uno.Themes.Markup;
using Uno.Toolkit.UI;

namespace Counter.Components;

public static class BaseLayout
{
    private static readonly Type[] Pages = [typeof(MainPage), typeof(SamplePage)];

    public static Border Create(Page page, ILanguageService languageService, params UIElement[] children)
    {
        return new Border()
            .Background(Theme.Brushes.Secondary.Container.Default)
            .SafeArea(SafeArea.InsetMask.VisibleBounds)
            .Child(
                new StackPanel()
                    .Spacing(16)
                    .HorizontalAlignment(HorizontalAlignment.Center)
                    .Orientation(Orientation.Vertical)
                    .Children(
                        new TextBlock()
                            .Text(x => x.Localized("common_app_title"))
                            .HorizontalAlignment(HorizontalAlignment.Center)
                            .FontSize(48)
                            .FontWeight(FontWeights.Bold)
                            .Margin(0, 32, 0, 32),
                        LanguageSwitcher(languageService),
                        NavigationButton(page),
                        new StackPanel()
                            .Spacing(16)
                            .HorizontalAlignment(HorizontalAlignment.Center)
                            .Orientation(Orientation.Vertical)
                            .Children(children)
                    )
            );
    }

    private static Button NavigationButton(Page page)
    {
        var button = new Button()
            .Content("Next Page")
            .HorizontalAlignment(HorizontalAlignment.Center)
            .Style(Theme.Button.Styles.Outlined);

        button.Click += (_, _) =>
        {
            var frame = page.Frame;
            if (frame is null) return;

            var currentIndex = Array.IndexOf(Pages, page.GetType());
            var nextIndex = (currentIndex + 1) % Pages.Length;
            frame.Navigate(Pages[nextIndex]);
        };

        return button;
    }

    private static StackPanel LanguageSwitcher(ILanguageService languageService)
    {
        var languages = languageService.GetLanguages();
        var menuFlyout = new MenuFlyout();
        var dropdown = new DropDownButton()
            .Content(languageService.CurrentLanguage.ToUpperInvariant())
            .Flyout(menuFlyout);

        foreach (var lang in languages)
        {
            var item = new MenuFlyoutItem() { Text = lang.ToUpperInvariant(), Tag = lang };
            item.Click += (sender, args) =>
            {
                if (sender is MenuFlyoutItem menuItem && menuItem.Tag?.ToString() is string newLang)
                {
                    languageService.SetLanguage(newLang);
                    dropdown.Content = newLang.ToUpperInvariant();
                }
            };
            menuFlyout.Items.Add(item);
        }

        return new StackPanel()
            .Orientation(Orientation.Horizontal)
            .HorizontalAlignment(HorizontalAlignment.Center)
            .Spacing(8)
            .Children(
                new TextBlock()
                    .Text(x => x.Localized("language_label"))
                    .VerticalAlignment(VerticalAlignment.Center)
                    .FontSize(16),
                dropdown
            );
    }
}
