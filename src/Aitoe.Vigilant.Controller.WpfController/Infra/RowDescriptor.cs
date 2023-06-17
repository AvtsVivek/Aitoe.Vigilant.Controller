using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aitoe.Vigilant.Controller.WpfController.Infra
{
    public class RowDescriptor
    {
        public RowDescriptor()
        {

        }
        public RowDescriptor(int rowCount)
        {
            RowNumber = rowCount.ToString();
            //Stuff = new Dictionary<int, string>();
            Stuff = new Dictionary<int, CellDescriptor>();
        }
        public string RowHeaderText { get; set; }
        public string RowNumber { get; set; }

        //public Dictionary<int, string> Stuff{ get; set; }

        public Dictionary<int, CellDescriptor> Stuff { get; set; }
    }
}
