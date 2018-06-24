using ImageMagick;
using Microsoft.Win32;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace PicPrompt
{
    public partial class MainWindow : Window
    {
        private System.Windows.Forms.NotifyIcon _notifyIcon;

        private MagickImage _image;

        private int _imageScale;

        private bool _imageIsZoomed;

        public MainWindow()
        {
            InitializeComponent();
            InitializeNotifyIcon();
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

        private void InitializeNotifyIcon()
        {
            _notifyIcon = new System.Windows.Forms.NotifyIcon();
            _notifyIcon.Icon = new System.Drawing.Icon(Application.GetResourceStream(new Uri("pack://application:,,,/Resources/Images/icon.ico")).Stream);
            _notifyIcon.Text = "PicPrompt";

            _notifyIcon.Click += (s, e) => Show();

            var contextMenu = new System.Windows.Forms.ContextMenu();
            contextMenu.MenuItems.Add("Quit", (s, e) => Quit_Click(null, null));

            _notifyIcon.ContextMenu = contextMenu;
            _notifyIcon.Visible = true;
        }

        public void ScaleImage(int animateDelay)
        {
            if (_image == null)
                return;

            _imageScale = 100;

            var scale_width = _image.Width;
            var scale_height = _image.Height;

            while (scale_width > Width * 80 / 100 || scale_height > Height * 80 / 100)
            {
                scale_width -= _image.Width * 10 / 100;
                scale_height -= _image.Height * 10 / 100;
                _imageScale -= 10;
            }

            ScaleLbl.Content = $"{_imageScale}%";

            Utils.Animator.Resize(Viewer, scale_width, scale_height, animateDelay);
        }

        public void OpenImage(string path)
        {
            if (_image != null)
                _image.Dispose();

            _image = new MagickImage(path);

            NameLbl.Visibility = Visibility.Visible;
            NameLbl.Content = _image.FileName.Split('\\')[Regex.Matches(_image.FileName, @"\\").Count];
            SizeLbl.Visibility = Visibility.Visible;
            SizeLbl.Content = $"{_image.Width} x {_image.Height}";
            ScaleLbl.Visibility = Visibility.Visible;

            NoneContentGrid.Visibility = Visibility.Collapsed;

            Viewer.Source = _image.ToBitmapSource();
            ScaleImage(500);

            ScaleLbl.Content = $"{_imageScale}%";

            if (Toolbar.Visibility == Visibility.Collapsed)
            {
                Toolbar.Margin = new Thickness(0, 0, 0, 0);
                Toolbar.Opacity = 0;
                Toolbar.Visibility = Visibility.Visible;

                Utils.Animator.Move(Toolbar, new Thickness(0, 0, 0, 30), 500);
                Utils.Animator.Opacity(Toolbar, 1, 1000);
            }
        }

        private void Zoom(int delta, double x, double y)
        {
            if (_image == null)
                return;

            _imageIsZoomed = true;
            _imageScale += delta;

            ScaleLbl.Content = $"{_imageScale}%";

            Viewer.Width += _image.Width * delta / 100;
            Viewer.Height += _image.Height * delta / 100;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _image.Dispose();
            _notifyIcon.Dispose();
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            var filePath = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];

            OpenImage(filePath);
        }

        private void Window_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            Point mousePos = e.GetPosition(this);

            if (e.Delta > 0)
                Zoom(10, mousePos.X, mousePos.Y);
            else
                Zoom(-10, mousePos.X, mousePos.Y);
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_image == null || _imageIsZoomed)
                return;

            ScaleImage(250);
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();

            if (dialog.ShowDialog() is true)
                OpenImage(dialog.FileName);
        }

        private void ZoomIn_Click(object sender, RoutedEventArgs e)
        {
            Zoom(10, 0, 0);
        }

        private void ZoomRefresh_Click(object sender, RoutedEventArgs e)
        {
            _imageIsZoomed = false;

            ScaleImage(250);
        }

        private void ZoomOut_Click(object sender, RoutedEventArgs e)
        {
            Zoom(-10, 0, 0);
        }

        private void RotateRight_Click(object sender, RoutedEventArgs e)
        {
            _image.Rotate(90);

            Viewer.Source = _image.ToBitmapSource();
        }

        private void RotateLeft_Click(object sender, RoutedEventArgs e)
        {
            _image.Rotate(-90);

            Viewer.Source = _image.ToBitmapSource();
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            _image.Dispose();
            _notifyIcon.Dispose();

            Environment.Exit(0);
        }
    }
}
