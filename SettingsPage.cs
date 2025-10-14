using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace test.UWP
{
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();
            LoadAppInfo();
        }

        private void LoadAppInfo()
        {
            var version = Windows.ApplicationModel.Package.Current.Id.Version;
            AppVersionTextBlock.Text = $"v{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }

        private void ThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = (ComboBoxItem)ThemeComboBox.SelectedItem;
            string theme = selectedItem.Content.ToString();

            ElementTheme newTheme = ElementTheme.Default;
            if (theme == "Light") newTheme = ElementTheme.Light;
            else if (theme == "Dark") newTheme = ElementTheme.Dark;

            if (Window.Current.Content is Frame frame &&
                frame.Content is Page page)
            {
                page.RequestedTheme = newTheme;
            }
        }
    }
}
