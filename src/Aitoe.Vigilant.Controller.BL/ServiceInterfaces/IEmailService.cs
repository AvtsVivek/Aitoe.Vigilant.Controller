using Aitoe.Vigilant.Controller.BL.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aitoe.Vigilant.Controller.BL.ServiceInterfaces
{
    public interface IEmailService
    {
        bool? ResetSMTPHost();
        Task<bool> TrySendEmailAsync(IEmail email);
        bool TrySendEmail(IEmail email);
        bool? SetSMTPHost(ISMTPHost smtpHost);
        ISMTPHost GetSMTPHost();
        bool? IsSMTPHostSet();
        Exception GetError();
    }
}
