using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace test.UWP
{
    public sealed partial class NetworkInternetPage : Page
    {
        public NetworkInternetPage()
        {
            this.InitializeComponent();
        }

        private void WifiToggle_Toggled(object sender, RoutedEventArgs e)
        {
            var toggle = sender as ToggleSwitch;
            if (toggle.IsOn)
            {
                WifiNetworksCombo.IsEnabled = true;
                System.Diagnostics.Debug.WriteLine("Wi-Fi turned ON");
            }
            else
            {
                WifiNetworksCombo.IsEnabled = false;
                System.Diagnostics.Debug.WriteLine("Wi-Fi turned OFF");
            }
        }

        private void ConnectWifi_Click(object sender, RoutedEventArgs e)
        {
            string selectedNetwork = (WifiNetworksCombo.SelectedItem as ComboBoxItem)?.Content.ToString();
            if (!string.IsNullOrEmpty(selectedNetwork))
            {
                var dialog = new ContentDialog
                {
                    Title = "Connecting",
                    Content = $"Connecting to {selectedNetwork}...",
                    CloseButtonText = "OK"
                };
                _ = dialog.ShowAsync();
            }
        }

        private void AirplaneToggle_Toggled(object sender, RoutedEventArgs e)
        {
            var toggle = sender as ToggleSwitch;
            System.Diagnostics.Debug.WriteLine(toggle.IsOn ? "Airplane mode ON" : "Airplane mode OFF");
        }

        private void ConnectVPN_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ContentDialog
            {
                Title = "VPN Connection",
                Content = "Simulating VPN connection...",
                CloseButtonText = "OK"
            };
            _ = dialog.ShowAsync();
        }
    }
}
