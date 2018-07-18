using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PicPrompt.Resources.Controls
{
    public class ZoomControl : Border
    {
        private UIElement _element;
        private Point _pan;
        private Point _previous;

        public bool EnableZoom
        {
            get => (bool)GetValue(EnableZoomProperty);
            set => SetValue(EnableZoomProperty, value);
        }

        public static DependencyProperty EnableZoomProperty =
            DependencyProperty.Register("EnableZoom", typeof(bool), typeof(ZoomControl));

        public bool EnablePan
        {
            get => (bool)GetValue(EnablePanProperty);
            set => SetValue(EnablePanProperty, value);
        }

        public static DependencyProperty EnablePanProperty = 
            DependencyProperty.Register("EnablePan", typeof(bool), typeof(ZoomControl));

        public override UIElement Child
        {
            get => base.Child;
            set
            {
                Attach(value);
                base.Child = value;
            }
        }

        private TranslateTransform GetTranslateTransform(UIElement element) => (TranslateTransform)((TransformGroup)element.RenderTransform).Children.First(tr => tr is TranslateTransform);
        private ScaleTransform GetScaleTransform(UIElement element) => (ScaleTransform)((TransformGroup)element.RenderTransform).Children.First(tr => tr is ScaleTransform);

        public ZoomControl()
            : base()
        {
        }

        public void Element_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (_element != null && EnableZoom)
                ZoomTo(e.Delta > 0 ? 0.2 : -0.2, e.GetPosition(_element));
        }

        public void Element_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_element != null && EnablePan)
            {
                var translateTransform = GetTranslateTransform(_element);

                _previous = e.GetPosition(this);
                _pan = new Point(translateTransform.X, translateTransform.Y);
                _element.CaptureMouse();
            }
        }

        public void Element_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_element != null && EnablePan)
                _element.ReleaseMouseCapture();
        }

        public void Element_MouseMove(object sender, MouseEventArgs e)
        {
            if (_element != null && EnablePan && _element.IsMouseCaptured)
            {
                var translateTransform = GetTranslateTransform(_element);
                Vector v = _previous - e.GetPosition(this);
                translateTransform.X = _pan.X - v.X;
                translateTransform.Y = _pan.Y - v.Y;
            }
        }

        public void Attach(UIElement element)
        {
            if (element != null)
            {
                _element = element;

                var transformGroup = new TransformGroup();

                var scaleTransform = new ScaleTransform();
                transformGroup.Children.Add(scaleTransform);

                var translateTransform = new TranslateTransform();
                transformGroup.Children.Add(translateTransform);

                _element.RenderTransform = transformGroup;
                _element.RenderTransformOrigin = new Point();
                _element.MouseWheel += Element_MouseWheel;
                _element.MouseLeftButtonDown += Element_MouseLeftButtonDown;
                _element.MouseLeftButtonUp += Element_MouseLeftButtonUp;
                _element.MouseMove += Element_MouseMove;
            }
        }

        public void Reset()
        {
            if (_element != null)
            {
                var scaleTransform = GetScaleTransform(_element);
                scaleTransform.ScaleX = 1.0;
                scaleTransform.ScaleY = 1.0;

                var translateTransform = GetTranslateTransform(_element);
                translateTransform.X = 0.0;
                translateTransform.Y = 0.0;

                _previous = new Point();
                _pan = new Point();
            }
        }

        public void Zoom(double delta)
        {
            if (_element != null)
            {
                var scaleTransform = GetScaleTransform(_element);
                var translateTransform = GetTranslateTransform(_element);

                if ((scaleTransform.ScaleX + delta) < 0.2 || (scaleTransform.ScaleY + delta) < 0.2)
                    return;

                if (_previous == new Point())
                    _previous = new Point(_element.RenderSize.Width / 2, _element.RenderSize.Height / 2);

                double absoluteX = _previous.X * scaleTransform.ScaleX + translateTransform.X;
                double absoluteY = _previous.Y * scaleTransform.ScaleY + translateTransform.Y;

                scaleTransform.ScaleX += delta;
                scaleTransform.ScaleY += delta;

                translateTransform.X = absoluteX - _previous.X * scaleTransform.ScaleX;
                translateTransform.Y = absoluteY - _previous.Y * scaleTransform.ScaleY;
            }
        }

        public void ZoomTo(double delta, Point position)
        {
            if (_element != null)
            {
                var scaleTransform = GetScaleTransform(_element);
                var translateTransform = GetTranslateTransform(_element);

                if ((scaleTransform.ScaleX + delta) < 0.2 || (scaleTransform.ScaleY + delta) < 0.2)
                    return;

                _previous = position;

                double absoluteX = position.X * scaleTransform.ScaleX + translateTransform.X;
                double absoluteY = position.Y * scaleTransform.ScaleY + translateTransform.Y;

                scaleTransform.ScaleX += delta;
                scaleTransform.ScaleY += delta;

                translateTransform.X = absoluteX - position.X * scaleTransform.ScaleX;
                translateTransform.Y = absoluteY - position.Y * scaleTransform.ScaleY;
            }
        }

        public void ZoomTo(double delta, double x, double y)
        {
            ZoomTo(delta, new Point(x, y));
        }
    }
}
