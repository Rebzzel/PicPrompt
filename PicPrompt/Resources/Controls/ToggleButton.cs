using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace PicPrompt.Resources.Controls
{
    public class ToggleButton : System.Windows.Controls.Primitives.ToggleButton
    {
        public Brush HoverBrush
        {
            get => (Brush)GetValue(HoverBrushProperty);
            set => SetValue(HoverBrushProperty, value);
        }

        public static DependencyProperty HoverBrushProperty =
            DependencyProperty.Register("HoverBrush", typeof(Brush), typeof(ToggleButton));

        public Brush PressBrush
        {
            get => (Brush)GetValue(PressBrushProperty);
            set => SetValue(PressBrushProperty, value);
        }

        public static DependencyProperty PressBrushProperty =
            DependencyProperty.Register("PressBrush", typeof(Brush), typeof(ToggleButton));

        public Brush CheckedBrush
        {
            get => (Brush)GetValue(CheckedBrushProperty);
            set => SetValue(CheckedBrushProperty, value);
        }

        public static DependencyProperty CheckedBrushProperty =
            DependencyProperty.Register("CheckedBrush", typeof(Brush), typeof(ToggleButton));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(ToggleButton));

        public int AnimationDelay
        {
            get => (int)GetValue(AnimationDelayProperty);
            set => SetValue(AnimationDelayProperty, value);
        }

        public static DependencyProperty AnimationDelayProperty =
            DependencyProperty.Register("AnimationDelay", typeof(int), typeof(ToggleButton), new PropertyMetadata(0));

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);

            if (IsChecked == true)
                return;

            Background.BeginAnimation(SolidColorBrush.ColorProperty, new ColorAnimation
            {
                To = GetColorFromBrush(HoverBrush),
                Duration = TimeSpan.FromMilliseconds(AnimationDelay)
            });
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);

            if (IsChecked == true)
                return;

            Background.BeginAnimation(SolidColorBrush.ColorProperty, null);

            Background.BeginAnimation(SolidColorBrush.ColorProperty, new ColorAnimation
            {
                From = GetColorFromBrush(HoverBrush),
                To = GetColorFromBrush(Background),
                Duration = TimeSpan.FromMilliseconds(AnimationDelay)
            });
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            if (IsChecked == true)
                return;

            Background.BeginAnimation(SolidColorBrush.ColorProperty, null);

            Background.BeginAnimation(SolidColorBrush.ColorProperty, new ColorAnimation
            {
                From = GetColorFromBrush(HoverBrush),
                To = GetColorFromBrush(PressBrush),
                Duration = TimeSpan.FromMilliseconds(AnimationDelay)
            });
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            if (IsChecked == true)
                return;

            Background.BeginAnimation(SolidColorBrush.ColorProperty, null);

            Background.BeginAnimation(SolidColorBrush.ColorProperty, new ColorAnimation
            {
                From = GetColorFromBrush(PressBrush),
                To = GetColorFromBrush(HoverBrush),
                Duration = TimeSpan.FromMilliseconds(AnimationDelay)
            });
        }

        protected override void OnClick()
        {
            base.OnClick();

            if (IsChecked == true)
            {
                Background.BeginAnimation(SolidColorBrush.ColorProperty, null);

                Background.BeginAnimation(SolidColorBrush.ColorProperty, new ColorAnimation
                {
                    To = GetColorFromBrush(CheckedBrush),
                    Duration = TimeSpan.FromMilliseconds(0)
                });
            }
            else
            {
                Background.BeginAnimation(SolidColorBrush.ColorProperty, null);

                Background.BeginAnimation(SolidColorBrush.ColorProperty, new ColorAnimation
                {
                    From = GetColorFromBrush(PressBrush),
                    To = GetColorFromBrush(HoverBrush),
                    Duration = TimeSpan.FromMilliseconds(AnimationDelay)
                });
            }
        }

        public Color GetColorFromBrush(Brush brush)
        {
            return brush == null ? Colors.Transparent : ((SolidColorBrush)brush).Color;
        }
    }
}
