using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aitoe.Vigilant.Controller.BL.Entites
{
    public interface ISMTPHost
    {
        string SMTPServer { get; set; }
        int? SMTPPort { get; set; }
        string SendersEmailAddress { get; set; }
        string EmailSendersPassword { get; set; }

    }
}
