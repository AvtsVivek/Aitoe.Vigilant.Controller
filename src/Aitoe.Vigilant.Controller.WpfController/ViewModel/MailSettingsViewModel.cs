using Aitoe.Vigilant.Controller.BL.Entites;
using Aitoe.Vigilant.Controller.BL.ExceptionDefs;
using Aitoe.Vigilant.Controller.BL.ServiceInterfaces;
using Aitoe.Vigilant.Controller.WpfController.Infra;
using Aitoe.Vigilant.Controller.WpfController.Infra.Messages;
using Aitoe.Vigilant.Controller.WpfController.Infra.States;
using Aitoe.Vigilant.Controller.WpfController.Infra.ViewModelStateEnums;
using AutoMapper;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;

namespace Aitoe.Vigilant.Controller.WpfController.ViewModel
{
    public class MailSettingsViewModel : MessageSettingsViewModel, IPageViewModel
    {
        public string Name
        {
            get
            {
                return "Mail Settings";
            }
        }

        private MailSettingsStateEnum _mailSettingState;

        public MailSettingsStateEnum MailSettingState
        {
            get { return _mailSettingState; }
            set
            {
                if (_mailSettingState == value)
                    return;

                _mailSettingState = value;
                RaisePropertyChanged();
            }
        }
        
        private string _sendersEmailAddress = string.Empty;
        public string SendersEmailAddress
        {
            get { return _sendersEmailAddress; }
            set
            {
                _sendersEmailAddress = value;
                _Context.Change();
                ConfigureSendersEmail.RaiseCanExecuteChanged();
                RaisePropertyChanged();
            }
        }
        private string _emailSendersPassword = string.Empty;
        public string EmailSendersPassword
        {
            get
            {
                return _emailSendersPassword;
            }

            set
            {
                _emailSendersPassword = value;
                _Context.Change();
                RaisePropertyChanged();
                ConfigureSendersEmail.RaiseCanExecuteChanged();
            }
        }

        private string _emailSendersPassword2 = string.Empty;
        public string EmailSendersPassword2
        {
            get
            {
                return _emailSendersPassword2;
            }
            set
            {
                _emailSendersPassword2 = value;
                
                _Context.Change();
                RaisePropertyChanged();
                ConfigureSendersEmail.RaiseCanExecuteChanged();
            }
        }

        private string _smptServer;

        public string SMTPServer
        {
            get { return _smptServer; }
            set
            {
                _smptServer = value;
                _Context.Change();
                ConfigureSendersEmail.RaiseCanExecuteChanged();
                RaisePropertyChanged();
            }
        }

        private int? _smptPort;

        public int? SMTPPort
        {
            get { return _smptPort; }
            set
            {
                //if (!value.HasValue)
                //    return;

                _smptPort = value;
                _Context.Change();
                ConfigureSendersEmail.RaiseCanExecuteChanged();
                RaisePropertyChanged();
            }
        }
                
        //private readonly IEmailService _EmailService;
        
        private SenderMailStateContext _Context;

        public RelayCommand<string> ConfigureSendersEmail { get; private set; }
        public RelayCommand<string> ResetSMTPHost { get; private set; }
        public RelayCommand<string> BackToGridView { get; private set; }
        public Uri EmailImage { get; private set; }

        protected readonly IEmailService _EmailService;
        protected readonly IMapper _InternalMapper;
        public RelayCommand<object> ViewClicked { get; private set; }

        public MailSettingsViewModel(IEmailService emailService, IMapper mapper) 
        {
            if (emailService == null)
                throw new ArgumentNullException("Email service is null");
            _EmailService = emailService;

            if (mapper == null)
                throw new ArgumentNullException("Auto mapper is null");
            _InternalMapper = mapper;

            EmailImage = new Uri("/Icons/Email.png", UriKind.Relative);
            var notConfigured = new SendersEmailNotConfiguredState();

            _Context = new SenderMailStateContext(notConfigured, this, emailService);

            ResetSMTPHost = new RelayCommand<string>(ResetSMTPHostExecute, CanResetSMTPHostExecute);
            BackToGridView = new RelayCommand<string>(BackToGridViewExecute);
            ViewClicked = new RelayCommand<object>(ViewClickedExecute);
            ConfigureSendersEmail = new RelayCommand<string>(ConfigureSendersEmailExecute, CanConfigureSendersEmailExecute);
            ISMTPHost host;
            if (_EmailService.IsSMTPHostSet().HasValue && _EmailService.IsSMTPHostSet().Value)
            {
                host = _EmailService.GetSMTPHost();
                _InternalMapper.Map(host, this);
            }
            //_EmailService.ResetSMTPHost();
            _Context.Change();
        }

