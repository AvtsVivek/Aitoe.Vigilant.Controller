using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Aitoe.Vigilant.Controller.BL.Entites
{
    public interface ICellDescBase
    {
        int Row { get; set; }
        int Column { get; set; }
        string CellName { get; set; }
    }
}
