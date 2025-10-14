using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace test.UWP
{
    public sealed partial class PersonalizationPage : Page
    {
        public PersonalizationPage()
        {
            this.InitializeComponent();
        }

        private void BackgroundCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selected = (BackgroundCombo.SelectedItem as ComboBoxItem)?.Content.ToString();
            switch (selected)
            {
                case "Windows Spotlight":
                    PreviewImage.Source = new BitmapImage(new System.Uri("ms-appx:///Assets/SpotlightPreview.jpg"));
                    break;
                case "Picture":
                    PreviewImage.Source = new BitmapImage(new System.Uri("ms-appx:///Assets/PicturePreview.jpg"));
                    break;
                case "Solid color":
                    PreviewImage.Source = new BitmapImage(new System.Uri("ms-appx:///Assets/SolidColorPreview.jpg"));
                    break;
                case "Slideshow":
                    PreviewImage.Source = new BitmapImage(new System.Uri("ms-appx:///Assets/SlideshowPreview.jpg"));
                    break;
                default:
                    break;
            }
        }

        private void ThemeToggle_Toggled(object sender, RoutedEventArgs e)
        {
            var isDark = ThemeToggle.IsOn;
            RequestedTheme = isDark ? ElementTheme.Dark : ElementTheme.Light;
        }

        private void AccentColorPicker_ColorChanged(ColorPicker sender, ColorChangedEventArgs args)
        {
            Color selectedColor = args.NewColor;
            Application.Current.Resources["SystemAccentColor"] = selectedColor;
        }
    }
}
