using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PicPrompt.Resources.Pages
{
    public partial class AboutPage : Page
    {
        public AboutPage()
        {
            InitializeComponent();

            var version = Assembly.GetEntryAssembly().GetName().Version;

            Version.Content = $"{version.Major}.{version.Minor}.{version.Build}";
        }

        private void Github_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Rebzzel/PicPrompt");
        }
    }
}
