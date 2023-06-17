using Aitoe.Vigilant.Controller.BL.Entites;
using Aitoe.Vigilant.Controller.BL.Infra;
using Aitoe.Vigilant.Controller.BL.RepositoryInterfaces;
using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Aitoe.Vigilant.Controller.BL.Ccc
{
    public class ExceptionHandlerCamProcRepository : ICamProcRepository
    {
        private ICamProcRepository _CamRepository;
        private Exception CamRepoException = null;
        private ILog _Log;
        public ExceptionHandlerCamProcRepository(ICamProcRepository camRepo, ILog log)
        {
            if (camRepo == null)
                throw new ArgumentNullException("Auth Service is null");
            _CamRepository = camRepo;

            if (log == null)
                throw new ArgumentNullException("Log is null");
            _Log = log;
        }
        public void AddAitoeRedCell(IAitoeRedCell cell)
        {
            try
            {
                _CamRepository.AddAitoeRedCell(cell);
            }
            catch (Exception ex)
            {
                _Log.Error(MethodBase.GetCurrentMethod().Name + " " + ex.GetaAllMessages());
                CamRepoException = ex.GetSummaryAitoeBaseException();
                return;
            }
        }

        public void CloseAllProcesses(string sScenario = null)
        {
            try
            {
                _CamRepository.CloseAllProcesses(sScenario);
            }
            catch (Exception ex)
            {
                _Log.Error(MethodBase.GetCurrentMethod().Name + " " + ex.GetaAllMessages());
                CamRepoException = ex.GetSummaryAitoeBaseException();
                return ;
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

        public IAitoeRedCell CreateAitoeRedCell()
        {
            try
            {
                return _CamRepository.CreateAitoeRedCell();
            }
            catch (Exception ex)
            {
                _Log.Error(MethodBase.GetCurrentMethod().Name + " " + ex.GetaAllMessages());
                CamRepoException = ex.GetSummaryAitoeBaseException();
                return null;
            }
        }

        public List<IAitoeRedCell> GetAllAitoeRedCells()
        {
            try
            {
                return _CamRepository.GetAllAitoeRedCells();
            }
            catch (Exception ex)
            {
                _Log.Error(MethodBase.GetCurrentMethod().Name + " " + ex.GetaAllMessages());
                CamRepoException = ex.GetSummaryAitoeBaseException();
                return null;
            }
        }

        public void LoadProcInfoFromSettings()
        {
            try
            {
                _CamRepository.LoadProcInfoFromSettings();
            }
            catch (Exception ex)
            {
                _Log.Error(MethodBase.GetCurrentMethod().Name + " " + ex.GetaAllMessages());
                CamRepoException = ex.GetSummaryAitoeBaseException();
                return;
            }
        }

        public void PersistProcInfoToSettings()
        {
            try
            {
                _CamRepository.PersistProcInfoToSettings();
            }
            catch (Exception ex)
            {
                _Log.Error(MethodBase.GetCurrentMethod().Name + " " + ex.GetaAllMessages());
                CamRepoException = ex.GetSummaryAitoeBaseException();
                return;
            }
        }

        public void RemoveAitoeRedCell(IAitoeRedCell cell)
        {
            try
            {
                _CamRepository.RemoveAitoeRedCell(cell);
            }
            catch (Exception ex)
            {
                _Log.Error(MethodBase.GetCurrentMethod().Name + " " + ex.GetaAllMessages());
                CamRepoException = ex.GetSummaryAitoeBaseException();
                return;
            }
        }

        //public void StartAllProcesses()
        //{
        //    try
        //    {
        //        _CamRepository.StartAllProcesses();
        //    }
        //    catch (Exception ex)
        //    {
        //        _Log.Error(MethodBase.GetCurrentMethod().Name + " " + ex.GetaAllMessages());
        //        CamRepoException = ex.GetSummaryAitoeBaseException();
        //        return;
        //    }
        //}

        public void StopProcess(int row, int column)
        {
            throw new NotImplementedException();
        }

        public void UpdateAitoeRedCell(IAitoeRedCell cell)
        {
            try
            {
                _CamRepository.UpdateAitoeRedCell(cell);
            }
            catch (Exception ex)
            {
                _Log.Error(MethodBase.GetCurrentMethod().Name + " " + ex.GetaAllMessages());
                CamRepoException = ex.GetSummaryAitoeBaseException();
                return;
            }
        }
    }
}
