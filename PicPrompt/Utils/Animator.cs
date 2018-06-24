using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace PicPrompt.Utils
{
    public class Animator
    {
        public static async Task<bool> Resize(object target, double width, double height, int delay)
        {
            var obj = target as FrameworkElement;

            if (obj != null)
            {
                obj.BeginAnimation(FrameworkElement.WidthProperty, new DoubleAnimation
                {
                    To = width,
                    Duration = TimeSpan.FromMilliseconds(delay),
                });

                obj.BeginAnimation(FrameworkElement.HeightProperty, new DoubleAnimation
                {
                    To = width,
                    Duration = TimeSpan.FromMilliseconds(delay),
                });

                await Task.Delay(delay);

                obj.Width = width;
                obj.Height = height;

                obj.BeginAnimation(FrameworkElement.WidthProperty, null);
                obj.BeginAnimation(FrameworkElement.HeightProperty, null);

                return true;
            }

            return false;
        }

        public static async Task<bool> Move(object target, Thickness to, int delay)
        {
            var obj = target as FrameworkElement;

            if (obj != null)
            {
                obj.BeginAnimation(FrameworkElement.MarginProperty, new ThicknessAnimation
                {
                    To = to,
                    Duration = TimeSpan.FromMilliseconds(delay),
                });

                await Task.Delay(delay);

                obj.Margin = to;

                obj.BeginAnimation(FrameworkElement.MarginProperty, null);

                return true;
            }

            return false;
        }

        public static async Task<bool> Opacity(object target, double opacity, int delay)
        {
            var obj = target as FrameworkElement;

            if (obj != null)
            {
                obj.BeginAnimation(FrameworkElement.OpacityProperty, new DoubleAnimation
                {
                    To = opacity,
                    Duration = TimeSpan.FromMilliseconds(delay),
                });

                await Task.Delay(delay);

                obj.Opacity = opacity;

                obj.BeginAnimation(FrameworkElement.OpacityProperty, null);

                if (opacity <= 0)
                    obj.Visibility = Visibility.Collapsed;

                return true;
            }

            return false;
        }
    }
}
