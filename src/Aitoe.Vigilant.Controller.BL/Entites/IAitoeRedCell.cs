using Aitoe.Vigilant.Controller.BL.Infra;
using Aitoe.Vigilant.Controller.BL.Infra.Events;
using System;
using System.Diagnostics;

namespace Aitoe.Vigilant.Controller.BL.Entites
{
    public interface IAitoeRedCell : ICellDescBase
    {
        event EventHandler<ReadOnlyEventArgs<AitoeRedProcessStatus>> ProcessChangeEvent;
        Process AitoeRedProcess { get;}
        string IpAddress { get; set; }
        string LoginId { get; set; }
        string Password { get; set; }
        bool IsWebCam { get; set; }
        void TryStartAitoeRedProcess();
        void CheckForAitoeRedProcSerializedStatusAndTryStart();
        void CloseProcessMainWindow(string sScenario = null);
        void StartCrashedProcess();
        AitoeRedProcessStatus GetAitoeRedProcStatus();
        void SetAitoeRedProcessStatusToStopped();
    }
}