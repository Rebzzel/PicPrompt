using ImageMagick;
using Microsoft.Win32;
using System;
using System.IO;
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

        private void Window_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                OpenImage(files[0]);
            }
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

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();

            if (dialog.ShowDialog() is true)
                OpenImage(dialog.FileName);
        }

        public void OpenImage(string path)
        {
            Viewer.Source = null;

            using (MagickImage image = new MagickImage(path))
            {
                var info = new FileInfo(path);

                Separator2.Visibility = Visibility.Visible;
                NameLbl.Visibility = Visibility.Visible;
                NameLbl.Content = $"{info.Name}";
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
