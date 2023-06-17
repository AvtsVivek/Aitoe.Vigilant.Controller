using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aitoe.Vigilant.Controller.BL.Entites
{
    public interface IEmail
    {
        List<string> ToAddressList { get; set; }
        string ToTitle { get; set; }
        string ToBody { get; set; }
        List<string> Attachments { get; set; }
    }
}
