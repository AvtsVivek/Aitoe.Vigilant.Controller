using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aitoe.Vigilant.Controller.WpfController.Infra.Messages
{
    public sealed class FromVigilantGridVMToVigilantSingleProcessVM
    {
        public int CamerasLicensed { get; set; }
        public int CamerasRunning { get; set; }
        public FromVigilantGridVMToVigilantSingleProcessVM(int running, int licensed)
        {
            CamerasLicensed = licensed;
            CamerasRunning = running;
        }
    }
}