        private void ViewClickedExecute(object parm)
        {
            if (parm.ToString() == "GoToMultiCamera")
            {
                _messageTypeToHomeVM.Value = new ToMultiControllerHomeVM("ProcessGridView");
                _messageTypeToHomeVM.Message = "Process Grid View to be shown";
                _messageTypeToHomeVM.Event = BroadCastEvents.ShowProcessGridView;
                MessengerInstance.Send(new NotificationMessage<MessageType<ToMultiControllerHomeVM>>(_messageTypeToHomeVM, _messageTypeToHomeVM.Message));
            }
        }

        private MessageType<ToMultiControllerHomeVM> _messageTypeToHomeVM = new MessageType<ToMultiControllerHomeVM>();
        private void BackToGridViewExecute(string obj)
        {
            _messageTypeToHomeVM.Value = new ToMultiControllerHomeVM("ProcessGridView");
            _messageTypeToHomeVM.Message = "Process Grid View to be shown";
            _messageTypeToHomeVM.Event = BroadCastEvents.ShowProcessGridView;
            MessengerInstance.Send(new NotificationMessage<MessageType<ToMultiControllerHomeVM>>(_messageTypeToHomeVM, _messageTypeToHomeVM.Message));
        }

        private bool CanResetSMTPHostExecute(string arg)
        {
            bool bResetEnabled = false;
            switch (MailSettingState)
            {
                case MailSettingsStateEnum.SendersEmailNotConfigured:
                    bResetEnabled = false;
                    break;
                case MailSettingsStateEnum.SendersPasswordsDoNotMatch:
                    bResetEnabled = false;
                    break;
                case MailSettingsStateEnum.SendersEmailDetailsReadyToBeSaved:
                    bResetEnabled = false;
                    break;
                case MailSettingsStateEnum.SendersEmailDetailsSaved:
                    bResetEnabled = true;
                    break;
                case MailSettingsStateEnum.SendersEmailDetailsSaveFailed:
                    bResetEnabled = true;
                    break;
            }
            
            return bResetEnabled;
        }

        private void ResetSMTPHostExecute(string obj)
        {
            _EmailService.ResetSMTPHost();

            EmailSendersPassword = string.Empty;
            EmailSendersPassword2 = string.Empty;
            SendersEmailAddress = string.Empty;
            SMTPServer = string.Empty;
            SMTPPort = null;
            SendMessage.RaiseCanExecuteChanged();
        }

        private bool CanConfigureSendersEmailExecute(string arg)
        {
            if (
                MailSettingState == MailSettingsStateEnum.SendersEmailDetailsReadyToBeSaved
                || MailSettingState == MailSettingsStateEnum.SendersEmailDetailsSaved
                )
            {
                return true;
            }
            else
                return false;
        }

        protected override bool CanSendMessageExecute(string arg)
        {
            if (!string.IsNullOrEmpty(ToAddress) &&
                !string.IsNullOrEmpty(ToBody) &&
                !string.IsNullOrEmpty(ToTitle) &&
                MailSettingState == MailSettingsStateEnum.SendersEmailDetailsSaved
                )
                return true;
            else
                return false;
        }

        private void ConfigureSendersEmailExecute(string obj)
        {
            //if (MailSettingState1 != MailSettingsStateEnum.SendersEmailDetailsReadyToBeSaved)
            //    return; // Do noting. Just return.
            var smptHost = _InternalMapper.Map<ISMTPHost>(this);
            bool? t = _EmailService.SetSMTPHost(smptHost);
            //MailSettingState1 = MailSettingsStateEnum.SendersEmailDetailsSaved;
            _Context.Change();
            SendMessage.RaiseCanExecuteChanged();
        }

        protected async override void SendMessageExecute(string obj)
        {
            //http://stackoverflow.com/a/14906489/1977871
            //http://stackoverflow.com/a/13494570/1977871
            var email = _InternalMapper.Map<IEmail>(this);
            var resp = _EmailService.TrySendEmailAsync(email);
            MessageSendResult = "Sending ....";
            await resp;

            if (!resp.Result)
            {
                MessageSendResult = "Failure sending message";
                var error = _EmailService.GetError();
                if (error is AitoeBaseException)
                {
                    var aitoeException = error as AitoeBaseException;
                    MessageSendResult = aitoeException.GetErrorCode().ToString() + ". See logs for more info.";
                }
            }
            else
            {
                MessageSendResult = "Message sent successifully";
            }
        }
    }
}