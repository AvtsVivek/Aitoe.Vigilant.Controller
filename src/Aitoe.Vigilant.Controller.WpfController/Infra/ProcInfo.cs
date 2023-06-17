using System;

namespace Aitoe.Vigilant.Controller.WpfController.Infra
{
    
    public class ProcInfo
    {
        internal string IpAddress { get; }
        internal int ProcId { get; }
        internal IntPtr ProcMainWindowHandle { get; }
        internal int Column { get; }
        internal int Row { get; }
        public ProcInfo(string ipAdd, int procId, IntPtr procMainWindowHandle, int rowNo, int colNo)
        {
            IpAddress = ipAdd;
            ProcId = procId;
            ProcMainWindowHandle = procMainWindowHandle;
            Row = rowNo;
            Column = colNo;
        }

        public override string ToString()
        {
            string procInfoString = "";
            procInfoString = "Ip Add: " + IpAddress + Environment.NewLine;
            procInfoString = procInfoString + "ProcId: " + ProcId.ToString() + Environment.NewLine;
            procInfoString = procInfoString + "ProcMainWindowHandle: " + ProcMainWindowHandle.ToString() + Environment.NewLine;
            procInfoString = procInfoString + "Row: " + Row.ToString() + Environment.NewLine;
            procInfoString = procInfoString + "Column: " + Column.ToString();
            return procInfoString;
        }

    }
}
