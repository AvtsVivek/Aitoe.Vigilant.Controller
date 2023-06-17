using Aitoe.Vigilant.Controller.BL.Entites;
using Aitoe.Vigilant.Controller.BL.RepositoryInterfaces;
using Aitoe.Vigilant.Controller.DL.Entities;
using Aitoe.Vigilant.Controller.DL.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;

namespace Aitoe.Vigilant.Controller.DL.Repositories
{
    public class CamProcRepository : ICamProcRepository
    {

        public List<IAitoeRedCell> GetAllAitoeRedCells()
        {
            return Cells;
        }

        public void CloseAllProcesses(string sScenario = null)
        {
            foreach (var cell in Cells)
            {
                cell.CloseProcessMainWindow(sScenario);
            }
        }

        public void CloseProcess(string ipAddress)
        {
            throw new NotImplementedException();
        }

        public void CloseProcess(int row, int column)
        {
            throw new NotImplementedException();
        }

        private List<IAitoeRedCell> Cells;

        public CamProcRepository()
        {
            Cells = new List<IAitoeRedCell>();
            Task checkProcStatusTask = new Task(() => CheckProcessStatus());
            checkProcStatusTask.Start();
        }
                
        private void CheckProcessStatus()
        {
            while (true)
            {
                Thread.Sleep(Settings.Default.PollingDuration);

                if (Cells == null)
                    continue;

                if (Cells.Count == 0)
                    continue;

                //var vigilantCellVMs = Cells.Where(c => c.GetType() == typeof(VigilantSingleProcessViewModel)).ToList();
                foreach (var cell in Cells)
                    cell.StartCrashedProcess();
            }
        }

        public void LoadProcInfoFromSettings()
        {
            if (string.IsNullOrEmpty(Settings.Default.GridCellsData))
                return;

            byte[] gridCellsDataBytes = Convert.FromBase64String(Settings.Default.GridCellsData);

            if (gridCellsDataBytes == null || gridCellsDataBytes.Length == 0)
                return;

            var ms = new MemoryStream(gridCellsDataBytes);

            if (ms == null)
                return;

            using (ms)
            {
                var bf = new BinaryFormatter();
                var cells = (List<IAitoeRedCell>)bf.Deserialize(ms);
                if (!AreCellsConsistent(cells))
                    Cells.Clear();
                else
                    Cells = cells;
                return;
            }
        }

        private bool AreCellsConsistent(List<IAitoeRedCell> cells)
        {
            int totalCellCount = cells.Count;

            if (totalCellCount == 0)
                return false;

            // Start with rows. Get the maximum row count.
            var maxRow = cells.Select(c => c.Row).Max();
            var maxColumn = cells.Select(c => c.Column).Max();

            var noOfCellsInEachRow = new List<int>();
            var noOfCellsInEachColumn = new List<int>();

            for (int i = 1; i <= maxRow; i++)
                noOfCellsInEachRow.Add(cells.Where(c => c.Row == i).Count());           

            if (noOfCellsInEachRow.Min() != noOfCellsInEachRow.Max())
                return false;

            for (int j = 1; j <= maxColumn; j++)
                noOfCellsInEachColumn.Add(cells.Where(c => c.Column == j).Count());

            if (noOfCellsInEachColumn.Min() != noOfCellsInEachColumn.Max())
                return false;

            if (noOfCellsInEachColumn.Min() * noOfCellsInEachRow.Min() != cells.Count)
                return false;

            return true;
        }

        public int GetRunningCameras()
        {
            int noOfRunningCameras = 0;
            foreach (var cell in Cells)
            {
                
            }
            return noOfRunningCameras;
        }

        public void PersistProcInfoToSettings()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Settings.Default.GridCellsData = string.Empty;
                Settings.Default.Save();

                var bf = new BinaryFormatter();

                if (!AreCellsConsistent(Cells))
                {
                    Settings.Default.GridCellsData = string.Empty;
                    Settings.Default.Save();
                }
                bf.Serialize(ms, Cells);
                ms.Position = 0;
                byte[] buffer = new byte[(int)ms.Length];
                ms.Read(buffer, 0, buffer.Length);
                Settings.Default.GridCellsData = Convert.ToBase64String(buffer);
                Settings.Default.Save();
            }
        }

        //public void StartAllProcesses()
        //{
        //    //StartAitoeRedProcessManually()
        //    Cells.ForEach(c => c.TryStartAitoeRedProcess());
        //}

        public void StopProcess(int row, int column)
        {
            throw new NotImplementedException();
        }

        public void AddAitoeRedCell(IAitoeRedCell cell)
        {
            var count = Cells.Where(c => c.Row == cell.Row && c.Column == cell.Column).Count();
            if (count == 0)
                Cells.Add(cell);
        }

        public void UpdateAitoeRedCell(IAitoeRedCell cell)
        {
            var targetCell = Cells.Where(c => c.Column == cell.Column && c.Row == cell.Row).FirstOrDefault();
            if (targetCell != null)
            {
                //http://stackoverflow.com/a/2376102/1977871
                throw new NotImplementedException();
            }
        }

        public IAitoeRedCell CreateAitoeRedCell()
        {
            var cell = new AitoeRedCell();
            Cells.Add(cell);
            return cell;
        }

        public void RemoveAitoeRedCell(IAitoeRedCell cell)
        {
            cell.CloseProcessMainWindow();
            Cells.Remove(cell);
        }
    }
}
