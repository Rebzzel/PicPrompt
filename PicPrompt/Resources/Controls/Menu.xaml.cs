using PicPrompt.Resources.Pages;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PicPrompt.Resources.Controls
{
    public partial class Menu : UserControl
    {
        private Rectangle _background;
        private SettingsPage _settingsPage;
        private AboutPage _aboutPage;

        public Menu()
        {
            InitializeComponent();

            _background = new Rectangle
            {
                Fill = new SolidColorBrush(Color.FromArgb(122, 0, 0, 0)),
                Opacity = 0
            };

            _settingsPage = new SettingsPage();
            _aboutPage = new AboutPage();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(0);
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(1);
        }

        public void Show(Panel panel)
        {
            if (panel == null)
                throw new ArgumentNullException();

            if (panel == Parent)
                return;

            panel.Children.Add(_background);
            panel.Children.Add(this);

            Utils.Animator.Opacity(_background, 1, 150);
            Utils.Animator.Scale(this, 0.3, 0.3, 1, 1, 150);
        }

        public void Hide()
        {
            if (Parent == null)
                return;

            var panel = ((Panel)Parent);

            Utils.Animator.Opacity(_background, 0, 150);
            Utils.Animator.Scale(this, 1, 1, 0.3, 0.3, 150);

            var timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(150)
            };

            timer.Tick += (_, __) =>
            {
                panel.Children.Remove(this);
                panel.Children.Remove(_background);
                timer.Stop();
            };

            timer.Start();
        }

        public void ChangePage(int index)
        {
            foreach (UIElement child in ((Panel)Settings.Parent).Children)
            {
                var button = child as ToggleButton;

                if (button == null)
                    continue;

                button.IsChecked = false;
                button.Background.BeginAnimation(SolidColorBrush.ColorProperty, null);
            }

            switch (index)
            {
                case 0:
                    Settings.IsChecked = true;

                    Settings.Background.BeginAnimation(SolidColorBrush.ColorProperty, new ColorAnimation
                    {
                        To = ((SolidColorBrush)Settings.PressBrush).Color,
                        Duration = TimeSpan.FromMilliseconds(0)
                    });

                    Frame.Content = _settingsPage;
                    break;
                case 1:
                    About.IsChecked = true;

                    About.Background.BeginAnimation(SolidColorBrush.ColorProperty, new ColorAnimation
                    {
                        To = ((SolidColorBrush)About.PressBrush).Color,
                        Duration = TimeSpan.FromMilliseconds(0)
                    });

                    Frame.Content = _aboutPage;
                    break;
            }
        }
    }
}
