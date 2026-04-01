using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace test.UWP
{
    public sealed partial class GeneralPage : Page, INotifyPropertyChanged
    {
        private string _selectedResolution = "Select resolution";
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

        public GeneralPage()
        {
            this.InitializeComponent();
            this.DataContext = this;
            AdvancedToggle.Checked += (s, e) => AdvancedContent.Visibility = Visibility.Visible;
            AdvancedToggle.Unchecked += (s, e) => AdvancedContent.Visibility = Visibility.Collapsed;
        }
        private void ResetAdvancedSettings_Click(object sender, RoutedEventArgs e)
        {
            // Reset toggle switch
            DeveloperModeToggle.IsOn = false;

            // Reset ComboBox to default (Warning, index 1)
            LoggingLevelComboBox.SelectedIndex = 1;

            // Clear TextBox
            ApiEndpointTextBox.Text = string.Empty;
        }

        private void ResolutionButton_Click(object sender, RoutedEventArgs e)
        {
            (sender as Button)?.Flyout?.ShowAt(sender as FrameworkElement);
        }

        private void ResolutionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ResolutionList.SelectedItem is ListViewItem item)
            {
                SelectedResolution = item.Content.ToString();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
