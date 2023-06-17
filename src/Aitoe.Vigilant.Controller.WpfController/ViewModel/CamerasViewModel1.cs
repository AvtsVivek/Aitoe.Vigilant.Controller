//using Aitoe.Vigilant.Controller.WpfController.Infra;
//using Aitoe.Vigilant.Controller.WpfController.Model;
//using GalaSoft.MvvmLight;
//using GalaSoft.MvvmLight.Command;
//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Windows.Input;

//namespace Aitoe.Vigilant.Controller.WpfController.ViewModel
//{
//    public class CamerasViewModel1 : ViewModelBase
//    {
//        public ObservableCollection<RowDescriptor> Rows { get; private set; }

//        public ObservableCollection<ColumnDescriptor> Columns { get; private set; }

//        public RelayCommand<string> AddColumnCommand { get; private set; }

//        public RelayCommand<string> RemoveColumnCommand { get; private set; }

//        public RelayCommand<string> AddRowCommand { get; private set; }

//        public RelayCommand<string> RemoveRowCommand { get; private set; }

//        public RelayCommand<string> AddRowHeaderCommand { get; private set; }

//        public RelayCommand<string> JustCheckCommand { get; private set; }

//        private SortedDictionary<int, string> sColDictionary = new SortedDictionary<int, string>();

//        private string m_cellNo;

//        public string CellNo
//        {
//            get { return m_cellNo; }
//            set
//            {
//                m_cellNo = value;
//                RaisePropertyChanged(() => CellNo);
//            }
//        }


//        public CamerasViewModel1()
//        {
//            Rows = new ObservableCollection<RowDescriptor>();
//            Columns = new ObservableCollection<ColumnDescriptor>();
//            AddColumnCommand = new RelayCommand<string>(AddColumnExecute);
//            RemoveColumnCommand = new RelayCommand<string>(RemoveColumnExecute);
//            AddRowCommand = new RelayCommand<string>(AddRowExecute);
//            RemoveRowCommand = new RelayCommand<string>(RemoveRowExecute);
//            AddRowHeaderCommand = new RelayCommand<string>(AddRowHeaderExecute);
//            JustCheckCommand = new RelayCommand<string>(JustCheckCommandExecute);
//            InitializeColumnNameDictionaryStrings();
//            CellNo = "Endaro MahanuBhavulu";
//        }

//        private void JustCheckCommandExecute(string s)
//        {
//            CellNo = "Andariki Vandanamulu";   
//        }
//        private void AddRowHeaderExecute(string s)
//        {
//            if (Columns.Count == 0)
//            {
//                string headerText = "";
//                Columns.Add(new ColumnDescriptor { ColumnHeaderText = headerText, DisplayMember = "RowNumber" });
//            }
//        }

//        private void AddColumnExecute(string s)
//        {
//            string columnHeaderText = sColDictionary[Columns.Count() - 1];
//            foreach (var rowDesc in RowDescriptors)
//            {
//                AddRowDescriptionCell(Columns.Count(), columnHeaderText + rowDesc.RowNumber.ToString(), rowDesc);
//            }
//            Columns.Add(new ColumnDescriptor { ColumnHeaderText = columnHeaderText, DisplayMember = "Stuff[" + Columns.Count().ToString() + "]" });
//        }

//        private void RemoveColumnExecute(string s)
//        {
//            Columns.Remove(this.Columns.FirstOrDefault(d => d.DisplayMember == s));
//            //RaisePropertyChanged(() => this.Columns);
//        }

//        private List<RowDescriptor> RowDescriptors = new List<RowDescriptor>();

//        private void AddRowExecute(string s)
//        {
//            int rowC = Rows.Count + 1;
//            int colC = Columns.Count - 1;
//            var rowDesc = new RowDescriptor(rowC);
//            for (int c = 0; c < colC; c++)
//            {
//                string columnHeaderText = sColDictionary[c];
//                AddRowDescriptionCell(c + 1, columnHeaderText + rowC.ToString(), rowDesc);
//            }

//            RowDescriptors.Add(rowDesc);
//            Rows.Add(rowDesc);
//            //RaisePropertyChanged(() => this.Rows);
//        }

//        private void AddRowDescriptionCell(int i, string s, RowDescriptor rowDesc)
//        {
//            var cellDesc = new CellDescriptor(s);
//            //rowDesc.Stuff.Add(i, s);
//            rowDesc.Stuff.Add(i, cellDesc);
//        }

//        private void RemoveRowExecute(string s)
//        {
//            Rows.Remove(this.Rows.FirstOrDefault());
//            //RaisePropertyChanged(() => this.Rows);
//        }

//        private void InitializeColumnNameDictionaryStrings()
//        {
//            const string sA = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
//            var sL = sA.Split(',').ToList();
//            sL.Sort();
//            int i = 0;
//            sL.ForEach(c => sColDictionary.Add(i++, c));
//        }
//    }
//}
