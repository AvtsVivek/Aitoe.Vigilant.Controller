using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aitoe.Vigilant.Controller.BL.Infra
{
    public enum AitoeRedProcessStatus
    {
        NotStarted,
        StoppedByUser,
        Crashed,


        Running,

        RunningSerialized
    }
}
