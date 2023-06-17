using Aitoe.Vigilant.Controller.BL.Entites;
using Aitoe.Vigilant.Controller.BL.RepositoryInterfaces;
using log4net;
using System;
using System.Collections.Generic;

namespace Aitoe.Vigilant.Controller.BL.Ccc
{
    public class LoggerCamProcRepository : ICamProcRepository
    {
        private readonly ICamProcRepository _Repository;
        private readonly ILog _Log;
        public LoggerCamProcRepository(ICamProcRepository repository, ILog log)
        {
            if (repository == null)
                throw new ArgumentNullException("CamProcRepository is null");
            _Repository = repository;

            if (log == null)
                throw new ArgumentNullException("Log is null");
            _Log = log;

            _Log.Info("Cam Repo created.");
        }

        public void AddAitoeRedCell(IAitoeRedCell cell)
        {
            _Repository.AddAitoeRedCell(cell);
            _Log.Info("Aitoe Red Cell Added.");
        }

        public IAitoeRedCell CreateAitoeRedCell()
        {
            var redCell = _Repository.CreateAitoeRedCell();
            _Log.Info("Aitoe Red Cell Created.");
            return redCell;
        }

        public List<IAitoeRedCell> GetAllAitoeRedCells()
        {
            var list = _Repository.GetAllAitoeRedCells();
            _Log.Info("GetAllAitoeRedCells Called.");
            return list;
        }

        public void CloseAllProcesses(string sScenario = null)
        {
            _Repository.CloseAllProcesses(sScenario);
            _Log.Info("Kill All Process Called.");
        }

        public void CloseProcess(string ipAddress)
        {
            _Repository.CloseProcess(ipAddress);
            _Log.Info("Kill Process for IpAddress " + ipAddress);
        }

        public void CloseProcess(int row, int column)
        {
            _Repository.CloseProcess(row, column);
            _Log.Info("Kill Process for row and column " + row + " " + column);
        }

        public void LoadProcInfoFromSettings()
        {
            _Repository.LoadProcInfoFromSettings();
            _Log.Info("LoadProcInfoFromSettings ");
        }

        public void PersistProcInfoToSettings()
        {
            _Repository.PersistProcInfoToSettings();
            _Log.Info("PersistProcInfoToSettings ");
        }

        public void RemoveAitoeRedCell(IAitoeRedCell cell)
        {
            _Repository.RemoveAitoeRedCell(cell);
            _Log.Info("RemoveAitoeRedCell ");
        }

        //public void StartAllProcesses()
        //{
        //    _Repository.StartAllProcesses();
        //    _Log.Info("StartAllProcesses ");
        //}

        public void StopProcess(int row, int column)
        {
            _Repository.StopProcess(row, column);
            _Log.Info("StopProcess row column " + row + " " + column);
        }

        public void UpdateAitoeRedCell(IAitoeRedCell cell)
        {
            _Repository.UpdateAitoeRedCell(cell);
            _Log.Info("UpdateAitoeRedCell " + cell.ToString());
        }
    }
}
