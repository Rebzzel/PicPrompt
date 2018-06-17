using ImageMagick;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace PicPrompt
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MaximizeOrRestore_Click(object sender, RoutedEventArgs e)
        {
            switch (WindowState)
            {
                case WindowState.Normal:
                    WindowState = WindowState.Maximized;
                    break;
                case WindowState.Maximized:
                    WindowState = WindowState.Normal;
                    break;
            }
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void OpenImage(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            
            if (dialog.ShowDialog() is true)
            {
                using (MagickImage image = new MagickImage(dialog.FileName))
                {
                    Separator2.Visibility = Visibility.Visible;
                    NameLbl.Visibility = Visibility.Visible;
                    NameLbl.Content = $"{dialog.SafeFileName}";
                    Separator3.Visibility = Visibility.Visible;
                    SizeLbl.Visibility = Visibility.Visible;
                    SizeLbl.Content = $"{image.Width} x {image.Height}";

                    NoneContentGrid.Visibility = Visibility.Collapsed;

                    Viewer.Width = image.Width;
                    Viewer.Height = image.Height;
                    Viewer.Source = image.ToBitmapSource();

                    if (Toolbar.Visibility == Visibility.Collapsed)
                    {
                        Toolbar.Visibility = Visibility.Visible;
                        Toolbar.BeginAnimation(MarginProperty, new ThicknessAnimation(new Thickness(0, 0, 0, 0), new Thickness(0, 0, 0, 30), new TimeSpan(0, 0, 0, 0, 700)));
                        Toolbar.BeginAnimation(OpacityProperty, new DoubleAnimation(0, 1, new TimeSpan(0, 0, 1)));
                    }
                }
            }
        }
    }
}
