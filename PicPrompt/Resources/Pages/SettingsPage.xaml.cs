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
        }

        private void BackgroundWork_Click(object sender, RoutedEventArgs e)
        {
            App.Config["allow-background-work"] = BackgroundWork.IsChecked;
            App.Config.Save();
        }
    }
}
