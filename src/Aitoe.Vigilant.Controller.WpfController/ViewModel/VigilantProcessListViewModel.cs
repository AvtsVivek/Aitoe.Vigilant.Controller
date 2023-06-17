using Aitoe.Vigilant.Controller.WpfController.Infra;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aitoe.Vigilant.Controller.WpfController.ViewModel
{
    public class VigilantProcessListViewModel : ViewModelBase, IPageViewModel
    {
        public string BindingKey
        {
            get
            {
                return "P";
            }
        }
        public string Name
        {
            get
            {
                return "Camera _Process List";
            }
        }
    }
}
