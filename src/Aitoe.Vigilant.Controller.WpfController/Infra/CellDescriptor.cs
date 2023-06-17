using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aitoe.Vigilant.Controller.WpfController.Infra
{
    public class CellDescriptor
    {
        public CellDescriptor(string Name)
        {
            CellName = Name;
        }

        public string CellName { get; set; }

        public override string ToString()
        {
            return CellName;
        }
    }
}
