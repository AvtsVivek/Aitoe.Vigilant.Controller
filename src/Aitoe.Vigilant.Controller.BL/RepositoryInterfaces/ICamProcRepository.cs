using Aitoe.Vigilant.Controller.BL.Entites;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aitoe.Vigilant.Controller.BL.RepositoryInterfaces
{
    public interface ICamProcRepository
    {
        List<IAitoeRedCell> GetAllAitoeRedCells();
        IAitoeRedCell CreateAitoeRedCell();
        void UpdateAitoeRedCell(IAitoeRedCell cell);
        void AddAitoeRedCell(IAitoeRedCell cell);
        void RemoveAitoeRedCell(IAitoeRedCell cell);
        //void StartAllProcesses();
        void StopProcess(int row, int column);
        void CloseAllProcesses(string sScenario = null);
        void CloseProcess(string ipAddress);
        void CloseProcess(int row, int column);
        void LoadProcInfoFromSettings();
        void PersistProcInfoToSettings();
    }
}
