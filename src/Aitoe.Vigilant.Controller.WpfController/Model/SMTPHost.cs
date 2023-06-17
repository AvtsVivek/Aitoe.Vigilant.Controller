using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aitoe.Vigilant.Controller.WpfController.Model
{
    public class SMTPHost
    {
        string SMTPServer { get; set; }
        int SMTPPort { get; set; }
        string SendersEmailAddress { get; set; }
        string SendersPassword { get; set; }
    }
}
