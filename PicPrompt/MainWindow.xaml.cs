using ImageMagick;
using LitJson;
using Microsoft.Win32;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PicPrompt
{
    public partial class MainWindow : Window, IDisposable
    {
        private static Window BackgroundWindow = new Window();

        public JsonData Configuration;

        private MagickImage _image;

        public MainWindow()
        {
            InitializeComponent();
            InitializeConfiguration();
        }

        public void Dispose()
        {
            if (_image != null)
            {
                _image.Dispose();
                _image = null;
            }
        }

        public void InitializeConfiguration()
        {
            if (!File.Exists("PicPrompt.json"))
            {
                File.WriteAllText("PicPrompt.json", 
@"{
    ""allow-background-work"": true
}");
            }

            Configuration = JsonMapper.ToObject(File.ReadAllText("PicPrompt.json"));
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            if (!(bool)Configuration["allow-background-work"])
                BackgroundWindow.Close();

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
                ZoomRefresh_Click(null, null);

            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.OemMinus)
                ZoomOut_Click(null, null);

            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.A)
                RotateLeft_Click(null, null);

            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.D)
                RotateRight_Click(null, null);
        }

        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
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
        }

        private void ZoomRefresh_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ZoomOut_Click(object sender, RoutedEventArgs e)
        {
        }

        private void RotateRight_Click(object sender, RoutedEventArgs e)
        {
            if (_image == null)
                return;

            _image.Rotate(90);

            Viewer.Source = _image.ToBitmapSource();
        }

        private void RotateLeft_Click(object sender, RoutedEventArgs e)
        {
            if (_image == null)
                return;

            _image.Rotate(-90);

            Viewer.Source = _image.ToBitmapSource();
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        public void OpenImage(string path)
        {
            Dispose();

            _image = new MagickImage(path);

            NoneContentGrid.Visibility = Visibility.Collapsed;

            foreach (FrameworkElement item in ((StackPanel)TitleBar.Children[0]).Children)
            {
                item.Visibility = Visibility.Visible;
            }

            NameLbl.Content = _image.FileName.Split('\\')[Regex.Matches(_image.FileName, @"\\").Count];
            SizeLbl.Content = $"{_image.Width} x {_image.Height}";

            foreach (FrameworkElement item in Context.Items)
            {
                if ((item as MenuItem) != null)
                    item.IsEnabled = true;
            }

            Viewer.Source = _image.ToBitmapSource();
            Viewer.Width = _image.Width;
            Viewer.Height = _image.Height;

            if (Toolbar.Visibility == Visibility.Collapsed)
            {
                Toolbar.Margin = new Thickness(0, 0, 0, 0);
                Toolbar.Opacity = 0;
                Toolbar.Visibility = Visibility.Visible;

                Utils.Animator.Move(Toolbar, new Thickness(0, 0, 0, 30), 500);
                Utils.Animator.Opacity(Toolbar, 1, 1000);
            }
        }

        public void Zoom(double ratio, double x, double y)
        {

        }
    }
}
