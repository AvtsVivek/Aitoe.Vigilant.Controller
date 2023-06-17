using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aitoe.Vigilant.Controller.WpfController.Infra.Messages
{
    public sealed class FromVigilantSingleProcessVMToVigilantGridVM
    {
        public bool IsWebCam { get; set; }
        public string IpAddress { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public string CellName { get; set; }
        public FromVigilantSingleProcessVMToVigilantGridVM(int row, int column, string cellName, bool isWebCam, string ipAddress)
        {
            Row = row;
            Column = column;
            CellName = cellName;
            IsWebCam = isWebCam;
            IpAddress = ipAddress;
        }
    }
}