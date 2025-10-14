using Windows.UI.Xaml.Controls;
using System.Collections.ObjectModel;

namespace test.UWP
{
    public sealed partial class AppsPage : Page
    {
        public ObservableCollection<AppItem> Apps { get; set; }

        public AppsPage()
        {
            this.InitializeComponent();

            Apps = new ObservableCollection<AppItem>
            {
                new AppItem("", "Terminal", "Developer command line interface"),
                new AppItem("", "File Explorer", "Manage your files and folders"),
                new AppItem("", "Registry Editor", "Advanced system configuration"),
                new AppItem("", "Task Manager", "Monitor performance and processes"),
                new AppItem("", "Notepad", "Edit text files quickly"),
                new AppItem("", "Calculator", "Perform basic and scientific calculations"),
                new AppItem("", "Code Editor", "Lightweight IDE for developers"),
                new AppItem("", "System Monitor", "CPU, RAM, and disk usage stats"),
                new AppItem("", "Hex Editor", "Inspect binary files"),
                new AppItem("", "Services", "Manage Windows services"),
                new AppItem("", "Command Prompt", "Legacy CLI for system tasks"),
                new AppItem("", "VS Debugger", "Debug and analyze apps"),
                new AppItem("", "PowerShell", "Powerful command shell and scripting")
            };

            AppsListView.ItemsSource = Apps;
        }
    }

    public class AppItem
    {
        public string Icon { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public AppItem(string icon, string name, string description)
        {
            Icon = icon;
            Name = name;
            Description = description;
        }
    }
}
