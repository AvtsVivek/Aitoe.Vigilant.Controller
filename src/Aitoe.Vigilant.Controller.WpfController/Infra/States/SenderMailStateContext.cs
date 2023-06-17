using Aitoe.Vigilant.Controller.BL.ServiceInterfaces;
using Aitoe.Vigilant.Controller.WpfController.ViewModel;

namespace Aitoe.Vigilant.Controller.WpfController.Infra.States
{
    public class SenderMailStateContext
    {
        private IEmailService _EmailService;
        private MailSettingsViewModel _Mvm;
        public SenderMailStateContext(ISenderMailState state, MailSettingsViewModel vm, IEmailService emailService)
        {
            _Mvm = vm;
            _EmailService = emailService;
            SenderMailState = state;
        }

        private ISenderMailState senderMailState;

        public ISenderMailState SenderMailState
        {
            get { return senderMailState; }
            set
            {
                if (senderMailState != null && senderMailState.GetType() == value.GetType())
                    return;
                senderMailState = value;
                _Mvm.MailSettingState = senderMailState.CurrentState;
            }
        }
        
        public void Change()
        {
            SenderMailState.Change(this, _Mvm, _EmailService);
        }
    }
}
