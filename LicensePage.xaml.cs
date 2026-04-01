using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace test.UWP
{
    public sealed partial class LicensePage : Page
    {
        public LicensePage()
        {
            this.InitializeComponent();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Go back to AboutPage
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void TextBlock_SelectionChanged_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
