using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aitoe.Vigilant.Controller.BL.Entites
{
    public interface IAuthDetails
    {
        string UserId { get; set; }
        string Password { get; set; }
        string OrderId { get; set; }

        DateTime LicenseCheckDate { get; set; }

    }
}
