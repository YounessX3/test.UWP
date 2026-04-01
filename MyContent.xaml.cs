using System;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Storage;
using Windows.Media.Core;
using Windows.UI.Xaml.Media;
using Windows.UI;

namespace test.UWP
{
    public sealed partial class MyContent : Page
    {
        public MyContent()
        {
            this.InitializeComponent();

            // Hide all by default
            VideoPlayer.Visibility = Visibility.Collapsed;
            MusicPlayer.Visibility = Visibility.Collapsed;
            TextScrollViewer.Visibility = Visibility.Collapsed;

            CloseVideoButton.Visibility = Visibility.Collapsed;
            CloseMusicButton.Visibility = Visibility.Collapsed;
            CloseTextButton.Visibility = Visibility.Collapsed;
        }

        private async void SelectVideoButton_Click(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker
            {
                SuggestedStartLocation = PickerLocationId.VideosLibrary,
                ViewMode = PickerViewMode.Thumbnail
            };
            picker.FileTypeFilter.Add(".mp4");

            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                var mediaSource = MediaSource.CreateFromStorageFile(file);
                VideoPlayer.Source = mediaSource;
                VideoPlayer.Visibility = Visibility.Visible;
                CloseVideoButton.Visibility = Visibility.Visible;
                VideoPlayer.MediaPlayer.Play();
            }
        }

        private async void SelectMusicButton_Click(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker
            {
                SuggestedStartLocation = PickerLocationId.MusicLibrary,
                ViewMode = PickerViewMode.List
            };
            picker.FileTypeFilter.Add(".mp3");

            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                var mediaSource = MediaSource.CreateFromStorageFile(file);
                MusicPlayer.Source = mediaSource;
                MusicPlayer.Visibility = Visibility.Visible;
                CloseMusicButton.Visibility = Visibility.Visible;
                MusicPlayer.MediaPlayer.Play();
            }
        }

        private async void SelectTextButton_Click(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
                ViewMode = PickerViewMode.List
            };
            picker.FileTypeFilter.Add(".txt");

            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                string textContent = await FileIO.ReadTextAsync(file);

                // Create border bezel with no rounded corners and accent color
                var border = new Border
                {
                    BorderThickness = new Thickness(2),
                    CornerRadius = new CornerRadius(0), // no rounded corners
                    BorderBrush = (SolidColorBrush)Application.Current.Resources["SystemControlHighlightAccentBrush"],
                    Background = new SolidColorBrush(Colors.White),
                    Padding = new Thickness(0), // no padding to keep scroll tight to border

                    Child = new ScrollViewer
                    {
                        HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                        VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                        MaxWidth = 1300,
                        MaxHeight = 900,
                        Background = new SolidColorBrush(Colors.Transparent),
                        Padding = new Thickness(0), // flush edges, no gap
                        Content = new TextBlock
                        {
                            Text = textContent,
                            TextWrapping = TextWrapping.Wrap,
                            FontSize = 16,
                            IsTextSelectionEnabled = true,
                            Foreground = new SolidColorBrush(Colors.Black),
                            Margin = new Thickness(8, 8, 8, 8) // optional inner margin inside scroll viewer
                        }
                    }
                };

                var dialog = new ContentDialog
                {
                    Title = "Text File Viewer",
                    CloseButtonText = "Close",
                    XamlRoot = this.XamlRoot,
                    RequestedTheme = ElementTheme.Default,
                    Content = border,
                    MaxWidth = 1350,   // Set max size on dialog as well
                    MaxHeight = 950
                };

                await dialog.ShowAsync();
            }
        }

        private void CloseVideoButton_Click(object sender, RoutedEventArgs e)
        {
            VideoPlayer.MediaPlayer.Pause();
            VideoPlayer.MediaPlayer.Source = null;
            VideoPlayer.Visibility = Visibility.Collapsed;
            CloseVideoButton.Visibility = Visibility.Collapsed;
        }

        private void CloseMusicButton_Click(object sender, RoutedEventArgs e)
        {
            MusicPlayer.MediaPlayer.Pause();
            MusicPlayer.MediaPlayer.Source = null;
            MusicPlayer.Visibility = Visibility.Collapsed;
            CloseMusicButton.Visibility = Visibility.Collapsed;
        }

        private void CloseTextButton_Click(object sender, RoutedEventArgs e)
        {
            TextViewer.Text = string.Empty;
            TextScrollViewer.Visibility = Visibility.Collapsed;
            CloseTextButton.Visibility = Visibility.Collapsed;
        }
    }
}