using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace test.UWP
{
    public sealed partial class SystemPage : Page, INotifyPropertyChanged
    {
        private string _selectedResolution = "1920 x 1080";
        public string SelectedResolution
        {
            get => _selectedResolution;
            set
            {
                if (_selectedResolution != value)
                {
                    _selectedResolution = value;
                    OnPropertyChanged(nameof(SelectedResolution));
                }
            }
        }

        public SystemPage()
        {
            this.InitializeComponent();
            this.DataContext = this;

            AdvancedToggle.Checked += (s, e) => AdvancedContent.Visibility = Visibility.Visible;
            AdvancedToggle.Unchecked += (s, e) => AdvancedContent.Visibility = Visibility.Collapsed;

            if (VolumeValueText != null && VolumeSlider != null)
                VolumeValueText.Text = ((int)VolumeSlider.Value).ToString();
        }

        private void ResetAdvancedSettings_Click(object sender, RoutedEventArgs e)
        {
            DeveloperModeToggle.IsOn = false;
            LoggingLevelComboBox.SelectedIndex = 1;
            ApiEndpointTextBox.Text = string.Empty;
        }

        private void ResolutionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ResolutionComboBox.SelectedItem is ComboBoxItem item)
            {
                SelectedResolution = item.Content.ToString();
            }
        }

        private void VolumeSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (VolumeValueText != null)
            {
                VolumeValueText.Text = ((int)e.NewValue).ToString();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
