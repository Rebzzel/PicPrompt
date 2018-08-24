using ImageMagick;
using Microsoft.Win32;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace PicPrompt
{
    public partial class MainWindow : Window, IDisposable
    {
        private Resources.Controls.Menu _menu;
        private MagickImage _image;
        private bool _imageIsEdited;

        public MainWindow()
        {
            InitializeComponent();

            _menu = new Resources.Controls.Menu();
        }

        public void Dispose()
        {
            if (_image != null)
            {
                _image.Dispose();
                _image = null;
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

        private void Window_Drop(object sender, DragEventArgs e)
        {
            var filePath = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];

            OpenImage(filePath);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close_Click(null, null);

            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.O)
                Open_Click(null, null);

            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.S)
                SaveAs_Click(null, null);

            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.OemPlus)
                ZoomIn_Click(null, null);

            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.D0)
                Refresh_Click(null, null);

            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.OemMinus)
                ZoomOut_Click(null, null);

            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.A)
                RotateLeft_Click(null, null);

            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.D)
                RotateRight_Click(null, null);

            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.V)
            {
                if (Clipboard.ContainsFileDropList() == true)
                {
                    var filePath = ((string[])Clipboard.GetData(DataFormats.FileDrop))[0];
                    OpenImage(filePath);
                } else if (Clipboard.ContainsImage() == true)
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        var encoder = new BmpBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(Clipboard.GetImage()));
                        encoder.Save(stream);
                        OpenImage(new System.Drawing.Bitmap(stream));
                    }
                }
            }
        }

        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            _imageIsEdited = true;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_image != null && !_imageIsEdited)
            {
                Viewer.Width = _image.Width > Width ? Width : _image.Width;
                Viewer.Height = _image.Height > Height ? Height : _image.Height;
            }
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();

            if (dialog.ShowDialog() == true)
                OpenImage(dialog.FileName);
        }

        private void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog();
        }

        private void ZoomIn_Click(object sender, RoutedEventArgs e)
        {
            _imageIsEdited = true;

            ViewerGrid.Zoom(0.2);
        }

        private void ZoomOut_Click(object sender, RoutedEventArgs e)
        {
            _imageIsEdited = true;

            ViewerGrid.Zoom(-0.2);
        }

        private void RotateRight_Click(object sender, RoutedEventArgs e)
        {
            if (_image != null)
            {
                _image.Rotate(90);

                Viewer.Source = _image.ToBitmapSource();
            }
        }

        private void RotateLeft_Click(object sender, RoutedEventArgs e)
        {
            if (_image != null)
            {
                _image.Rotate(-90);

                Viewer.Source = _image.ToBitmapSource();
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            _imageIsEdited = false;

            ViewerGrid.Reset();
            Viewer.Width = _image.Width > Width ? Width : _image.Width;
            Viewer.Height = _image.Height > Height ? Height : _image.Height;
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Options_Click(object sender, RoutedEventArgs e)
        {
            _menu.Show(MainGrid);
            _menu.ChangePage(0);
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            _menu.Show(MainGrid);
            _menu.ChangePage(1);
        }

        public void OpenImage(string path)
        {
            ViewerGrid.Reset();

            if (_image != null)
            {
                _image.Dispose();
                _image = null;
            }

            _image = new MagickImage(path);

            NoneContentGrid.Visibility = Visibility.Collapsed;

            foreach (FrameworkElement item in ((StackPanel)TitleBar.Children[0]).Children)
            {
                if (item.Name == "Separator4" || item.Name == "ScaleLbl")
                    continue;

                item.Visibility = Visibility.Visible;
            }

            NameLbl.Content = _image.FileName.Split('\\')[Regex.Matches(_image.FileName, @"\\").Count];
            SizeLbl.Content = $"{_image.Width} x {_image.Height}";

            foreach (FrameworkElement item in MainMenu.Items)
            {
                if ((item as MenuItem) != null)
                    item.IsEnabled = true;
            }

            Viewer.Source = _image.ToBitmapSource();
            Viewer.Width = _image.Width > Width ? Width : _image.Width;
            Viewer.Height = _image.Height > Height ? Height : _image.Height;

            if (Toolbar.Visibility == Visibility.Collapsed)
            {
                Toolbar.Margin = new Thickness(0, 0, 0, 0);
                Toolbar.Opacity = 0;
                Toolbar.Visibility = Visibility.Visible;

                Utils.Animator.Margin(Toolbar, new Thickness(0, 0, 0, 30), new Utils.Animator.AnimationOptions { Duration = TimeSpan.FromMilliseconds(500), AccelerationRatio = 0.25, DecelerationRatio = 0.75 });
                Utils.Animator.Opacity(Toolbar, 1, new Utils.Animator.AnimationOptions { Duration = TimeSpan.FromMilliseconds(1000), AccelerationRatio = 0.25, DecelerationRatio = 0.75 });
            }
        }

        public void OpenImage(System.Drawing.Bitmap bitmap)
        {
            ViewerGrid.Reset();

            if (_image != null)
            {
                _image.Dispose();
                _image = null;
            }

            _image = new MagickImage(bitmap);

            NoneContentGrid.Visibility = Visibility.Collapsed;

            foreach (FrameworkElement item in ((StackPanel)TitleBar.Children[0]).Children)
            {
                if (item.Name == "Separator4" || item.Name == "ScaleLbl")
                    continue;

                item.Visibility = Visibility.Visible;
            }

            NameLbl.Content = _image.FileName.Split('\\')[Regex.Matches(_image.FileName, @"\\").Count];
            SizeLbl.Content = $"{_image.Width} x {_image.Height}";

            foreach (FrameworkElement item in MainMenu.Items)
            {
                if ((item as MenuItem) != null)
                    item.IsEnabled = true;
            }

            Viewer.Source = _image.ToBitmapSource();
            Viewer.Width = _image.Width > Width ? Width : _image.Width;
            Viewer.Height = _image.Height > Height ? Height : _image.Height;

            if (Toolbar.Visibility == Visibility.Collapsed)
            {
                Toolbar.Margin = new Thickness(0, 0, 0, 0);
                Toolbar.Opacity = 0;
                Toolbar.Visibility = Visibility.Visible;

                Utils.Animator.Margin(Toolbar, new Thickness(0, 0, 0, 30), new Utils.Animator.AnimationOptions { Duration = TimeSpan.FromMilliseconds(500), AccelerationRatio = 0.25, DecelerationRatio = 0.75 });
                Utils.Animator.Opacity(Toolbar, 1, new Utils.Animator.AnimationOptions { Duration = TimeSpan.FromMilliseconds(1000), AccelerationRatio = 0.25, DecelerationRatio = 0.75 });
            }
        }
    }
}
