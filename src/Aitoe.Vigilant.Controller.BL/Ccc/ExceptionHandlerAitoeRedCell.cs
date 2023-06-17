using Aitoe.Vigilant.Controller.BL.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aitoe.Vigilant.Controller.BL.Infra;
using Aitoe.Vigilant.Controller.BL.Infra.Events;
using System.Diagnostics;

namespace Aitoe.Vigilant.Controller.BL.Ccc
{
    public class ExceptionHandlerAitoeRedCell : IAitoeRedCell
    {
        public Process AitoeRedProcess
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string CellName
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public int Column
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string IpAddress
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool IsWebCam
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string LoginId
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string Password
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public int Row
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public event EventHandler<ReadOnlyEventArgs<AitoeRedProcessStatus>> ProcessChangeEvent;

        public void CheckForAitoeRedProcSerializedStatusAndTryStart()
        {
            throw new NotImplementedException();
        }

        public void CloseProcessMainWindow(string sScenario = null)
        {
            throw new NotImplementedException();
        }

        public AitoeRedProcessStatus GetAitoeRedProcStatus()
        {
            throw new NotImplementedException();
        }

        public void SetAitoeRedProcessStatusToStopped()
        {
            throw new NotImplementedException();
        }


        public void StartCrashedProcess()
        {
            throw new NotImplementedException();
        }

        public void TryStartAitoeRedProcess()
        {
            throw new NotImplementedException();
        }
    }
}
