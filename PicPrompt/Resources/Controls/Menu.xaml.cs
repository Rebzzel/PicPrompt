using PicPrompt.Resources.Pages;
using System;
using System.Collections.Generic;
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
        public List<Page> Pages { get; private set; }

        private int _currentPageIndex;
        private Rectangle _background;

        public Menu()
        {
            InitializeComponent();

            Pages = new List<Page>();
            Pages.Add(new SettingsPage());
            Pages.Add(new AboutPage());

            _background = new Rectangle { Fill = Brushes.Black };
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

        public async void Show(Panel panel)
        {
            if (panel == null)
                throw new ArgumentNullException();

            if (panel == Parent)
                return;

            panel.Children.Add(_background);
            panel.Children.Add(this);

            Utils.Animator.Opacity(_background, 0.85, new Utils.Animator.AnimationOptions { Duration = TimeSpan.FromMilliseconds(150) });
            await Utils.Animator.Scale(this, 0.3, 0.3, 1, 1, new Utils.Animator.AnimationOptions { Duration = TimeSpan.FromMilliseconds(150), AccelerationRatio = 0.6, DecelerationRatio = 0.1 });
        }

        public async void Hide()
        {
            if (Parent == null)
                return;

            var panel = ((Panel)Parent);

            Utils.Animator.Opacity(_background, 0, new Utils.Animator.AnimationOptions { Duration = TimeSpan.FromMilliseconds(150) });
            await Utils.Animator.Scale(this, 1, 1, 0.3, 0.3, new Utils.Animator.AnimationOptions { Duration = TimeSpan.FromMilliseconds(150), AccelerationRatio = 0.6, DecelerationRatio = 0.1 });

            panel.Children.Remove(this);
            panel.Children.Remove(_background);
        }

        public async void ChangePage(int index)
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
                    break;
                case 1:
                    About.IsChecked = true;

                    About.Background.BeginAnimation(SolidColorBrush.ColorProperty, new ColorAnimation
                    {
                        To = ((SolidColorBrush)About.PressBrush).Color,
                        Duration = TimeSpan.FromMilliseconds(0)
                    });
                    break;
            }

            if (_currentPageIndex < index)
            {
                await Utils.Animator.Margin(Frame, new Thickness(-500, 0, 0, 0), new Utils.Animator.AnimationOptions { Duration = TimeSpan.FromMilliseconds(150), AccelerationRatio = 0.6, DecelerationRatio = 0.1 });
                Frame.Content = Pages[index];
                Frame.Margin = new Thickness(500, 0, 0, 0);
                await Utils.Animator.Margin(Frame, new Thickness(0, 0, 0, 0), new Utils.Animator.AnimationOptions { Duration = TimeSpan.FromMilliseconds(150), AccelerationRatio = 0.1, DecelerationRatio = 0.6 });
            }
            else if (_currentPageIndex > index)
            {
                await Utils.Animator.Margin(Frame, new Thickness(500, 0, 0, 0), new Utils.Animator.AnimationOptions { Duration = TimeSpan.FromMilliseconds(150), AccelerationRatio = 0.6, DecelerationRatio = 0.1 });
                Frame.Content = Pages[index];
                Frame.Margin = new Thickness(-500, 0, 0, 0);
                await Utils.Animator.Margin(Frame, new Thickness(0, 0, 0, 0), new Utils.Animator.AnimationOptions { Duration = TimeSpan.FromMilliseconds(150), AccelerationRatio = 0.1, DecelerationRatio = 0.6 });
            } else
            {
                Frame.Content = Pages[index];
            }

            _currentPageIndex = index;
        }
    }
}
