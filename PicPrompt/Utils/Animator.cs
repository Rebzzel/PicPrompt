using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace PicPrompt.Utils
{
    public class Animator
    {
        public class AnimationOptions
        {
            public Duration Duration;
            public double AccelerationRatio;
            public double DecelerationRatio;

            public AnimationOptions()
            {
                Duration = TimeSpan.FromMilliseconds(0);
                AccelerationRatio = 0.0;
                DecelerationRatio = 0.0;
            }
        }

        public static async Task Size(object target, Size size, AnimationOptions options)
        {
            var obj = target as FrameworkElement;

            if (obj != null)
            {
                obj.BeginAnimation(FrameworkElement.WidthProperty, new DoubleAnimation
                {
                    To = size.Width,
                    Duration = options.Duration,
                    AccelerationRatio = options.AccelerationRatio,
                    DecelerationRatio = options.DecelerationRatio
                });

                obj.BeginAnimation(FrameworkElement.HeightProperty, new DoubleAnimation
                {
                    To = size.Height,
                    Duration = options.Duration,
                    AccelerationRatio = options.AccelerationRatio,
                    DecelerationRatio = options.DecelerationRatio
                });

                await Task.Delay(options.Duration.TimeSpan.Milliseconds);

                obj.BeginAnimation(FrameworkElement.WidthProperty, null);
                obj.BeginAnimation(FrameworkElement.HeightProperty, null);

                obj.Width = size.Width;
                obj.Height = size.Height;
            }
        }

        public static async Task Opacity(object target, double opacity, AnimationOptions options)
        {
            var obj = target as FrameworkElement;

            if (obj != null)
            {
                obj.BeginAnimation(FrameworkElement.OpacityProperty, new DoubleAnimation
                {
                    To = opacity,
                    Duration = options.Duration,
                    AccelerationRatio = options.AccelerationRatio,
                    DecelerationRatio = options.DecelerationRatio
                });

                await Task.Delay(options.Duration.TimeSpan.Milliseconds);

                obj.BeginAnimation(FrameworkElement.OpacityProperty, null);

                obj.Opacity = opacity;
            }
        }

        public static async Task Margin(object target, Thickness to, AnimationOptions options)
        {
            var obj = target as FrameworkElement;

            if (obj != null)
            {
                obj.BeginAnimation(FrameworkElement.MarginProperty, new ThicknessAnimation
                {
                    To = to,
                    Duration = options.Duration,
                    AccelerationRatio = options.AccelerationRatio,
                    DecelerationRatio = options.DecelerationRatio
                });

                await Task.Delay(options.Duration.TimeSpan.Milliseconds);

                obj.BeginAnimation(FrameworkElement.MarginProperty, null);

                obj.Margin = to;
            }
        }

        public static async Task Scale(object target, double x1, double y1, double x2, double y2, AnimationOptions options)
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
                    From = x1,
                    To = x2,
                    Duration = options.Duration,
                    AccelerationRatio = options.AccelerationRatio,
                    DecelerationRatio = options.DecelerationRatio
                });

                scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, new DoubleAnimation
                {
                    From = y1,
                    To = y2,
                    Duration = options.Duration,
                    AccelerationRatio = options.AccelerationRatio,
                    DecelerationRatio = options.DecelerationRatio
                });

                await Task.Delay(options.Duration.TimeSpan.Milliseconds);

                scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, null);
                scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, null);

                scaleTransform.ScaleX = x2;
                scaleTransform.ScaleY = y2;
            }
        }
    }
}
