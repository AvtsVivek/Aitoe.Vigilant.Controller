using GalaSoft.MvvmLight;

namespace Aitoe.Vigilant.Controller.WpfController.Infra
{
    public class CellDescBase : ViewModelBase
    {
        public CellDescBase()
        {

        }
        public CellDescBase(int row, int column, string cellName)
        {
            Row = row;
            Column = column;
            CellName = cellName;
        }

        private string _cellName;

        //[field:NonSerialized]
        //public event PropertyChangedEventHandler PropertyChanged;

        public string CellName
        {
            get
            {
                return _cellName;
            }

            set
            {
                if (_cellName == value)
                {
                    return;
                }
                _cellName = value;
                RaisePropertyChanged();
            }
        }
        public int Row { get; set; }
        public int Column { get; set; }

        //protected void RaisePropertyChanged([CallerMemberName] string caller = "")
        //{
        //    var temp = PropertyChanged;

        //    if (temp != null)
        //    {
        //        temp(this, new PropertyChangedEventArgs(caller));
        //    }
        //}

    }

    
    public class CornerHeaderCell : CellDescBase
    {
        public CornerHeaderCell(int row, int column, string cellName) : base(row, column, cellName)
        {

        }
    }
    
    public class RowHeaderCell : CellDescBase
    {
        public int CellHeight { get; set; }
        public RowHeaderCell(int row, int column, string cellName) : base(row, column, cellName)
        {

        }
    }
    
    public class ColumnHeaderCell : CellDescBase
    {
        public int CellWidth { get; set; }
        public ColumnHeaderCell(int row, int column, string cellName) : base(row, column, cellName)
        {

        }
    }
}
