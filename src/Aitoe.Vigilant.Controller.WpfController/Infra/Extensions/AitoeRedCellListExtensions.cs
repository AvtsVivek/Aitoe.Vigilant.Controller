using Aitoe.Vigilant.Controller.BL.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aitoe.Vigilant.Controller.WpfController.Infra.Extensions
{
    public static class AitoeRedCellListExtensions
    {
        public static int GetRows(this List<IAitoeRedCell> cells)
        {
            var rowsHeaderCells = cells.Where(c => c.Column == 1).ToList();
            return rowsHeaderCells.Count;
        }
        public static int GetColumns(this List<IAitoeRedCell> cells)
        {
            var columnsHeaderCells = cells.Where(c => c.Row == 1).ToList();
            return columnsHeaderCells.Count;
        }
    }
}
