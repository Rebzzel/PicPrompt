using System.Windows.Controls;
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

        private void Back_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var parent = ((Grid)Parent);

            parent.Children.Remove(this);
            parent.Children.Remove(_backgroundRect);
        }
    }
}
