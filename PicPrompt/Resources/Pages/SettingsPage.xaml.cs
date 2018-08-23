using Microsoft.Win32;
using System;
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

            if ((bool)App.Config["start-with-windows-enabled"] == true)
            {
                StartWithWindows.IsChecked = true;
                AddToAutorun();
            } else
            {
                StartWithWindows.IsChecked = false;
                RemoveFromAutorun();
            }
      
            BackgroundWork.IsChecked = (bool)App.Config["allow-background-work"];
        }

        private void StartWithWindows_Click(object sender, RoutedEventArgs e)
        {
            App.Config["start-with-windows-enabled"] = StartWithWindows.IsChecked;
            App.Config.Save();

            if ((bool)App.Config["start-with-windows-enabled"] == true)
            {
                AddToAutorun();
            }
            else
            {
                RemoveFromAutorun();
            }
        }

        private void BackgroundWork_Click(object sender, RoutedEventArgs e)
        {
            App.Config["allow-background-work"] = BackgroundWork.IsChecked;
            App.Config.Save();
        }

        public void AddToAutorun()
        {
            var key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            key.SetValue("PicPrompt", Environment.GetCommandLineArgs()[0]);
        }

        public void RemoveFromAutorun()
        {
            var key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            key.SetValue("PicPrompt", false);
        }
    }
}
