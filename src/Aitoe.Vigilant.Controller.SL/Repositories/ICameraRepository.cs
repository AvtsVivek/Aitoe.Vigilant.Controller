using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aitoe.Vigilant.Controller.SL.Repositories
{
    public interface ICameraRepository
    {
        IQueryable<string> GetAllIpCameras();
    }
}
