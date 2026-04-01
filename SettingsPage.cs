using System;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using Windows.ApplicationModel;
using Windows.Networking.Connectivity;
using Windows.Storage.Pickers;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace test.UWP
{
    public sealed partial class SettingsPage : Page
    {
        private readonly Random rng = new Random();
        private readonly HttpClient httpClient = new HttpClient();

        // Flag to skip initial meme format combo box event firing
        private bool _memeFormatInitialized = false;

        public SettingsPage()
        {
            this.InitializeComponent();
            LoadSettings();
            CheckInternetConnection();
        }

        private void LoadSettings()
        {
            AppVersionTextBlock.Text = GetAppVersion();
            ConfettiText.Text = $"{ConfettiSlider.Value}% Confetti";
            ScreamVolumeValue.Text = $"Volume: {ScreamVolumeSlider.Value}%";
        }

        private string GetAppVersion()
        {
            try
            {
                var v = Package.Current.Id.Version;
                return $"v{v.Major}.{v.Minor}.{v.Build}.{v.Revision}";
            }
            catch
            {
                return "Version unavailable";
            }
        }

        // 🐔 Chicken Mode: show toast notification (MessageDialog)
        private async void ChickenModeToggle_Toggled(object sender, RoutedEventArgs e)
        {
            if (ChickenModeToggle.IsOn)
            {
                var dialog = new MessageDialog("🐔 Cluck! Chicken mode activated!", "Chicken Mode");
                await dialog.ShowAsync();
            }
            else
            {
                var dialog = new MessageDialog("Chicken mode off. Back to normal.", "Chicken Mode");
                await dialog.ShowAsync();
            }
        }

        // 🔊 Scream Volume slider update
        private void ScreamVolumeSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (ScreamVolumeValue != null)
                ScreamVolumeValue.Text = $"Volume: {(int)e.NewValue}%";
        }

        // 💥 Explosion Frequency combobox popup
        private async void ExplosionFrequencyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ExplosionFrequencyComboBox.SelectedItem is ComboBoxItem item)
            {
                string content = item.Content.ToString();
                if (content == "None") return;

                var dialog = new MessageDialog($"Boom! Explosion frequency set to '{content}'. 💥", "Explosion Frequency");
                await dialog.ShowAsync();
            }
        }

        // 📁 Auto-Yeet: open file picker to select file
        private async void AutoYeetToggle_Toggled(object sender, RoutedEventArgs e)
        {
            if (AutoYeetToggle.IsOn)
            {
                var picker = new FileOpenPicker();
                picker.ViewMode = PickerViewMode.Thumbnail;
                picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                picker.FileTypeFilter.Add("*");

                var file = await picker.PickSingleFileAsync();
                if (file != null)
                {
                    var dialog = new MessageDialog($"You auto-yeeted the file:\n{file.Name}", "Auto-Yeet");
                    await dialog.ShowAsync();
                }
                else
                {
                    AutoYeetToggle.IsOn = false; // turn off if no file chosen
                }
            }
        }

        // 🌐 Meme format: open URL in browser, skip first event firing
        private async void MemeFormatComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_memeFormatInitialized)
            {
                _memeFormatInitialized = true;
                return; // skip first time event
            }

            if (MemeFormatComboBox.SelectedItem is ComboBoxItem item)
            {
                string content = item.Content.ToString();
                string url = null;

                if (content == "Drake Format")
                    url = "https://knowyourmeme.com/memes/drakeposting";
                else if (content == "Distracted Boyfriend")
                    url = "https://knowyourmeme.com/memes/distracted-boyfriend";
                else if (content == "Woman Yelling at Cat")
                    url = "https://knowyourmeme.com/memes/woman-yelling-at-cat";
                else if (content == "Galaxy Brain")
                    url = "https://knowyourmeme.com/memes/galaxy-brain";

                if (url != null)
                {
                    await Windows.System.Launcher.LaunchUriAsync(new Uri(url));
                }
            }
        }

        // 🍌 Banana mode: change background, no vibration on old SDK, show message instead
        private async void BananaToggle_Toggled(object sender, RoutedEventArgs e)
        {
            if (BananaToggle.IsOn)
            {
                MainStack.Background = new SolidColorBrush(Colors.Yellow);

                var dialog = new MessageDialog("🍌 Banana mode activated! Vibration not supported on this SDK.", "Banana Mode");
                await dialog.ShowAsync();
            }
            else
            {
                MainStack.Background = null;
            }
        }

        // 😱 Scream button: popup dialog
        private async void ScreamButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new MessageDialog("AAAAAAHHHHHHHH!!! 😱", "Scream Mode");
            await dialog.ShowAsync();
        }

        // 🎉 Confetti slider: update text with repeated emoji string
        private void ConfettiSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (ConfettiText != null)
            {
                int amount = (int)e.NewValue;
                string confettiEmojis = string.Concat(System.Linq.Enumerable.Repeat("🎉", amount / 10));
                ConfettiText.Text = $"{amount}% Confetti {confettiEmojis}";
            }
        }

        // 🥚 Easter Egg toggle: show/hide text
        private void EasterEggToggle_Toggled(object sender, RoutedEventArgs e)
        {
            EasterEggText.Visibility = EasterEggToggle.IsOn ? Visibility.Visible : Visibility.Collapsed;
        }

        // 🔀 Randomize button label
        private void RandomTextButton_Click(object sender, RoutedEventArgs e)
        {
            string[] words = { "Yeet", "Sussy", "Clown Mode", "Oof", "Rizz", "Womp Womp", "Pog", "Brrt", "Sus" };
            RandomTextButton.Content = words[rng.Next(words.Length)];
        }

        // 🌐 Fetch joke from web API with UWP-compatible JSON parsing
        private async void FetchJokeButton_Click(object sender, RoutedEventArgs e)
        {
            JokeTextBlock.Text = "Fetching joke...";
            try
            {
                string url = "https://official-joke-api.appspot.com/random_joke";
                string json = await httpClient.GetStringAsync(url);
                Joke joke = DeserializeJson<Joke>(json);
                JokeTextBlock.Text = $"{joke.Setup}\n\n{joke.Punchline}";
            }
            catch
            {
                JokeTextBlock.Text = "Failed to fetch joke.";
            }
        }

        // 📶 Internet status: show status, tap to refresh
        private void CheckInternetConnection()
        {
            var profile = NetworkInformation.GetInternetConnectionProfile();
            if (profile != null && profile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess)
            {
                InternetStatusText.Text = "Connected to the internet 🌐";
                InternetStatusText.Foreground = new SolidColorBrush(Colors.Green);
            }
            else
            {
                InternetStatusText.Text = "No internet connection ❌";
                InternetStatusText.Foreground = new SolidColorBrush(Colors.Red);
            }
        }

        private void InternetStatusText_Tapped(object sender, RoutedEventArgs e)
        {
            CheckInternetConnection();
        }

        private T DeserializeJson<T>(string json)
        {
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                return (T)serializer.ReadObject(ms);
            }
        }

        [System.Runtime.Serialization.DataContract]
        private class Joke
        {
            [System.Runtime.Serialization.DataMember(Name = "setup")]
            public string Setup { get; set; }

            [System.Runtime.Serialization.DataMember(Name = "punchline")]
            public string Punchline { get; set; }
        }
    }
}
