using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Aitoe.Vigilant.Controller.WpfController.Infra
{
    public class ExtendedListView : ListView
    {
        static ExtendedListView()
        {
            ViewProperty.OverrideMetadata(typeof(ExtendedListView), new PropertyMetadata(new PropertyChangedCallback(OnViewPropertyChanged)));
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        #region DataMatrix Extension
        private static void OnViewPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UpdateGridView(d as ExtendedListView, (IDataMatrix)d.GetValue(MatrixSourceProperty));
        }

        public static readonly DependencyProperty MatrixSourceProperty =
            DependencyProperty.Register("MatrixSource",
                                                typeof(IDataMatrix), typeof(ExtendedListView),
                                                new FrameworkPropertyMetadata(null,
                                                                              new PropertyChangedCallback(
                                                                                  OnMatrixSourceChanged)));

        public IDataMatrix MatrixSource
        {
            get { return (IDataMatrix)GetValue(MatrixSourceProperty); }
            set { SetValue(MatrixSourceProperty, value); }
        }

        private static void OnMatrixSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var listView = d as ExtendedListView;
            var dataMatrix = e.NewValue as IDataMatrix;

            UpdateGridView(listView, dataMatrix);
        }

        public static readonly DependencyProperty CellTemplateSelectorProperty =
           DependencyProperty.Register("CellTemplateSelector",
                                               typeof(DataTemplateSelector), typeof(ExtendedListView),
                                               new FrameworkPropertyMetadata(null,
                                                                             new PropertyChangedCallback(
                                                                                 OnCellTemplateSelectorChanged)));

        public DataTemplateSelector CellTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(CellTemplateSelectorProperty); }
            set { SetValue(CellTemplateSelectorProperty, value); }
        }

        private static void OnCellTemplateSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var listView = d as ExtendedListView;
            if (listView != null)
            {
                UpdateGridView(listView, listView.MatrixSource);
            }
        }

        private static void UpdateGridView(ExtendedListView listView, IDataMatrix dataMatrix)
        {
            if (listView == null || listView.View == null || !(listView.View is GridView) || dataMatrix == null)
                return;

            listView.ItemsSource = dataMatrix;
            var gridView = listView.View as GridView;
            gridView.Columns.Clear();
            foreach (var col in dataMatrix.Columns)
            {
                var column = new GridViewColumn
                {
                    Header = col.Name,
                    HeaderTemplate = gridView.ColumnHeaderTemplate
                };
                if (listView.CellTemplateSelector != null)
                {
                    column.CellTemplateSelector = new DataMatrixCellTemplateSelectorWrapper(listView.CellTemplateSelector, col.Name);
                }
                else
                {
                    column.DisplayMemberBinding = new Binding(string.Format("[{0}]", col.Name));
                }
                gridView.Columns.Add(column);
            }
        }

        public class DataMatrixCellTemplateSelectorWrapper : DataTemplateSelector
        {
            private readonly DataTemplateSelector _ActualSelector;
            private readonly string _ColumnName;
            private Dictionary<string, object> _OriginalRow;

            public DataMatrixCellTemplateSelectorWrapper(DataTemplateSelector actualSelector, string columnName)
            {
                _ActualSelector = actualSelector;
                _ColumnName = columnName;
            }

            public override DataTemplate SelectTemplate(object item, DependencyObject container)
            {
                // The item is basically the Content of the ContentPresenter.
                // In the DataMatrix binding case that is the dictionary containing the cell objects.
                // In order to be able to select a template based on the actual cell object and also
                // be able to bind to that object within the template we need to set the DataContext
                // of the template to the actual cell object. However after the template is selected
                // the ContentPresenter will set the DataContext of the template to the presenters
                // content. 
                // So in order to achieve what we want, we remember the original DataContext and then
                // change the ContentPresenter content to the actual cell object.
                // Therefor we need to remember the orginal DataContext otherwise in subsequent calls
                // we would get the first cell object.

                // remember old data context
                if (item is Dictionary<string, object>)
                {
                    _OriginalRow = item as Dictionary<string, object>;
                }

                if (_OriginalRow == null)
                    return null;

                // get the actual cell object
                var obj = _OriginalRow[_ColumnName];

                // select the template based on the cell object
                var template = _ActualSelector.SelectTemplate(obj, container);

                // find the presenter and change the content to the cell object so that it will become
                // the data context of the template
                var presenter = Utils.GetFirstParentForChild<ContentPresenter>(container);
                if (presenter != null)
                {
                    presenter.Content = obj;
                }

                return template;
            }
        }
        #endregion
    }
}
