using System.Collections;
using System.Collections.Generic;

namespace Aitoe.Vigilant.Controller.WpfController.Infra
{
    public interface IDataMatrix : IEnumerable
    {
        List<MatrixColumn> Columns { get; set; }
    }

    public class DataMatrix : IDataMatrix
    {
        public List<MatrixColumn> Columns { get; set; }
        public Dictionary<string, Dictionary<string, object>> Rows { get; set; }

        public DataMatrix()
        {
            Columns = new List<MatrixColumn>();
            Rows = new Dictionary<string, Dictionary<string, object>>();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Rows.Values.GetEnumerator();
        }
    }

    public class MatrixColumn
    {
        public string Name { get; set; }
    }

}
