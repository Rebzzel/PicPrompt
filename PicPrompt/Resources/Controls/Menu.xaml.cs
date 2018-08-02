using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PicPrompt.Resources.Controls
{
    public partial class Menu : UserControl
    {
        private Rectangle _backgroundRect;

        public Menu()
        {
            InitializeComponent();
        }

        public void Show(Grid parent)
        {
            if (parent != null)
            {
                _backgroundRect = new Rectangle
                {
                    Fill = new SolidColorBrush(Color.FromArgb(122, 13, 13, 13))
                };

                parent.Children.Add(_backgroundRect);
                parent.Children.Add(this);
            }
        }

        public void Hide()
        {
            if (Parent != null)
            {
                var parent = ((Grid)Parent);

                parent.Children.Remove(this);
                parent.Children.Remove(_backgroundRect);
            }
        }

        private void Back_Click(object sender,RoutedEventArgs e)
        {
            Hide();
        }

        private void General_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(0);
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(1);
        }

        public void ChangePage(int index)
        {
            var parent = ((Panel)General.Parent);

            switch (index)
            {
                case 0:
                    foreach (UIElement child in parent.Children)
                    {
                        var obj = child as ToggleButton;

                        if (obj == null || obj == General)
                            continue;

                        obj.IsChecked = false;
                        obj.Background.BeginAnimation(SolidColorBrush.ColorProperty, null);
                    }
                    break;
                case 1:
                    foreach (UIElement child in parent.Children)
                    {
                        var obj = child as ToggleButton;

                        if (obj == null || obj == About)
                            continue;

                        obj.IsChecked = false;
                        obj.Background.BeginAnimation(SolidColorBrush.ColorProperty, null);
                    }
                    break;
            }
        }
    }
}
