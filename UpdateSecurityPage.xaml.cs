using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace test.UWP
{
    public sealed partial class UpdateSecurityPage : Page
    {
        public UpdateSecurityPage()
        {
            this.InitializeComponent();
        }

        private void CheckUpdates_Click(object sender, RoutedEventArgs e)
        {
            StatusTextBlock.Text = "You're up to date.";
            LastCheckedTextBlock.Text = $"Last checked: Today at {DateTime.Now:HH:mm}";
        }
    }
}
