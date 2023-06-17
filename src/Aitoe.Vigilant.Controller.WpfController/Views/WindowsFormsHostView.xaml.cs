using Aitoe.Vigilant.Controller.WpfController.Infra;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace Aitoe.Vigilant.Controller.WpfController.Views
{
    //http://stackoverflow.com/questions/977729/wpf-windowsformshost-sizing
    //http://stackoverflow.com/questions/20386415/hosting-external-exe-in-wpf
    /// <summary>
    /// Interaction logic for WindowsFormsHostView.xaml
    /// </summary>
    public partial class WindowsFormsHostView : UserControl
    {
        
        public ProcessWindowStyle PWS { get; set; }

        private Process _vigilantProcess;       

        public static readonly DependencyProperty VigilantProcInfoProperty =
            DependencyProperty.Register("VigilantProcInfo", typeof(ProcInfo), typeof(WindowsFormsHostView), new
            PropertyMetadata(null, new PropertyChangedCallback(OnVigilantProcInfoChanged)));

        public ProcInfo VigilantProcInfo
        {
            get { return (ProcInfo)GetValue(VigilantProcInfoProperty); }
            set { SetValue(VigilantProcInfoProperty, value); }
        }

        private static void OnVigilantProcInfoChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var formsHostView = d as WindowsFormsHostView;
            formsHostView.OnVigilantProcInfoChanged(e);
        }

        private void OnVigilantProcInfoChanged(DependencyPropertyChangedEventArgs e)
        {
            StartVigilantProcess();
            //this.FormsPanel.Paint
        }

        public WindowsFormsHostView()
        {
            InitializeComponent();
        }        
        public bool StartVigilantProcess()
        {
            try
            {
                if (VigilantProcInfo == null)
                    return false;
                
                if (VigilantProcInfo.ProcId == 0)
                    return false;

                if (!Process.GetProcesses().Any(x => x.Id == VigilantProcInfo.ProcId))
                    return false;// Try find the process with the given id. If not found, then possibley its already dead.

                _vigilantProcess = Process.GetProcessById(VigilantProcInfo.ProcId);
                
                int style = PInvokeDefs.GetWindowLong(VigilantProcInfo.ProcMainWindowHandle, PInvokeDefs.GWL_STYLE);
                style = style & ~((int)PInvokeDefs.WS_CAPTION) & ~((int)PInvokeDefs.WS_THICKFRAME);
                style |= ((int)PInvokeDefs.WS_CHILD);
                //style = style & ~((int)PInvokeDefs.WS_CAPTION);
                //style &= ~((int)PInvokeDefs.WS_THICKFRAME);
                //style &= ~((int)PInvokeDefs.WS_BORDER);
                PInvokeDefs.SetWindowLong(VigilantProcInfo.ProcMainWindowHandle, PInvokeDefs.GWL_STYLE, new IntPtr(style));
                PInvokeDefs.SetParent(VigilantProcInfo.ProcMainWindowHandle, FormsPanel.Handle);
                PInvokeDefs.MoveWindow(VigilantProcInfo.ProcMainWindowHandle, 0, 0, (int)this.Width, (int)this.Height, true);
                //PInvokeDefs.SendMessage(VigilantProcess.MainWindowHandle, 0x0112, new IntPtr(0xF020), new IntPtr(0));
                //FormsPanel.AutoSize = true;
                //PInvokeDefs.SetWindowPos(VigilantProcess.MainWindowHandle, IntPtr.Zero, 0, 0, FormsPanel.Width, FormsPanel.Height, 0x0040);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString()); //Need to do proper logging.
                return false;
            }
        }

        private void FormsPanel_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            CallWin32SetWindowPos();
        }

        private void CallWin32SetWindowPos()
        {
            try
            {
                if (_vigilantProcess == null)
                    return;
                if (VigilantProcInfo.ProcMainWindowHandle != IntPtr.Zero)
                    //PInvokeDefs.MoveWindow(VigilantProcess.MainWindowHandle, 0, 0, (int)Width, (int)Height, true);
                    PInvokeDefs.SetWindowPos(VigilantProcInfo.ProcMainWindowHandle, IntPtr.Zero, 0, 0, FormsPanel.Width, FormsPanel.Height, 0x0040);
                else
                    MessageBox.Show("Problem!!!. Process MainWindowHandle is zero for the following Vigilant Process " + Environment.NewLine + VigilantProcInfo.ToString());// Need to do proper logging.
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message.ToString());// Need to do proper logging.
            }

        }
    }
}
