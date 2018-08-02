using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace PicPrompt.Resources.Controls
{
    public class Button : System.Windows.Controls.Button
    {
        public Brush HoverBrush
        {
            get => (Brush)GetValue(HoverBrushProperty);
            set => SetValue(HoverBrushProperty, value);
        }

        public static DependencyProperty HoverBrushProperty =
            DependencyProperty.Register("HoverBrush", typeof(Brush), typeof(Button));

        public Brush PressBrush
        {
            get => (Brush)GetValue(PressBrushProperty);
            set => SetValue(PressBrushProperty, value);
        }

        public static DependencyProperty PressBrushProperty = 
            DependencyProperty.Register("PressBrush", typeof(Brush), typeof(Button));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(Button));

        public int AnimationDelay
        {
            get => (int)GetValue(AnimationDelayProperty);
            set => SetValue(AnimationDelayProperty, value);
        }

        public static DependencyProperty AnimationDelayProperty =
            DependencyProperty.Register("AnimationDelay", typeof(int), typeof(Button), new PropertyMetadata(0));

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);

            Background.BeginAnimation(SolidColorBrush.ColorProperty, new ColorAnimation
            {
                To = GetColorFromBrush(HoverBrush),
                Duration = TimeSpan.FromMilliseconds(AnimationDelay)
            });
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);

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

            Background.BeginAnimation(SolidColorBrush.ColorProperty, null);

            Background.BeginAnimation(SolidColorBrush.ColorProperty, new ColorAnimation
            {
                From = GetColorFromBrush(PressBrush),
                To = GetColorFromBrush(HoverBrush),
                Duration = TimeSpan.FromMilliseconds(AnimationDelay)
            });
        }

        public Color GetColorFromBrush(Brush brush)
        {
            return brush == null ? Colors.Transparent : ((SolidColorBrush)brush).Color;
        }
    }
}
