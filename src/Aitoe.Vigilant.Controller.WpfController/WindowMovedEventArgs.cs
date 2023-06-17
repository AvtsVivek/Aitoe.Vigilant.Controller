using System;
using System.Windows;

namespace Aitoe.Vigilant.Controller.WpfController
{
    // This is not being used. So eventually can be removed.
    public class WindowMovedEventArgs : EventArgs
    {
        public String SomeInfo { get; set; }
        public Rect WindowRectangle { get; set; }

    }
}
