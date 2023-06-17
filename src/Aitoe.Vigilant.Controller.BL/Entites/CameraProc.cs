//using Aitoe.Vigilant.Controller.BL.Infra;
//using Aitoe.Vigilant.Controller.BL.Infra.Events;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Runtime.Serialization;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace Aitoe.Vigilant.Controller.BL.Entites
//{
//    // Encapsulates a camera process.
//    public class CameraProcess : ICellDescBase
//    {
//        public string CellName
//        {
//            get;
//            set;
//        }

//        public int Column
//        {
//            get;
//            set;
//        }

//        public int Row
//        {
//            get;
//            set;
//        }

//        private AitoeRedProcessStatus RedProcStatus = AitoeRedProcessStatus.NotStarted;

//        public string IpAddress { get; set; }
//        public string LoginId { get; set; }

//        private string _password = string.Empty;
//        public string Password
//        {
//            get
//            {
//                return _password;
//            }
//            set
//            {
//                if (_password == value)
//                    return;
//                _password = value;
//            }
//        }

//        private string sAitoeRedPath = string.Empty;

//        [OnSerializing]
//        public void AitoeRedSingleProcessOnSerializing(StreamingContext context)
//        {
//            if (RedProcStatus == AitoeRedProcessStatus.Running)
//                RedProcStatus = AitoeRedProcessStatus.RunningSerialized;
//        }

//        [OnDeserializing]
//        public void AitoeRedSingleProcessOnDeserializing(StreamingContext context)
//        {
//            InitializeProcess();
//        }

//        [OnDeserialized]
//        public void AitoeRedSingleProcessOnDeserialized(StreamingContext context)
//        {
//            if (RedProcStatus == AitoeRedProcessStatus.RunningSerialized)
//                InitiateAitoeRedProcess();
//        }

//        [NonSerialized]
//        private Process AitoeRedProcess;

//        public CameraProcess()
//        {
//            InitializeProcess();
//        }

//        private void InitializeProcess()
//        {
//            sAitoeRedPath = @"Vigilant.exe";
//        }

//        public void InitiateAitoeRedProcess()
//        {
//            if (string.IsNullOrEmpty(IpAddress))
//                return;

//            if (string.IsNullOrEmpty(LoginId))
//                return;

//            if (string.IsNullOrEmpty(Password))
//                return;

//            VerifyProcessStatus();

//            if (RedProcStatus == AitoeRedProcessStatus.Running)
//                return;

//            bool bStartedCorrectly = false;

//            for (int i = 0; i < 5; i++)
//            {
//                AitoeRedProcess = new Process();
//                AitoeRedProcess.EnableRaisingEvents = true;
//                AitoeRedProcess.StartInfo = new ProcessStartInfo(sAitoeRedPath);
//                AitoeRedProcess.StartInfo.Arguments = "GOP1N@TH108 " + IpAddress + " " + LoginId + " " + Password + " 0";

//                if (StartAitoeRedProcess())
//                {
//                    bStartedCorrectly = true;
//                    // The following is to be done by the view model and not the model.
//                    //var vProcInfo = new ProcInfo(IpAddress, VigilantProcess.Id, VigilantProcess.MainWindowHandle, Row, Column);
//                    //VigilantProcInfo = vProcInfo;
//                    //VigilantVisibility = Visibility.Visible;
//                    //FormVisibility = Visibility.Collapsed;
//                    RedProcStatus = AitoeRedProcessStatus.Running;
//                    break;
//                }
//                else
//                    KillProcess();
//            }

//            if (!bStartedCorrectly)
//            {
//                throw new Exception(string.Format("Process Could not start for column {0} and row {1} for IPAddress {2}", Column, Row, IpAddress));
//            }
//            else
//            {
//                AitoeRedProcess.Exited += Process_Exited;
//            }
//        }

//        private void Process_Exited(object sender, EventArgs e)
//        {
//            var exitCode = AitoeRedProcess.ExitCode;

//            if (exitCode == 35)
//            {
//                //FormVisibility = Visibility.Visible;
//                //VigilantVisibility = Visibility.Collapsed;
//                RedProcStatus = AitoeRedProcessStatus.StoppedByUser;
//                //VigilantProcInfo = null;
//            }
//            else if (RedProcStatus != AitoeRedProcessStatus.StoppedByUser)
//                RedProcStatus = AitoeRedProcessStatus.Crashed; // Its is likely that the process has crashed.

//        }

//        public bool StartAitoeRedProcess()
//        {
//            try
//            {
//                AitoeRedProcess.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
//                AitoeRedProcess.Start();

//                if (!HasProcessStartedSuccessifully())
//                    return false;

//                return true;
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.Message.ToString());
//                return false;
//            }
//        }
//        private bool HasProcessStartedSuccessifully()
//        {
//            int count = 0;
//            //http://stackoverflow.com/a/11747621/1977871
//            //var procRunningStatus = (!VigilantProcess.WaitForInputIdle() || VigilantProcess.MainWindowHandle == IntPtr.Zero || !GetWin32WindowRectangle()) && count < 100;
//            var procRunningStatus = (!AitoeRedProcess.WaitForInputIdle() || AitoeRedProcess.MainWindowHandle == IntPtr.Zero) && count < 100;
//            while (procRunningStatus)
//            {
//                count++;
//                AitoeRedProcess.Refresh();
//                Thread.Sleep(100);
//            }

//            if (count > 98)
//                return false;
//            else
//                return true;
//        }

//        private void VerifyProcessStatus()
//        {
//            if (AitoeRedProcess != null && AitoeRedProcess.IsRunning() && !AitoeRedProcess.HasExited)
//                RedProcStatus = AitoeRedProcessStatus.Running;
//        }

//        private void KillProcess()
//        {
//            if (AitoeRedProcess != null)
//                if (AitoeRedProcess.IsRunning() && !AitoeRedProcess.HasExited)
//                    AitoeRedProcess.Kill();
//        }
//    }
//}
