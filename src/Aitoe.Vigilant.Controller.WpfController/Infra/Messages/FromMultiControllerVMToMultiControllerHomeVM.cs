using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aitoe.Vigilant.Controller.WpfController.Infra.Messages
{
    public sealed class FromMultiControllerVMToMultiControllerHomeVM
    {
        public string ShowView { get; private set; }
        public FromMultiControllerVMToMultiControllerHomeVM(string showView)
        {
            ShowView = showView;
        }
    }
}
