using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aitoe.Vigilant.Controller.WpfController.Infra
{
    [Serializable]
    public class TwoDProcessMatrix
    {
        private int _column;

        public int Column
        {
            get { return _column; }
            private set { _column = value; }
        }

        private int _row;

        public int Row
        {
            get { return _row; }
            private set { _row = value; }
        }


        private bool _processStatus;

        public bool ProcessStatus
        {
            get { return _processStatus; }
            private set { _processStatus = value; }
        }

        private int _processArg;

        public int ProcessArg
        {
            get { return _processArg; }
            set { _processArg = value; }
        }

        public TwoDProcessMatrix()
        {
            _column = 1;
            _row = 1;
            _processStatus = false;
            _processArg = 0;
        }

        public TwoDProcessMatrix(int col, int row, bool pocStatus, int procArg)
        {
            _column = col;
            _row = row;
            _processStatus = ProcessStatus;
            _processArg = procArg;
        }

        public void UpdateProcessMatrix(bool bProcessStatus, int procArg)
        {
            _processStatus = bProcessStatus;
            _processArg = procArg;
        }
    }
}
