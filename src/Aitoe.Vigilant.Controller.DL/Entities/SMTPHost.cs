using Aitoe.Vigilant.Controller.BL.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aitoe.Vigilant.Controller.DL.Entities
{
    public class SMTPHost : ISMTPHost
    {
        public string SendersEmailAddress
        {
            get;
            set;
        }

        public string EmailSendersPassword
        {
            get;
            set;
        }

        public int? SMTPPort
        {
            get;
            set;
        }

        public string SMTPServer
        {
            get;
            set;
        }


    }
}
