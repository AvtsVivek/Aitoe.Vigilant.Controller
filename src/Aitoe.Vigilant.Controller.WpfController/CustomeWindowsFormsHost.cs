using System;
using System.Threading;
using System.Windows;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;

namespace Aitoe.Vigilant.Controller.WpfController
{
    public class CustomeWindowsFormsHost : WindowsFormsHost
    {
        //public event EventHandler<WindowMovedEventArgs> WindowMoved;

        protected override Size ArrangeOverride(Size finalSize)
        {
            return base.ArrangeOverride(finalSize);
        }

        protected override void OnChildDesiredSizeChanged(UIElement child)
        {
            base.OnChildDesiredSizeChanged(child);
        }

        protected override Geometry GetLayoutClip(Size layoutSlotSize)
        {
            return base.GetLayoutClip(layoutSlotSize);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            return base.MeasureOverride(constraint);
        }

        protected override Vector ScaleChild(Vector newScale)
        {
            return base.ScaleChild(newScale);
        }

        protected override void OnManipulationBoundaryFeedback(ManipulationBoundaryFeedbackEventArgs e)
        {
            base.OnManipulationBoundaryFeedback(e);
        }

        protected override void OnManipulationStarted(ManipulationStartedEventArgs e)
        {
            base.OnManipulationStarted(e);
        }

        protected override void OnManipulationCompleted(ManipulationCompletedEventArgs e)
        {
            base.OnManipulationCompleted(e);
        }

        protected override void OnWindowPositionChanged(Rect rcBoundingBox)
        {
            ////EventHandler<WindowMovedEventArgs> temp = Volatile.Read(ref WindowMoved);
            //EventHandler<WindowMovedEventArgs> temp = WindowMoved;
            //var e = new WindowMovedEventArgs();
            //e.SomeInfo = "Moved PC" + i++;
            //e.WindowRectangle = rcBoundingBox;
            //if (temp != null) temp(this, e);
            //base.OnWindowPositionChanged(rcBoundingBox);
        }
        public static int i = 0;
        protected override IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            //switch (msg)
            //{
            //    case 0x0046: //WINDOWPOSCHANGING
            //        {
                        
            //        }
            //        break;
            //    case 0x0214: // WM_SIZING
            //        {

            //        }
            //        break;
            //    case 0x0005: // WM_SIZE
            //        {

            //        }
            //        break;
            //    case 0x0032: 
            //        {

            //        }
            //        break;
            //    case 0x0070:
            //        {
            //            //EventHandler<WindowMovedEventArgs> temp = WindowMoved;
            //            //var e = new WindowMovedEventArgs();
            //            ////e.WindowRectangle = rcBoundingBox;
            //            //e.SomeInfo = "Moved " + i++;
            //            //if (temp != null) temp(this, e);
            //        }
            //        break;

            //}
            return base.WndProc(hwnd, msg, wParam, lParam, ref handled);
        }
    }
}
