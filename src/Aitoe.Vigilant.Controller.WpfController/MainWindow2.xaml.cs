//using System.Diagnostics;
//using System.Windows;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Windows.Controls;
//using Aitoe.Vigilant.Controller.WpfController.Infra;

//namespace Aitoe.Vigilant.Controller.WpfController
//{
//    /// <summary>
//    /// Interaction logic for MainWindow.xaml
//    /// </summary>
//    public partial class MainWindow2 : Window
//    {
//        //string sVigilantPath = @"E:\Trials\CPP\QT\QtTrials\QtWidgetsAppT2\BuildDebug\debug\QtWidgetsAppT2.exe";
//        //string sVigilantPath = @"E:\Trials\CPP\QT\QtTrials\QtWidgetsAppT1\BuildDebug\debug\QtWidgetsAppT1.exe";
//        string sVigilantPath = @"D:\AitoeVSTS\Vigilant\aitoe_red\build\Vigilant\Vigilant.exe";
//        //string sVigilantPath = @"D:\Win32Project1\Debug\Win32Project1.exe";

//        public MainWindow2()
//        {
//            InitializeComponent();

//            var tempProcess = new Process();
//            tempProcess.StartInfo = new ProcessStartInfo(sVigilantPath);
//            var hostTemp = new VigilantCamStream();
//            hostTemp.UnSubscribePaintEvent();
//            hostTemp.VigilantProcess = tempProcess;
//            hostTemp.PWS = ProcessWindowStyle.Minimized;
//            hostTemp.StartVigilatProcess();
//            //System.Threading.Thread.Sleep(500);
//            if (tempProcess != null && !tempProcess.HasExited)
//                tempProcess.Kill();
//        }

//        private List<Process> vigilantProcesses = new List<Process>();
//        Process p, p2, p3;

//        private void Window_Loaded(object sender, RoutedEventArgs e)
//        {          
            
//        }

//        private void btnReset_Click(object sender, RoutedEventArgs e)
//        {
//            PInvokeDefs.MoveWindow(p.MainWindowHandle, 0, 0, 670, 500, true);
//        }
        
//        private void btnResize_Click(object sender, RoutedEventArgs e)
//        {
//            //Host.IsResizeInProgress = true;            
//        }

//        private void btnApply_Click(object sender, RoutedEventArgs e)
//        {
//            //Host.IsResizeInProgress = false;
//        }

//        private void btnAdd_Click(object sender, RoutedEventArgs e)
//        {
//            p = new Process();
//            p.StartInfo = new ProcessStartInfo(sVigilantPath);
//            var host = new VigilantCamStream();
//            host.Width = 200;
//            host.Height = 200;
//            host.VigilantProcess = p;
//            host.PWS = ProcessWindowStyle.Normal;// Default is ProcessWindowStyle.Normal
//            host.StartVigilatProcess();
//            var lbi = new ListBoxItem();
//            lbi.Content = host;
//            //MyListBox.Items.Add(lbi);
//        }

//        private void btnStart_Click(object sender, RoutedEventArgs e)
//        {
//            p = new Process();
//            p.StartInfo = new ProcessStartInfo(sVigilantPath);
//            Host.VigilantProcess = p;
//            //Host.PWS = ProcessWindowStyle.Normal;// Default is ProcessWindowStyle.Normal
//            Host.StartVigilatProcess();

//            //p2 = new Process();
//            ////p.StartInfo = new ProcessStartInfo(@"notepad.exe");
//            //p2.StartInfo = new ProcessStartInfo(sVigilantPath);
//            //Host2.VigilantProcess = p2;
//            ////Host.PWS = ProcessWindowStyle.Normal;// Default is ProcessWindowStyle.Normal
//            //Host2.StartVigilatProcess();

//            //p3 = new Process();
//            ////p.StartInfo = new ProcessStartInfo(@"notepad.exe");
//            //p3.StartInfo = new ProcessStartInfo(sVigilantPath);
//            //Host3.VigilantProcess = p3;
//            //Host3.StartVigilatProcess();

//        }

//        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
//        {

//            foreach (var process in vigilantProcesses)
//            {
//                if (process != null && !process.HasExited)
//                    process.Kill();
//            }

//            if (p !=null && !p.HasExited)
//                p.Kill();
//            //if (!p2.HasExited)
//            //    p2.Kill();
//            //if (!p3.HasExited)
//            //    p3.Kill();
//        }
//    }
//}
