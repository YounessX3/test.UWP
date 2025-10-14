using System;
using Windows.Globalization;
using Windows.System.UserProfile;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace test.UWP
{
    public sealed partial class TimeLanguagePage : Page
    {
        private DispatcherTimer _clockTimer;

        public TimeLanguagePage()
        {
            this.InitializeComponent();

            // Setup clock
            _clockTimer = new DispatcherTimer();
            _clockTimer.Interval = TimeSpan.FromSeconds(1);
            _clockTimer.Tick += ClockTimer_Tick;
            _clockTimer.Start();

            // Set region and language
            var region = new GeographicRegion();
            var languages = GlobalizationPreferences.Languages;

            RegionText.Text = $"Region: {region.DisplayName}";
            LanguageText.Text = $"Language: {languages[0]}";
        }

        private void ClockTimer_Tick(object sender, object e)
        {
            ClockText.Text = DateTime.Now.ToString("HH:mm:ss");
        }
    }
}
