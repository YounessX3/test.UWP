using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.ViewManagement;
using Windows.ApplicationModel.Core;

namespace test.UWP
{
    public sealed partial class HomePage : Page
    {
        private ToggleButton[] buttons;

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
            "Settings"
        };

        public HomePage()
        {
            this.InitializeComponent();

            // Set dark gray title bar button color
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            Color titleBarColor = (this.ActualTheme == ElementTheme.Dark)
                ? Color.FromArgb(0xFF, 0x1E, 0x1E, 0x1E)  // Dark mode color
                : Color.FromArgb(0xFF, 0xEF, 0xEF, 0xEF); // Light mode color

            titleBar.ButtonBackgroundColor = titleBarColor;
            titleBar.ButtonForegroundColor = Colors.White;
            titleBar.ButtonHoverBackgroundColor = Color.FromArgb(255, 60, 60, 63);
            titleBar.ButtonHoverForegroundColor = Colors.White;
            titleBar.ButtonPressedBackgroundColor = Color.FromArgb(255, 30, 30, 33);
            titleBar.ButtonPressedForegroundColor = Colors.White;
            titleBar.ButtonInactiveBackgroundColor = titleBarColor;
            titleBar.ButtonInactiveForegroundColor = Colors.Gray;

            buttons = new[]
            {
                BtnSystem,
                BtnDevices,
                BtnNetwork,
                BtnPersonalization,
                BtnApps,
                BtnAccounts,
                BtnTime,
                BtnGaming,
                BtnUpdate
            };

            foreach (var btn in buttons)
            {
                SetupRevealBrush(btn);
                btn.Checked += Btn_Checked;
            }

            SetButtonsBackground();
            this.ActualThemeChanged += HomePage_ActualThemeChanged;

            SuggestBox.TextChanged += SuggestBox_TextChanged;
            SuggestBox.QuerySubmitted += SuggestBox_QuerySubmitted;
        }

        private void HomePage_ActualThemeChanged(FrameworkElement sender, object args)
        {
            SetButtonsBackground();

            Color titleBarColor = (this.ActualTheme == ElementTheme.Dark)
                ? Color.FromArgb(0xFF, 0x1E, 0x1E, 0x1E)
                : Color.FromArgb(0xFF, 0xEF, 0xEF, 0xEF);

            var titleBar = ApplicationView.GetForCurrentView().TitleBar;

            titleBar.ButtonBackgroundColor = titleBarColor;
            titleBar.ButtonForegroundColor = (this.ActualTheme == ElementTheme.Dark) ? Colors.White : Colors.Black;

            // Adjust hover and pressed colors similarly...
        }

        private void SetButtonsBackground()
        {
            Brush backgroundBrush = this.ActualTheme == ElementTheme.Dark
                ? (Brush)Resources["DarkRevealBackgroundBrush"]
                : (Brush)Resources["LightRevealBackgroundBrush"];

            foreach (var btn in buttons)
            {
                btn.Background = backgroundBrush;
            }
        }

        private void SetupRevealBrush(Control button)
        {
            RevealBrush.SetState(button, RevealBrushState.Normal);

            button.PointerEntered += (s, e) => RevealBrush.SetState(button, RevealBrushState.PointerOver);
            button.PointerExited += (s, e) => RevealBrush.SetState(button, RevealBrushState.Normal);
            button.PointerPressed += (s, e) => RevealBrush.SetState(button, RevealBrushState.Pressed);
            button.PointerReleased += (s, e) => RevealBrush.SetState(button, RevealBrushState.PointerOver);
        }

        private void Btn_Checked(object sender, RoutedEventArgs e)
        {
            var btn = sender as ToggleButton;
            if (btn == null)
                return;

            string tag = null;

            if (btn == BtnSystem) tag = "system";
            else if (btn == BtnDevices) tag = "devices";
            else if (btn == BtnNetwork) tag = "network";
            else if (btn == BtnPersonalization) tag = "personalization";
            else if (btn == BtnApps) tag = "apps";
            else if (btn == BtnAccounts) tag = "accounts";
            else if (btn == BtnTime) tag = "time";
            else if (btn == BtnGaming) tag = "gaming";
            else if (btn == BtnUpdate) tag = "update";

            if (!string.IsNullOrEmpty(tag))
            {
                var rootFrame = Window.Current.Content as Frame;
                rootFrame?.Navigate(typeof(MainPage), tag);
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            foreach (var btn in buttons)
            {
                btn.IsChecked = false;
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
            string selected = args.QueryText;
            var rootFrame = Window.Current.Content as Frame;

            switch (selected.ToLowerInvariant())
            {
                case "home":
                    rootFrame?.Navigate(typeof(HomePage));
                    break;
                case "system":
                    rootFrame?.Navigate(typeof(MainPage), "system");
                    break;
                case "devices":
                    rootFrame?.Navigate(typeof(MainPage), "devices");
                    break;
                case "network & internet":
                    rootFrame?.Navigate(typeof(MainPage), "network");
                    break;
                case "personalization":
                    rootFrame?.Navigate(typeof(MainPage), "personalization");
                    break;
                case "apps":
                    rootFrame?.Navigate(typeof(MainPage), "apps");
                    break;
                case "accounts":
                    rootFrame?.Navigate(typeof(MainPage), "accounts");
                    break;
                case "time & language":
                    rootFrame?.Navigate(typeof(MainPage), "time");
                    break;
                case "gaming":
                    rootFrame?.Navigate(typeof(MainPage), "gaming");
                    break;
                case "update & security":
                case "update":
                    rootFrame?.Navigate(typeof(MainPage), "update");
                    break;
                case "settings":
                    rootFrame?.Navigate(typeof(SettingsPage));
                    break;
                default:
                    break;
            }

            sender.ItemsSource = null;
        }

        // NEW: Settings button click handler
        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            var rootFrame = Window.Current.Content as Frame;
            rootFrame?.Navigate(typeof(MainPage), "settings");
        }
    }
}