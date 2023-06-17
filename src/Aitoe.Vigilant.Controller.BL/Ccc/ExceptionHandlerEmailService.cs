using Aitoe.Vigilant.Controller.BL.Entites;
using Aitoe.Vigilant.Controller.BL.Infra;
using Aitoe.Vigilant.Controller.BL.ServiceInterfaces;
using log4net;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Aitoe.Vigilant.Controller.BL.Ccc
{
    public class ExceptionHandlerEmailService : IEmailService
    {

        private readonly IEmailService _EmailService;
        private Exception EmailException = null;
        private readonly ILog _Log;
        public ExceptionHandlerEmailService(IEmailService emailService, ILog log)
        {
            if (emailService == null)
                throw new ArgumentNullException("Email Service is null");
            _EmailService = emailService;

            if (log == null)
                throw new ArgumentNullException("Log is null");
            _Log = log;
        }

        public ISMTPHost GetSMTPHost()
        {
            try
            {
                return _EmailService.GetSMTPHost();
            }
            catch (Exception ex)
            {
                _Log.Error(MethodBase.GetCurrentMethod().Name + " " + ex.GetaAllMessages());
                EmailException = ex.GetSummaryAitoeBaseException();
                return null;
            }
        }

        public bool? IsSMTPHostSet()
        {
            try
            {
                return _EmailService.IsSMTPHostSet();
            }
            catch (Exception ex)
            {
                _Log.Error(MethodBase.GetCurrentMethod().Name + " " + ex.GetaAllMessages());
                EmailException = ex.GetSummaryAitoeBaseException();
                return null;
            }
        }

        public bool? ResetSMTPHost()
        {
            
            try
            {
                return _EmailService.ResetSMTPHost();
            }
            catch (Exception ex)
            {
                _Log.Error(MethodBase.GetCurrentMethod().Name + " " + ex.GetaAllMessages());
                EmailException = ex.GetSummaryAitoeBaseException();
                return null;
            }
        }

        public async Task<bool> TrySendEmailAsync(IEmail email)
        {
            try
            {
                return await _EmailService.TrySendEmailAsync(email);
            }
            catch (Exception ex)
            {
                _Log.Error(MethodBase.GetCurrentMethod().Name + " " + ex.GetaAllMessages());
                EmailException = ex.GetSummaryAitoeBaseException();
                return false;
            }
        }

        public bool? SetSMTPHost(ISMTPHost smtpHost)
        {
            try
            {
                return _EmailService.SetSMTPHost(smtpHost);
            }
            catch (Exception ex)
            {
                _Log.Error(MethodBase.GetCurrentMethod().Name + " " + ex.GetaAllMessages());
                EmailException = ex.GetSummaryAitoeBaseException();
                return null;
            }
        }       

        public bool TrySendEmail(IEmail email)
        {
            return _EmailService.TrySendEmail(email);
        }
        public Exception GetError()
        {
            return EmailException;
        }
    }
}
