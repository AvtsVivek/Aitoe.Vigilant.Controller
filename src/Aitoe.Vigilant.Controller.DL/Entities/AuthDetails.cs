using Aitoe.Vigilant.Controller.BL.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aitoe.Vigilant.Controller.DL.Entities
{
    [Serializable]
    public class AuthDetails : IAuthDetails
    {
        public string OrderId
        {
            get;

            set;
        }

        public string Password
        {
            get;

            set;
        }

        public string UserId
        {
            get;

            set;
        }

        public DateTime LicenseCheckDate
        {
            get;

            set;
        }
    }
}
