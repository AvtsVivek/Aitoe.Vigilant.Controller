using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aitoe.Vigilant.Controller.WpfController.Infra.Messages
{
    public sealed class FromMultiControllerVMToVigilantGridVM
    {
        public int? CellHeight { get; }
        public int? CellWidth { get; }
        public int? Rows { get; }
        public int? Columns { get; }
        public bool? IsGridHeaderOn { get; }
        public bool? IsGridOn { get; }

        public FromMultiControllerVMToVigilantGridVM()
        {

        }

        public FromMultiControllerVMToVigilantGridVM(int? rows)
        {
            Rows = rows;
        }

        public FromMultiControllerVMToVigilantGridVM(int? rows, int? columns)
        {
            Columns = columns;
            Rows = rows;
        }

        public FromMultiControllerVMToVigilantGridVM(int? rows, int? columns, int? cellHeight)
        {
            CellHeight = cellHeight;
            Columns = columns;
            Rows = rows;
        }

        public FromMultiControllerVMToVigilantGridVM(int? rows, int? columns, int? cellHeight, int? cellWidth)
        {
            CellHeight = cellHeight;
            CellWidth = cellWidth;
            Columns = columns;
            Rows = rows;
        }

        public FromMultiControllerVMToVigilantGridVM(int? rows, int? columns, int? cellHeight, int? cellWidth, bool? isGridOn)
        {
            Rows = rows;
            Columns = columns;
            CellHeight = cellHeight;
            CellWidth = cellWidth;
            IsGridOn = isGridOn;
        }

        public FromMultiControllerVMToVigilantGridVM(int? rows, int? columns, int? cellHeight, int? cellWidth, bool? isGridOn, bool? isGridHeaderOn)
        {
            Rows = rows;
            Columns = columns;
            CellHeight = cellHeight;
            CellWidth = cellWidth;
            IsGridOn = isGridOn;
            IsGridHeaderOn = isGridHeaderOn;
        }
    }
}
