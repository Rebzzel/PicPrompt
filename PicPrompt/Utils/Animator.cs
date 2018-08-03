using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
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

        public static async Task<bool> Move(object target, Thickness to, int delay, double accelerationRatio = 0, double decelerationRatio = 0)
        {
            var obj = target as FrameworkElement;

            if (obj != null)
            {
                obj.BeginAnimation(FrameworkElement.MarginProperty, new ThicknessAnimation
                {
                    To = to,
                    Duration = TimeSpan.FromMilliseconds(delay),
                    AccelerationRatio = accelerationRatio,
                    DecelerationRatio = decelerationRatio
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

        public static async Task<bool> Scale(object target, double fromX, double fromY,  double toX, double toY, int delay)
        {
            var obj = target as FrameworkElement;

            if (obj != null)
            {
                var transformGroup = new TransformGroup();

                var scaleTransform = new ScaleTransform();
                transformGroup.Children.Add(scaleTransform);

                obj.RenderTransform = transformGroup;
                obj.RenderTransformOrigin = new Point(0.5, 0.5);

                scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, new DoubleAnimation
                {
                    From = fromX,
                    To = toX,
                    Duration = TimeSpan.FromMilliseconds(delay),
                });

                scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, new DoubleAnimation
                {
                    From = fromY,
                    To = toY,
                    Duration = TimeSpan.FromMilliseconds(delay),
                });

                await Task.Delay(delay);

                scaleTransform.ScaleX = toX;
                scaleTransform.ScaleY = toY;

                scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, null);
                scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, null);

                return true;
            }

            return false;
        }
    }
}
