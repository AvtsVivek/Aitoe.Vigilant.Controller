using Aitoe.Vigilant.Controller.WpfController.Infra;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

//http://stackoverflow.com/questions/977729/wpf-windowsformshost-sizing
//http://stackoverflow.com/questions/20386415/hosting-external-exe-in-wpf
namespace Aitoe.Vigilant.Controller.WpfController
{
    /// <summary>
    /// Interaction logic for VigilantCamStream.xaml
    /// </summary>
    public partial class VigilantCamStream : UserControl
    {
        public event EventHandler<WindowMovedEventArgs> WindowMoved;
        public int ColumnNo { get; set; }
        public int RowNo { get; set; }

        private int xLoc;
        private int yLoc;

        //public System.Windows.Forms.Panel

        public VigilantCamStream()
        {
            InitializeComponent();
            //FormsPanel.Resize += FormsPanel_Resize;
            //IsResizeInProgress = false;
            //WindowMoved += VigilantCamStream_WindowMoved;
            
        }

        public System.Windows.Forms.Panel VigilantPanel
        {
            get { return FormsPanel; }
        }


        public int getPanelWidth()
        {
            return FormsPanel.Width;
        }

        public int getPanelHeight()
        {
            return FormsPanel.Height;
        }

        private void FormsPanel_Resize(object sender, EventArgs e)
        {
            
        }

        internal void UnSubscribePaintEvent()
        {
            //FormsPanel.Paint -= FormsPanel_Paint;
        }
        
        //public bool IsResizeInProgress { get; set; }

        public Process VigilantProcess { get; set; }

        public ProcessWindowStyle PWS { get; set; }

        public bool StartVigilatProcess()
        {
            try
            {
                VigilantProcess.StartInfo.WindowStyle = PWS;
                VigilantProcess.Start();

                if (!HasProcessStartedSuccessifully())
                    return false;
                //if (!VigilantProcess.WaitForInputIdle())
                  //  return false;
                //VigilantProcess.Refresh();
                int style = PInvokeDefs.GetWindowLong(VigilantProcess.MainWindowHandle, PInvokeDefs.GWL_STYLE);
                style = style & ~((int)PInvokeDefs.WS_CAPTION) & ~((int)PInvokeDefs.WS_THICKFRAME);
                style |= ((int)PInvokeDefs.WS_CHILD);
                //style = style & ~((int)PInvokeDefs.WS_CAPTION);
                //style &= ~((int)PInvokeDefs.WS_THICKFRAME);
                //style &= ~((int)PInvokeDefs.WS_BORDER);
                PInvokeDefs.SetWindowLong(VigilantProcess.MainWindowHandle, PInvokeDefs.GWL_STYLE, new IntPtr(style));
                PInvokeDefs.SetParent(VigilantProcess.MainWindowHandle, FormsPanel.Handle);
                PInvokeDefs.MoveWindow(VigilantProcess.MainWindowHandle, 0, 0, (int)this.Width, (int)this.Height, true);
                //PInvokeDefs.SendMessage(VigilantProcess.MainWindowHandle, 0x0112, new IntPtr(0xF020), new IntPtr(0));
                //FormsPanel.AutoSize = true;
                //PInvokeDefs.SetWindowPos(VigilantProcess.MainWindowHandle, IntPtr.Zero, 0, 0, FormsPanel.Width, FormsPanel.Height, 0x0040);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return false;
            }
        }

        private bool HasProcessStartedSuccessifully()
        {
            int count = 0;
            //http://stackoverflow.com/a/11747621/1977871
            while ((!VigilantProcess.WaitForInputIdle() || VigilantProcess.MainWindowHandle == IntPtr.Zero || !GetWin32WindowRectangle()) && count < 100)
            {
                count++;
                VigilantProcess.Refresh();
                Thread.Sleep(100);
            }

            if (count > 98)
                return false;
            else
                return true;
        }

        Rectangle win32Rectangle = new Rectangle();

        private void FormsPanel_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            //GetWin32WindowRectangle();

            try
            {
                if (VigilantProcess != null)
                    //PInvokeDefs.MoveWindow(VigilantProcess.MainWindowHandle, 0, 0, (int)Width, (int)Height, true);
                    PInvokeDefs.SetWindowPos(VigilantProcess.MainWindowHandle, IntPtr.Zero, 0, 0, FormsPanel.Width, FormsPanel.Height, 0x0040);

            }
            catch (Exception)
            {

            }


            //if (!GetWin32WindowRectangle())
            //    return;

            //GetWin32WindowRectangle();


            //var movEventArgs = new WindowMovedEventArgs();
            //movEventArgs.SomeInfo = win32Rectangle.X.ToString() + " " + win32Rectangle.Y.ToString() + "; " + win32Rectangle.Width.ToString() + " " + win32Rectangle.Height.ToString();

            //EventHandler<WindowMovedEventArgs> temp = WindowMoved;
            //if (temp != null) temp(this, movEventArgs);


            //if (xLoc == 0)
            //{
            //    xLoc = win32Rectangle.X;
            //}
            //else if (xLoc != 0 && xLoc == win32Rectangle.X)
            //{

            //}
            //else //if (xLoc != 0 && xLoc != win32Rectangle.X)
            //{
            //    xLoc = win32Rectangle.X;
            //    if (VigilantProcess != null)
            //        PInvokeDefs.MoveWindow(VigilantProcess.MainWindowHandle, 0, 0, (int)win32Rectangle.Width, (int)win32Rectangle.Height, true);
            //}

            //if (VigilantProcess != null)
            //    PInvokeDefs.MoveWindow(VigilantProcess.MainWindowHandle, 0, 0, (int)win32Rectangle.Width, (int)win32Rectangle.Height, true);

            //EventHandler<WindowMovedEventArgs> temp = WindowMoved;
            //if (temp != null) temp(this, movEventArgs);
        }

        private bool GetWin32WindowRectangle()
        {
            if (VigilantProcess == null || VigilantProcess.HasExited)
                return false;
            var processHandleRef = new HandleRef(VigilantProcess, VigilantProcess.MainWindowHandle);
            PInvokeDefs.RECT rct;

            if (!PInvokeDefs.GetWindowRect(processHandleRef, out rct))
                return false;

            win32Rectangle.X = rct.Left;
            win32Rectangle.Y = rct.Top;
            win32Rectangle.Width = rct.Right - rct.Left;
            win32Rectangle.Height = rct.Bottom - rct.Top;
            return true;
        }


        private void VigilantCamStream_WindowMoved(object sender, WindowMovedEventArgs e)
        {
            //GetWin32WindowRectangle();
            //e.SomeInfo = win32Rectangle.X.ToString() + " " + win32Rectangle.Y.ToString() + "; " + win32Rectangle.Width.ToString() + " " + win32Rectangle.Height.ToString();
            //EventHandler<WindowMovedEventArgs> temp = WindowMoved;
            //if (temp != null) temp(this, e);
        }
    }
}
