using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace test.UWP
{
    public sealed partial class MainPage : Page
    {
        private static readonly Dictionary<string, Type> TagPageMap = new Dictionary<string, Type>()
        {
            { "system", typeof(SystemPage) },
            { "home", typeof(HomePage) },
            { "apps", typeof(AppsPage) },
            { "devices", typeof(DevicesPage) },
            { "network", typeof(NetworkInternetPage) },
            { "personalization", typeof(PersonalizationPage) },
            { "appstools", typeof(AppsPage) },
            { "accounts", typeof(AccountsPage) },
            { "time", typeof(TimeLanguagePage) },
            { "gaming", typeof(GamingPage) },
            { "update", typeof(UpdateSecurityPage) },
            { "settings", typeof(SettingsPage) },
            { "my-content", typeof(MyContent) } // ✅ Added MyContent page
        };

        private List<string> Suggestions = new List<string>
        {
            "Home",
            "System",
            "Devices",
            "Network & Internet",
            "Personalization",
            "Apps & Tools",
            "Accounts",
            "Time & Language",
            "Gaming",
            "Update & Security",
            "About",
            "Settings"
        };

        private bool hasNavigatedOnLoad = false;

        public MainPage()
        {
            this.InitializeComponent();

            ApplyTitleBarButtonColors();
            this.ActualThemeChanged += MainPage_ActualThemeChanged;

            SuggestBox.TextChanged += SuggestBox_TextChanged;
            SuggestBox.QuerySubmitted += SuggestBox_QuerySubmitted;
        }

        private void MainPage_ActualThemeChanged(FrameworkElement sender, object args)
        {
            ApplyTitleBarButtonColors();
        }

        private void ApplyTitleBarButtonColors()
        {
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            bool isDark = this.ActualTheme == ElementTheme.Dark;

            Color backgroundColor = isDark ? Colors.Black : Colors.White;
            Color foregroundColor = isDark ? Colors.White : Colors.Black;
            Color hoverBackgroundColor = isDark ? Color.FromArgb(255, 30, 30, 30) : Color.FromArgb(255, 220, 220, 220);
            Color pressedBackgroundColor = isDark ? Color.FromArgb(255, 10, 10, 10) : Color.FromArgb(255, 200, 200, 200);
            Color inactiveBackgroundColor = backgroundColor;
            Color inactiveForegroundColor = isDark ? Colors.Gray : Colors.DarkGray;

            titleBar.ButtonBackgroundColor = backgroundColor;
            titleBar.ButtonForegroundColor = foregroundColor;
            titleBar.ButtonHoverBackgroundColor = hoverBackgroundColor;
            titleBar.ButtonHoverForegroundColor = foregroundColor;
            titleBar.ButtonPressedBackgroundColor = pressedBackgroundColor;
            titleBar.ButtonPressedForegroundColor = foregroundColor;
            titleBar.ButtonInactiveBackgroundColor = inactiveBackgroundColor;
            titleBar.ButtonInactiveForegroundColor = inactiveForegroundColor;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string navParam = e.Parameter as string;
            if (!string.IsNullOrEmpty(navParam))
            {
                if (navParam == "settings")
                {
                    NavView.SelectedItem = NavView.SettingsItem;
                    ContentFrame.Navigate(typeof(SettingsPage));
                    hasNavigatedOnLoad = true;
                }
                else
                {
                    var item = GetNavigationViewItem(navParam);
                    if (item != null)
                    {
                        NavView.SelectedItem = item;
                        NavView_Navigate(navParam);
                        hasNavigatedOnLoad = true;
                    }
                    else
                    {
                        NavigateToHomeDefault();
                    }
                }
            }
            else
            {
                NavigateToHomeDefault();
            }
        }
        private void NavigateToHomeDefault()
        {
            var defaultItem = GetNavigationViewItem("home");
            if (defaultItem != null)
            {
                NavView.SelectedItem = defaultItem;
                NavView_Navigate("home");
                hasNavigatedOnLoad = true;
            }
        }

        private void NavView_Loaded(object sender, RoutedEventArgs e)
        {
            NavView.MenuItems.Add(new NavigationViewItemSeparator());
            NavView.MenuItems.Add(new NavigationViewItem()
            {
                Content = "My content",
                Icon = new SymbolIcon(Symbol.Folder),
                Tag = "my-content"
            });

            if (!hasNavigatedOnLoad)
            {
                var defaultItem = GetNavigationViewItem("home");
                if (defaultItem != null)
                {
                    NavView.SelectedItem = defaultItem;
                    NavView_Navigate(defaultItem.Tag?.ToString());
                }
            }

            ContentFrame.Navigated += On_Navigated;
        }

        private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                ContentFrame.Navigate(typeof(SettingsPage));
            }
            else if (args.InvokedItemContainer != null)
            {
                var tag = args.InvokedItemContainer.Tag?.ToString();

                if (tag == "home")
                {
                    var rootFrame = Window.Current.Content as Frame;
                    rootFrame.Navigate(typeof(HomePage));
                    return;
                }

                NavView_Navigate(tag);
            }
        }

        private void NavView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            if (ContentFrame.CanGoBack)
            {
                ContentFrame.GoBack();
            }
        }

        private void NavView_Navigate(string tag)
        {
            if (string.IsNullOrEmpty(tag)) return;

            if (TagPageMap.TryGetValue(tag, out var pageType))
            {
                if (pageType != ContentFrame.CurrentSourcePageType)
                {
                    ContentFrame.Navigate(pageType);
                }
            }
        }

        private NavigationViewItem GetNavigationViewItem(string tag)
        {
            return NavView.MenuItems
                .OfType<NavigationViewItem>()
                .FirstOrDefault(i => i.Tag?.ToString() == tag);
        }

        private void On_Navigated(object sender, NavigationEventArgs e)
        {
            if (ContentFrame.SourcePageType == typeof(SettingsPage))
            {
                NavView.SelectedItem = NavView.SettingsItem;
            }
            else
            {
                var tag = TagPageMap.FirstOrDefault(kvp => kvp.Value == ContentFrame.SourcePageType).Key;
                if (tag != null)
                {
                    NavView.SelectedItem = GetNavigationViewItem(tag);
                }
            }
        }

        private void SuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var filtered = Suggestions
                    .Where(item => item.IndexOf(sender.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();

                sender.ItemsSource = filtered;
            }
        }

        private void SuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            string selected = args.QueryText.ToLowerInvariant();

            if (selected.Contains("system")) NavView_Navigate("apps");
            else if (selected.Contains("device")) NavView_Navigate("devices");
            else if (selected.Contains("network")) NavView_Navigate("network");
            else if (selected.Contains("personal")) NavView_Navigate("personalization");
            else if (selected.Contains("apps")) NavView_Navigate("apps");
            else if (selected.Contains("account")) NavView_Navigate("accounts");
            else if (selected.Contains("time")) NavView_Navigate("time");
            else if (selected.Contains("game")) NavView_Navigate("gaming");
            else if (selected.Contains("update")) NavView_Navigate("update");
            else if (selected.Contains("privacy")) NavView_Navigate("privacy");
            else if (selected.Contains("about")) NavView_Navigate("about");
            else if (selected.Contains("settings")) ContentFrame.Navigate(typeof(SettingsPage));
            else if (selected.Contains("home"))
            {
                var rootFrame = Window.Current.Content as Frame;
                rootFrame.Navigate(typeof(HomePage));
            }

            sender.ItemsSource = null;
        }
    }
}
