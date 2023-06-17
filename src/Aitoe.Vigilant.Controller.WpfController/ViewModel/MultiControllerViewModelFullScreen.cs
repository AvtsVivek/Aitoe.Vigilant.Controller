using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aitoe.Vigilant.Controller.WpfController.ViewModel
{
    public class MultiControllerViewModelFullScreen : IPageViewModel
    {
        // This is just a hack ViewModel to get a consitint look for the FullScreen Menu.
        public string Name
        {
            get
            {
                return "Full _Screen";
            }
        }
    }
}
