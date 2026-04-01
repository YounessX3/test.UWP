using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI;

namespace test.UWP
{
    public sealed partial class UpdateSecurityPage : Page
    {
        private ObservableCollection<UpdateItem> Updates = new ObservableCollection<UpdateItem>();

        public UpdateSecurityPage()
        {
            this.InitializeComponent();
            ResetSteps();
            UpdatesListView.ItemsSource = Updates;
            // Removed RestartPanel.Visibility management since no RestartPanel in XAML now
        }

        private async void CheckUpdates_Click(object sender, RoutedEventArgs e)
        {
            ResetSteps();

            TopProgressBar.Visibility = Visibility.Visible;
            UpdateProgressRing.IsActive = true;
            UpdateProgressRing.Visibility = Visibility.Visible;

            StatusTextBlock.Text = "Checking for updates...";
            LastCheckedTextBlock.Text = "";

            for (int step = 0; step <= 3; step++)
            {
                AdvanceStep(step);
                await Task.Delay(2000);
            }

            StatusTextBlock.Text = "You're up to date.";
            LastCheckedTextBlock.Text = $"Last checked: Today at {DateTime.Now:HH:mm}";

            UpdateProgressRing.IsActive = false;
            UpdateProgressRing.Visibility = Visibility.Collapsed;
            TopProgressBar.Visibility = Visibility.Collapsed;
        }

        private void AdvanceStep(int step)
        {
            var accentBrush = (SolidColorBrush)Application.Current.Resources["SystemControlHighlightAccentBrush"];

            switch (step)
            {
                case 0:
                    StepIcon1.Foreground = accentBrush;
                    break;
                case 1:
                    StepLine1.Fill = accentBrush;
                    StepIcon2.Foreground = accentBrush;
                    break;
                case 2:
                    StepLine2.Fill = accentBrush;
                    StepIcon3.Foreground = accentBrush;
                    break;
                case 3:
                    StepLine3.Fill = accentBrush;
                    StepIcon4.Foreground = accentBrush;
                    break;
            }
        }

        private void ResetSteps()
        {
            var grayBrush = new SolidColorBrush(Colors.Gray);

            StepIcon1.Foreground = grayBrush;
            StepIcon2.Foreground = grayBrush;
            StepIcon3.Foreground = grayBrush;
            StepIcon4.Foreground = grayBrush;

            StepLine1.Fill = grayBrush;
            StepLine2.Fill = grayBrush;
            StepLine3.Fill = grayBrush;
        }

        private async void DownloadAndInstall_Click(object sender, RoutedEventArgs e)
        {
            Updates.Clear();
            UpdatesListContainer.Visibility = Visibility.Visible;

            var random = new Random();

            var fakeUpdates = new[]
            {
                new UpdateItem("Windows Feature Update 22H2", GetRandomSpeed(random)),
                new UpdateItem(".NET Framework Security Update", GetRandomSpeed(random)),
                new UpdateItem("Remove Adobe Flash from Edge", GetRandomSpeed(random)),
                new UpdateItem("Cumulative Update KB5006670", GetRandomSpeed(random)),
                new UpdateItem("Windows Defender Definition Update", GetRandomSpeed(random)),
                new UpdateItem("Microsoft Edge Browser Update", GetRandomSpeed(random)),
                new UpdateItem("Optional Driver Update for Graphics Card", GetRandomSpeed(random)),
                new UpdateItem("Windows Servicing Stack Update", GetRandomSpeed(random))
            };

            foreach (var update in fakeUpdates)
                Updates.Add(update);

            var tasks = new Task[Updates.Count];
            for (int i = 0; i < Updates.Count; i++)
                tasks[i] = SimulateDownloadAsync(Updates[i]);

            await Task.WhenAll(tasks);

            UpdatesListContainer.Visibility = Visibility.Collapsed;

            // Show ContentDialog after all updates complete
            var dialog = new ContentDialog
            {
                Title = "Installation successful",
                Content = "Please restart your PC to complete the updates.",
                PrimaryButtonText = "Restart Now",
                CloseButtonText = "Later"
            };

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                // Restart simulation
                var restartDialog = new ContentDialog
                {
                    Title = "Restart",
                    Content = "The PC would restart now (simulation).",
                    CloseButtonText = "OK"
                };
                await restartDialog.ShowAsync();
            }
            else
            {
                // User chose "Later" - do nothing or handle as needed
            }
        }

        private string GetRandomSpeed(Random rnd)
        {
            double speed = rnd.NextDouble() * 3 + 0.2;
            if (speed < 1)
                return $"{(speed * 1024):0} KB/s";
            else
                return $"{speed:0.0} MB/s";
        }

        private async Task SimulateDownloadAsync(UpdateItem update)
        {
            var random = new Random();

            while (update.Progress < 100)
            {
                await Task.Delay(500);
                double increment = random.NextDouble() * 5 + 1;
                update.Progress = Math.Min(100, update.Progress + increment);
            }

            update.Progress = 100;
            update.DownloadSpeed = "Completed";
        }
    }

    public class UpdateItem : INotifyPropertyChanged
    {
        public string Name { get; }
        private double _progress;
        public double Progress
        {
            get => _progress;
            set
            {
                if (_progress != value)
                {
                    _progress = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(ProgressPercent));
                }
            }
        }

        private string _downloadSpeed;
        public string DownloadSpeed
        {
            get => _downloadSpeed;
            set
            {
                if (_downloadSpeed != value)
                {
                    _downloadSpeed = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ProgressPercent => $"{(int)Progress}%";

        public UpdateItem(string name, string initialSpeed)
        {
            Name = name;
            DownloadSpeed = initialSpeed;
            Progress = 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
