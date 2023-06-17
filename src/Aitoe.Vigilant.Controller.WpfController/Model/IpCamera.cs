using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aitoe.Vigilant.Controller.WpfController.Model
{
    public class IpCamera
    {
        public IpCamera()
        {

        }
        public string IpAddress { get; set; }
        //public string FirstName { get; set; }
        //public string FatherName { get; set; }
        //public DateTime DateOfBirth { get; set; }

        public override string ToString()
        {
            //return base.ToString();
            return IpAddress;
        }
    }
}
