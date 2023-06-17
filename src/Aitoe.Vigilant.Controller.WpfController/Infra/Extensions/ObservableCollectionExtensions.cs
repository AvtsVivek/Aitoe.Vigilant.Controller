using Aitoe.Vigilant.Controller.BL.RepositoryInterfaces;
using Aitoe.Vigilant.Controller.WpfController.ViewModel;
using AutoMapper;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Aitoe.Vigilant.Controller.WpfController.Infra.Extensions
{
    public static class ObservableCollectionExtensions
    {
        private static SortedDictionary<int, string> ColNameDictionary = new SortedDictionary<int, string>();

        static ObservableCollectionExtensions()
        {
            InitializeColumnNameDictionaryStrings();
        }

        private static void InitializeColumnNameDictionaryStrings()
        {
            const string sA = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            var sL = sA.Split(',').ToList();
            sL.Sort();
            int i = 0;
            sL.ForEach(c => ColNameDictionary.Add(i++, c));
        }

        public static int GetRows(this ObservableCollection<CellDescBase> cells)
        {
            var rows = cells.Where(c => c.GetType() == typeof(RowHeaderCell)).ToList();
            return rows.Count + 1;
        }

        public static int GetColumns(this ObservableCollection<CellDescBase> cells)
        {
            var rows = cells.Where(c => c.GetType() == typeof(ColumnHeaderCell)).ToList();
            return rows.Count + 1;
        }

        public static void AddCell(this ObservableCollection<CellDescBase> cells, CellDescBase cell)
        {
            var count = cells.Where(c => c.GetType() == cell.GetType() && c.Row == cell.Row && c.Column == cell.Column).Count();
            if (count == 0)
                cells.Add(cell);
        }
        public static void AddCell(this ObservableCollection<CellDescBase> cells, int row, int column)
        {
            var count = cells.Where(c => c.Row == row && c.Column == column).Count();
            if (count == 0)
            {
                CellDescBase cell = null;
                if (row == 0 && column == 0)
                    cell = new CornerHeaderCell(0, 0, " ");
                else if (row != 0 && column == 0)
                    cell = new RowHeaderCell(row, 0, row.ToString());
                else if (row == 0 && column != 0)
                    cell = new ColumnHeaderCell(0, column, ColNameDictionary[column - 1]);
                else
                    cell = new VigilantSingleProcessViewModel(row, column, row.ToString() + " " + ColNameDictionary[column - 1]);
                cells.Add(cell);
            }
        }

        public static void RemoveCell(this ObservableCollection<CellDescBase> cells, VigilantSingleProcessViewModel cell, ICamProcRepository camProcRepo)
        {
            cells.Remove(cell);
            camProcRepo.RemoveAitoeRedCell(cell.AitoeRedCellModel);
        }


        public static CellDescBase AddCellWithAitoeRed(this ObservableCollection<CellDescBase> cells, int row, int column, ICamProcRepository camProcRepo, IMapper mapper)
        {
            var count = cells.Where(c => c.Row == row && c.Column == column).Count();
            if (count == 0)
            {
                CellDescBase cell = null;
                if (row == 0 && column == 0)
                    cell = new CornerHeaderCell(0, 0, " ");
                else if (row != 0 && column == 0)
                    cell = new RowHeaderCell(row, 0, row.ToString());
                else if (row == 0 && column != 0)
                    cell = new ColumnHeaderCell(0, column, ColNameDictionary[column - 1]);
                else
                {
                    var vigilantCell = new VigilantSingleProcessViewModel(row, column, row.ToString() + " " + ColNameDictionary[column - 1], mapper);
                    cell = vigilantCell;
                    camProcRepo.AddAitoeRedCell(vigilantCell.AitoeRedCellModel);
                }
                cells.Add(cell);
                return cell;
            }
            else
                return null;
        }

        public static void SetColumnHeaderText(this ObservableCollection<CellDescBase> cells)
        {
            var columnHeaderCellVMs = cells.Where(c => c.GetType() == typeof(ColumnHeaderCell)).ToList();

            foreach (var columnHeaderCell in columnHeaderCellVMs)
            {
                var colHC = (ColumnHeaderCell)columnHeaderCell;
                colHC.CellName = ColNameDictionary[colHC.Column - 1].ToString();
            }
        }

        public static void SetRowHeaderText(this ObservableCollection<CellDescBase> cells)
        {
            var rowHeaderCellVMs = cells.Where(c => c.GetType() == typeof(RowHeaderCell)).ToList();

            foreach (var rowHeaderCell in rowHeaderCellVMs)
            {
                var rowHC = (RowHeaderCell)rowHeaderCell;
                rowHC.CellName = rowHC.Row.ToString();
            }
        }
    }
}