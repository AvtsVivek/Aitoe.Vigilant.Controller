using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aitoe.Vigilant.Controller.WpfController.Infra.Messages
{
    public sealed class ToMultiControllerHomeVM
    {
        public string ShowView { get; private set; }
        public ToMultiControllerHomeVM(string showView)
        {
            ShowView = showView;
        }
    }
}
