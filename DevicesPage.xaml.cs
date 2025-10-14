using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace test.UWP
{
    public sealed partial class DevicesPage : Page
    {
        public DevicesPage()
        {
            this.InitializeComponent();
        }

        private void BluetoothToggle_Toggled(object sender, RoutedEventArgs e)
        {
            var toggle = sender as ToggleSwitch;
            if (toggle != null)
            {
                if (toggle.IsOn)
                {
                    System.Diagnostics.Debug.WriteLine("Bluetooth turned ON");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Bluetooth turned OFF");
                }
            }
        }

        private void AddPrinterButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Add Printer",
                Content = "Simulated: Searching for printers...",
                CloseButtonText = "OK"
            };
            _ = dialog.ShowAsync();
        }
    }
}
