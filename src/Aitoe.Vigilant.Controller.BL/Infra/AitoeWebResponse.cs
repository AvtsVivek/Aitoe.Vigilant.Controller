using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aitoe.Vigilant.Controller.BL.Infra
{
    public class AitoeWebResponseLoginStatus
    {
        public int Status { get; set; }
    }
    public class AitoeWebResponseLicensesForCustomer
    {
        public int id { get; set; }
        public string orderid {get; set; }
        public string customeremailid { get; set; }
        public int numberofcameras { get; set; }
        public int licenseActivated { get; set; }
        public DateTime purchaseDate { get; set; }

    }
    public class AitoeWebResponseNumberOfCamsPerLicense
    {
        public int numberofcameras { get; set; }
    }

    public class AitoeWebResponseActivatedFlag
    {
        public int status { get; set; }
    }
    public class AitoeWebResponseSetUnActivatedFlag
    {
        public int status { get; set; }
    }

    public class AitoeWebResponseSetActivatedFlag
    {
        public int status { get; set; }
    }
}
