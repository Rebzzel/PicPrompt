using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace PicPrompt.Resources.Pages
{
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();

            BackgroundWork.IsChecked = (bool)App.Config["allow-background-work"];
            BackgroundWork.Click += (_, __) => UpdateConfig();
        }

        public void UpdateConfig()
        {
            File.WriteAllText($"{App.Config.info.FullName}", "{\n\t\"allow-background-work\": " + BackgroundWork.IsChecked.ToString().ToLower() + "\n}");
        }
    }
}
