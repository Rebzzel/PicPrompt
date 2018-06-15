using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace PicPrompt.Resources.Controls
{
    class ClickableLabel : Label
    {
        public static readonly RoutedEvent ClickEvent = ButtonBase.ClickEvent.AddOwner(typeof(ClickableLabel));

        public event RoutedEventHandler Click
        {
            add => AddHandler(ClickEvent, value);
            remove => RemoveHandler(ClickEvent, value);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            CaptureMouse();
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            if (IsMouseCaptured)
            {
                ReleaseMouseCapture();

                if (IsMouseOver)
                    RaiseEvent(new RoutedEventArgs(ClickEvent, this));
            }
        }
    }
}
