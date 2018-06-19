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
        private System.Windows.Forms.NotifyIcon _notifyIcon;

        public MainWindow()
        {
            InitializeComponent();

            _notifyIcon = new System.Windows.Forms.NotifyIcon();
            _notifyIcon.Text = "PicPrompt";
            _notifyIcon.Icon = new System.Drawing.Icon(Application.GetResourceStream(new Uri("pack://application:,,,/Resources/Images/icon.ico")).Stream);
            _notifyIcon.Click += (s, e) => Show();

            var contextMenu = new System.Windows.Forms.ContextMenu();
            contextMenu.MenuItems.Add("Quit", (s, e) => Quit_Click(null, null));

            _notifyIcon.ContextMenu = contextMenu;
            _notifyIcon.Visible = true;
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            string file = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
            OpenImage(file);
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Hide();
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

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            _notifyIcon.Dispose();

            Environment.Exit(0);
        }

        public void OpenImage(string path)
        {
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
