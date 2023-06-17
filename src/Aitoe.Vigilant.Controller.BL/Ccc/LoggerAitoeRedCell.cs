using Aitoe.Vigilant.Controller.BL.Entites;
using Aitoe.Vigilant.Controller.BL.Infra;
using Aitoe.Vigilant.Controller.BL.Infra.Events;
using log4net;
using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Aitoe.Vigilant.Controller.BL.Ccc
{
    [Serializable]
    public class LoggerAitoeRedCell : IAitoeRedCell
    {
        [OnDeserializing]
        public void LoggerAitoeRedProcessOnDeserializing(StreamingContext context)
        {

        }

        [OnDeserialized]
        public void LoggerAitoeRedProcessOnDeserialized(StreamingContext context)
        {
            _AitoeRedCell.ProcessChangeEvent += _AitoeRedCell_ProcessChangeEvent;

            // The following is a special case. For a deserialized object, there seems to be no way to 
            // inject logger through DI because, ctor is not called during serialization. 
            // Even if it is called, ctor injection will not happen, becuase this is not called by the DI contianer.
            // So better get this directly. 
            if (_Log == null)
                _Log = LogManager.GetLogger(this.GetType());
        }

        [NonSerialized]
        private ILog _log;

        public ILog _Log
        {
            get { return _log; }
            set { _log = value; }
        }

        private readonly IAitoeRedCell _AitoeRedCell;
        [field: NonSerialized]
        public event EventHandler<ReadOnlyEventArgs<AitoeRedProcessStatus>> ProcessChangeEvent;
        public LoggerAitoeRedCell(IAitoeRedCell aitoeRedCell, ILog log)
        {
            if (aitoeRedCell == null)
                throw new ArgumentException("AitoeRedCell injected is null");
            _AitoeRedCell = aitoeRedCell;

            if (log == null)
                throw new ArgumentNullException("Log is null");
            _Log = log;

            _AitoeRedCell.ProcessChangeEvent += _AitoeRedCell_ProcessChangeEvent;
            _Log.Info("Logger Aitoe Red Cell Ctro");
        }

        private void _AitoeRedCell_ProcessChangeEvent(object sender, ReadOnlyEventArgs<AitoeRedProcessStatus> e)
        {
            var procChangeEvent = ProcessChangeEvent;
            if (procChangeEvent != null)
            {
                procChangeEvent(this, e);
            }
        }

        public Process AitoeRedProcess
        {
            get
            {
                return _AitoeRedCell.AitoeRedProcess;
            }
        }

        public string CellName
        {
            get
            {
                return _AitoeRedCell.CellName;
            }

            set
            {
                _AitoeRedCell.CellName = value;
            }
        }

        public int Column
        {
            get
            {
                return _AitoeRedCell.Column;
            }

            set
            {
                _AitoeRedCell.Column = value;
            }
        }

        public string IpAddress
        {
            get
            {
                return _AitoeRedCell.IpAddress;
            }

            set
            {
                _AitoeRedCell.IpAddress = value;
            }
        }

        public string LoginId
        {
            get
            {
                return _AitoeRedCell.LoginId;
            }

            set
            {
                _AitoeRedCell.LoginId = value;
            }
        }

        public string Password
        {
            get
            {
                return _AitoeRedCell.Password;
            }

            set
            {
                _AitoeRedCell.Password = value;
            }
        }

        public int Row
        {
            get
            {
                return _AitoeRedCell.Row;
            }

            set
            {
                _AitoeRedCell.Row = value;
            }
        }

        public bool IsWebCam
        {
            get
            {
                return _AitoeRedCell.IsWebCam;
            }

            set
            {
                _AitoeRedCell.IsWebCam = value;
            }
        }

        public void CheckForAitoeRedProcSerializedStatusAndTryStart()
        {
            _Log.Info("CheckForAitoeRedProcSerializedStatusAndTryStart");
            _AitoeRedCell.CheckForAitoeRedProcSerializedStatusAndTryStart();
        }

        public void CloseProcessMainWindow(string sScenario = null)
        {
            _Log.Info("Kill Process " + sScenario);
            _AitoeRedCell.CloseProcessMainWindow(sScenario);
        }

        public AitoeRedProcessStatus GetAitoeRedProcStatus()
        {
            _Log.Info("GetAitoeRedProcStatus");
            return _AitoeRedCell.GetAitoeRedProcStatus();
        }

        //SetAitoeRedProcessStatusToNotStarted

        public void SetAitoeRedProcessStatusToStopped()
        {
            _Log.Info("SetAitoeRedProcessStatusToNotStarted");
            _AitoeRedCell.SetAitoeRedProcessStatusToStopped();
        }


        public void StartCrashedProcess()
        {
            _Log.Info("Start Crashed Process");
            _AitoeRedCell.StartCrashedProcess();
        }

        public void TryStartAitoeRedProcess()
        {
            _Log.Info("Starting Aitoe Red");
            _AitoeRedCell.TryStartAitoeRedProcess();
        }
    }
}
