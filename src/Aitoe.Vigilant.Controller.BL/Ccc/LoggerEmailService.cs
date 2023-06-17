using Aitoe.Vigilant.Controller.BL.Entites;
using Aitoe.Vigilant.Controller.BL.ServiceInterfaces;
using log4net;
using System;
using System.Threading.Tasks;

namespace Aitoe.Vigilant.Controller.BL.Ccc
{
    public class LoggerEmailService : IEmailService
    {
        private readonly IEmailService _EmailService;
        private readonly ILog _Log;
        public LoggerEmailService(IEmailService emailService, ILog log)
        {
            if (emailService == null)
                throw new ArgumentNullException("Email Service is null");
            _EmailService = emailService;

            if (log == null)
                throw new ArgumentNullException("Log is null");
            _Log = log;

            _Log.Info("Email Service created.");
        }

        public ISMTPHost GetSMTPHost()
        {
            _Log.Info("GetSMTPHost");
            return _EmailService.GetSMTPHost();
        }

        public bool? IsSMTPHostSet()
        {
            _Log.Info("GetSMTPHost");
            return _EmailService.IsSMTPHostSet();
        }

        public bool? ResetSMTPHost()
        {
            _Log.Info("GetSMTPHost");
            return _EmailService.ResetSMTPHost();
        }

        public async Task<bool> TrySendEmailAsync(IEmail email)
        {
            _Log.Info("GetSMTPHost");
            return await _EmailService.TrySendEmailAsync(email);
        }

        public bool? SetSMTPHost(ISMTPHost smtpHost)
        {
            _Log.Info("GetSMTPHost");
            return _EmailService.SetSMTPHost(smtpHost);
        }

        public Exception GetError()
        {
            return _EmailService.GetError();
        }

        public bool TrySendEmail(IEmail email)
        {
            _Log.Info("TrySendEmail");
            return _EmailService.TrySendEmail(email);
        }
    }
}
