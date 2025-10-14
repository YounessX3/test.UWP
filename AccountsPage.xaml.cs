using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Collections.ObjectModel;

namespace test.UWP
{
    public sealed partial class AccountsPage : Page
    {
        public ObservableCollection<string> AccountNames { get; set; }

        public AccountsPage()
        {
            this.InitializeComponent();
            AccountNames = new ObservableCollection<string>();
            AccountsListView.ItemsSource = AccountNames;
        }

        private void AddAccount_Click(object sender, RoutedEventArgs e)
        {
            string accountName = AccountInput.Text.Trim();
            if (!string.IsNullOrEmpty(accountName) && !AccountNames.Contains(accountName))
            {
                AccountNames.Add(accountName);
                AccountInput.Text = ""; // clear input
            }
        }
    }
}
