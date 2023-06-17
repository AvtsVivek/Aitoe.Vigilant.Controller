using Aitoe.Vigilant.Controller.BL.ServiceInterfaces;
using Aitoe.Vigilant.Controller.WpfController.Infra.States;
using Aitoe.Vigilant.Controller.WpfController.ViewModel;

namespace Aitoe.Vigilant.Controller.WpfController.Infra.Extensions
{
    public static class SenderMailStateContextExtensions
    {
        public static bool CheckForNotConfiguredState(this SenderMailStateContext context, MailSettingsViewModel msvm)
        {
            if(
                string.IsNullOrEmpty(msvm.SendersEmailAddress) ||
                string.IsNullOrEmpty(msvm.EmailSendersPassword) ||
                string.IsNullOrEmpty(msvm.EmailSendersPassword2) ||
                string.IsNullOrEmpty(msvm.SMTPServer) ||
                !msvm.SMTPPort.HasValue 
            )
            {
                if (context.SenderMailState is SendersEmailNotConfiguredState)
                    return true;
                else
                {
                    context.SenderMailState = new SendersEmailNotConfiguredState();
                    return true;
                }
            }
            return false;
        }
        public static bool CheckForPasswordNotMatchingState(this SenderMailStateContext context, MailSettingsViewModel msvm)
        {
            if (
                msvm.EmailSendersPassword != msvm.EmailSendersPassword2
                )
            {

                if (context.SenderMailState is SendersPasswordsDoNotMatchState)
                    return true;
                
                else
                {
                    context.SenderMailState = new SendersPasswordsDoNotMatchState();
                    return true;
                }
            }
            return false;
        }
        public static bool CheckForReadyToBeSavedState(this SenderMailStateContext context, MailSettingsViewModel msvm, IEmailService emailService)
        {
            if (
                !string.IsNullOrEmpty(msvm.SendersEmailAddress) &&
                !string.IsNullOrEmpty(msvm.EmailSendersPassword) &&
                !string.IsNullOrEmpty(msvm.EmailSendersPassword2) &&
                !string.IsNullOrEmpty(msvm.SMTPServer) &&
                msvm.SMTPPort.HasValue &&
                msvm.EmailSendersPassword == msvm.EmailSendersPassword2 &&
                !emailService.IsSMTPHostSet().Value
                )
            {
                if (context.SenderMailState is SendersEmailDetailsReadyToBeSavedState)
                    return true;
                else
                {
                    context.SenderMailState = new SendersEmailDetailsReadyToBeSavedState();
                    return true;
                }
            }
            return false;
        }
        public static bool CheckForSavedState(this SenderMailStateContext context, MailSettingsViewModel msvm, IEmailService emailService)
        {
            if (emailService.IsSMTPHostSet().HasValue && emailService.IsSMTPHostSet().Value)
            {
                if (context.SenderMailState is SendersEmailDetailsSavedState)
                    return true;
                else
                {
                    context.SenderMailState = new SendersEmailDetailsSavedState();
                    return true;
                }
            }
            return false;
        }
    }
}
