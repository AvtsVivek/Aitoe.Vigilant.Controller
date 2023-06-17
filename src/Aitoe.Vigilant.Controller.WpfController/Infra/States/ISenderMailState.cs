using Aitoe.Vigilant.Controller.BL.ServiceInterfaces;
using Aitoe.Vigilant.Controller.WpfController.Infra.Extensions;
using Aitoe.Vigilant.Controller.WpfController.Infra.ViewModelStateEnums;
using Aitoe.Vigilant.Controller.WpfController.ViewModel;
using System;

namespace Aitoe.Vigilant.Controller.WpfController.Infra.States
{
    public interface ISenderMailState
    {
        void Change(SenderMailStateContext context, MailSettingsViewModel MSvm, IEmailService emailService);
        MailSettingsStateEnum CurrentState { get; set; }
    }

    public class SendersEmailNotConfiguredState : ISenderMailState
    {
        public MailSettingsStateEnum CurrentState { get; set; }
        public SendersEmailNotConfiguredState()
        {
            CurrentState = MailSettingsStateEnum.SendersEmailNotConfigured;
        }

        public void Change(SenderMailStateContext context, MailSettingsViewModel msvm, IEmailService emailService)
        {
            if (context.CheckForPasswordNotMatchingState(msvm))
                return;
            if (context.CheckForNotConfiguredState(msvm))
                return;
            if (context.CheckForReadyToBeSavedState(msvm, emailService))
                return;
            if (context.CheckForSavedState(msvm, emailService))
                return;
        }
    }

    public class SendersPasswordsDoNotMatchState : ISenderMailState
    {
        public MailSettingsStateEnum CurrentState { get; set; }
        public SendersPasswordsDoNotMatchState()
        {
            CurrentState = MailSettingsStateEnum.SendersPasswordsDoNotMatch;
        }
        public void Change(SenderMailStateContext context, MailSettingsViewModel msvm, IEmailService emailService)
        {
            if (context.CheckForPasswordNotMatchingState(msvm))
                return;
            if (context.CheckForNotConfiguredState(msvm))
                return;
            if (context.CheckForReadyToBeSavedState(msvm, emailService))
                return;
            if (context.CheckForSavedState(msvm, emailService))
                return;

        }
    }

    public class SendersEmailDetailsReadyToBeSavedState : ISenderMailState
    {
        public MailSettingsStateEnum CurrentState { get; set; }
        public SendersEmailDetailsReadyToBeSavedState()
        {
            CurrentState = MailSettingsStateEnum.SendersEmailDetailsReadyToBeSaved;
        }
        public void Change(SenderMailStateContext context, MailSettingsViewModel msvm, IEmailService emailService)
        {
            if (context.CheckForPasswordNotMatchingState(msvm))
                return;
            if (context.CheckForNotConfiguredState(msvm))
                return;
            if (context.CheckForReadyToBeSavedState(msvm, emailService))
                return;
            if (context.CheckForSavedState(msvm, emailService))
                return;
        }
    }

    public class SendersEmailDetailsSavedState : ISenderMailState
    {
        public MailSettingsStateEnum CurrentState { get; set; }
        public SendersEmailDetailsSavedState()
        {
            CurrentState = MailSettingsStateEnum.SendersEmailDetailsSaved;
        }
        private MailSettingsViewModel _MSvm { get; set; }
        public void Change(SenderMailStateContext context, MailSettingsViewModel msvm, IEmailService emailService)
        {
            if (context.CheckForPasswordNotMatchingState(msvm))
                return;
            if (context.CheckForNotConfiguredState(msvm))
                return;
            if (context.CheckForReadyToBeSavedState(msvm, emailService))
                return;
            if (context.CheckForSavedState(msvm, emailService))
                return;
        }
    }

    public class SendersEmailDetailsSaveFailedState : ISenderMailState
    {
        public MailSettingsStateEnum CurrentState { get; set; }
        public SendersEmailDetailsSaveFailedState()
        {
            CurrentState = MailSettingsStateEnum.SendersEmailDetailsSaveFailed;
        }
        private MailSettingsViewModel _MSvm { get; set; }
        public void Change(SenderMailStateContext context, MailSettingsViewModel msvm, IEmailService emailService)
        {
            throw new NotImplementedException();
        }
    }

}
