using Aitoe.Vigilant.Controller.BL.Entites;
using Aitoe.Vigilant.Controller.BL.Infra;
using Aitoe.Vigilant.Controller.BL.Infra.Events;
using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Threading;

namespace Aitoe.Vigilant.Controller.DL.Entities
{
    [Serializable]
    public class AitoeRedCell : IAitoeRedCell
    {
        [field: NonSerialized]
        public event EventHandler<ReadOnlyEventArgs<AitoeRedProcessStatus>> ProcessChangeEvent;
        public string CellName
        {
            get;
            set;
        }

        public int Column
        {
            get;
            set;
        }

        public int Row
        {
            get;
            set;
        }

        public string IpAddress { get; set; }
        public string LoginId { get; set; }

        private string _password = string.Empty;
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                if (_password == value)
                    return;
                _password = value;
            }
        }

        private string sAitoeRedPath = string.Empty;


        [NonSerialized]
        private Process _aitoeRedProcess;

        public Process AitoeRedProcess
        {
            get { return _aitoeRedProcess; }
            private set { _aitoeRedProcess = value; }
        }

        public bool IsWebCam
        {
            get; set;
        }

        [OnSerializing]
        public void AitoeRedProcessOnSerializing(StreamingContext context)
        {
            if (RedProcStatus == AitoeRedProcessStatus.Running)
                RedProcStatus = AitoeRedProcessStatus.RunningSerialized;
        }

        public override string ToString()
        {
            var s = "Cell Name " + CellName + " Ip Address " + IpAddress + " Process Id" + AitoeRedProcess == null ? "No Process" : AitoeRedProcess.Id.ToString();
            return base.ToString();
        }

        [OnDeserializing]
        public void AitoeRedProcessOnDeserializing(StreamingContext context)
        {
            Initialize();
        }

        [OnDeserialized]
        public void AitoeRedProcessOnDeserialized(StreamingContext context)
        {
            //if (RedProcStatus == AitoeRedProcessStatus.RunningSerialized)
            //    StartAitoeRedProcess();
        }


        private void Initialize()
        {
            sAitoeRedPath = @"Vigilant.exe";
        }

        public AitoeRedCell()
        {
            Initialize();
        }

        public void CloseProcessMainWindow(string sScenario = null)
        {
            if (AitoeRedProcess != null)
                if (AitoeRedProcess.IsRunning() && !AitoeRedProcess.HasExited)
                    AitoeRedProcess.CloseMainWindow();

            if (!string.IsNullOrEmpty(sScenario))
                if (sScenario == "StopAllCameras")
                    RedProcStatus = AitoeRedProcessStatus.StoppedByUser;
        }

        private bool HasProcessStartedSuccessifully()
        {
            int count = 0;
            //http://stackoverflow.com/a/11747621/1977871
            //var procRunningStatus = (!VigilantProcess.WaitForInputIdle() || VigilantProcess.MainWindowHandle == IntPtr.Zero || !GetWin32WindowRectangle()) && count < 100;
            //var procRunningStatus = (!AitoeRedProcess.WaitForInputIdle() || AitoeRedProcess.MainWindowHandle == IntPtr.Zero) && count < 100;
            while ((!AitoeRedProcess.WaitForInputIdle() || AitoeRedProcess.MainWindowHandle == IntPtr.Zero) && count < 100)
            {
                count++;
                AitoeRedProcess.Refresh();
                Thread.Sleep(100);
            }

            if (count > 97)
                return false;
            else
                return true;
        }

        public void CheckForAitoeRedProcSerializedStatusAndTryStart()
        {
            if (RedProcStatus == AitoeRedProcessStatus.RunningSerialized)
                InitializeAitoeRedProcess();
        }

        public void TryStartAitoeRedProcess()
        {
            InitializeAitoeRedProcess();
        }

        private void InitializeAitoeRedProcess()
        {
            string sArgs = string.Empty;

            if (!IsWebCam)
            {
                if (string.IsNullOrEmpty(IpAddress))
                    return;

                if (string.IsNullOrEmpty(LoginId))
                    return;

                if (string.IsNullOrEmpty(Password))
                    return;

                sArgs = "GOP1N@TH108 " + IpAddress + " " + LoginId + " " + Password + " 0";
            }
            else
                sArgs = "GOP1N@TH108 " + "0.0.0.0:0" + " " + "NoLogin" + " " + "NoPassword" + " 0";

            VerifyProcessStatus();

            if (RedProcStatus == AitoeRedProcessStatus.Running)
                return;

            bool bStartedCorrectly = false;

            for (int i = 0; i < 5; i++)
            {
                AitoeRedProcess = new Process();
                AitoeRedProcess.EnableRaisingEvents = true;
                AitoeRedProcess.StartInfo = new ProcessStartInfo(sAitoeRedPath);
                AitoeRedProcess.StartInfo.Arguments = sArgs;

                if (StartAitoeRedProcess())
                {
                    bStartedCorrectly = true;
                    RedProcStatus = AitoeRedProcessStatus.Running;
                    break;
                }
                else
                    CloseProcessMainWindow();
            }

            if (!bStartedCorrectly)
                throw new Exception(string.Format("Process Could not start for column {0} and row {1} for IPAddress {2}", Column, Row, IpAddress));
            else
                AitoeRedProcess.Exited += Process_Exited;

            OnProcessChangeEvent();
            return;
        }

        private void OnProcessChangeEvent()
        {
            var procChangeEvent = ProcessChangeEvent;
            if (procChangeEvent != null)
            {
                var args = procChangeEvent.CreateArgs(RedProcStatus);
                procChangeEvent(this, args);
            }
        }

        private bool StartAitoeRedProcess()
        {
            try
            {
                AitoeRedProcess.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                AitoeRedProcess.Start();

                if (!HasProcessStartedSuccessifully())
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return false;
            }
        }
        private void Process_Exited(object sender, EventArgs e)
        {
            var exitCode = AitoeRedProcess.ExitCode;
            if (exitCode == 35)
                RedProcStatus = AitoeRedProcessStatus.StoppedByUser;
            
            else if (RedProcStatus != AitoeRedProcessStatus.StoppedByUser)
                RedProcStatus = AitoeRedProcessStatus.Crashed; // Its is likely that the process has crashed.

            OnProcessChangeEvent();
        }

        private AitoeRedProcessStatus RedProcStatus = AitoeRedProcessStatus.NotStarted;
        public AitoeRedProcessStatus GetAitoeRedProcStatus()
        {
            return RedProcStatus;
        }

        public void SetAitoeRedProcessStatusToStopped()
        {
            RedProcStatus = AitoeRedProcessStatus.StoppedByUser;
        }

        private void VerifyProcessStatus()
        {
            if (AitoeRedProcess != null && AitoeRedProcess.IsRunning() && !AitoeRedProcess.HasExited)
                RedProcStatus = AitoeRedProcessStatus.Running;
        }

        public void StartCrashedProcess()
        {
            if (RedProcStatus == AitoeRedProcessStatus.NotStarted)
                return; // Since the process has not yet started, simply return. Do nothing.

            if (RedProcStatus == AitoeRedProcessStatus.StoppedByUser)
                return; // Since the process was stopped, simply return. Do nothing.

                VerifyProcessStatus();
            if (RedProcStatus == AitoeRedProcessStatus.Running)
                return;

            InitializeAitoeRedProcess();
        }
    }
}