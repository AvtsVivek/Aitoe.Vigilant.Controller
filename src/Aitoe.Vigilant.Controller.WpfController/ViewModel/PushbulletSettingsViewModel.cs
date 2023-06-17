using Aitoe.Vigilant.Controller.BL.Entites;
using Aitoe.Vigilant.Controller.BL.ServiceInterfaces;
using Aitoe.Vigilant.Controller.WpfController.Infra;
using Aitoe.Vigilant.Controller.WpfController.Infra.Messages;
using AutoMapper;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace Aitoe.Vigilant.Controller.WpfController.ViewModel
{
    public class PushbulletSettingsViewModel : MessageSettingsViewModel, IPageViewModel
    {
        public string Name
        {
            get
            {
                return "Pushbullet Settings";
            }
        }       

        public Uri PushbulletImageSource { get; set; }
        public RelayCommand<object> ConfigurePushBullet { get; private set; }
        public RelayCommand<string> BackToGridView { get; private set; }
        public RelayCommand<object> ViewClicked { get; private set; }
        protected readonly IPushbulletService _PushbulletService;
        protected readonly IMapper _InternalMapper;
        //ConfigurePushBullet
        public PushbulletSettingsViewModel(IPushbulletService pushbulletService, IMapper mapper)
        {
            if (pushbulletService == null)
                throw new ArgumentNullException("Pushbullet service is null");
            _PushbulletService = pushbulletService;


            if (mapper == null)
                throw new ArgumentNullException("Auto mapper is null");
            _InternalMapper = mapper;

            PushbulletImageSource = new Uri("/Icons/Pushbullet-icon.png", UriKind.Relative);
            SetPushbulletConfigurationMessageVisibility();
            ConfigurePushBullet = new RelayCommand<object>(ConfigurePushBulletExecute);
            BackToGridView = new RelayCommand<string>(BackToGridViewExecute);
            ViewClicked = new RelayCommand<object>(ViewClickedExecute);
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


        private void AttachmentFileBrowseExecute(string obj)
        {
            var ofd = new OpenFileDialog() { Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif|All Files (*.*)|*.*" };
            var result = ofd.ShowDialog();
            if (result == false) return;
            MessageAttachmentFilePath = ofd.FileName;
        }

        protected override void SendMessageExecute(string obj)
        {
            var email = _InternalMapper.Map<IEmail>(this);
            var resp = _PushbulletService.SendPushbulletMessage(email);

            //_PushbulletService.SendPushbulletMessage();
            //try
            //{
            //    var _sCommandArgs = "-t " + "\"Test Mail-GbdbMailer 1\" " + "-b \"" + "This is from GbDbMailer. Tesing p option WITH 1 attachment\" " + "-e \"" + "Vivekanand.Swamy@gmail.com\"" + " -m 1 -p 0 -i \"" + "D:\\Lotus123.jpg" + "\"";
            //    var gbdbmailerProcess = new Process();
            //    var processStartInfo = new ProcessStartInfo();
            //    processStartInfo.CreateNoWindow = true;
            //    processStartInfo.UseShellExecute = false;
            //    processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            //    processStartInfo.FileName = _gbdbmailerPath;

            //    if (!string.IsNullOrEmpty(MessageAttachmentFilePath))
            //        _sCommandArgs = _sCommandArgs + " -i " + "\"" + MessageAttachmentFilePath + "\"";

            //    processStartInfo.Arguments = _sCommandArgs;

            //    gbdbmailerProcess.StartInfo = processStartInfo;

            //    gbdbmailerProcess.EnableRaisingEvents = true;

            //    gbdbmailerProcess.Exited += (s, ea) =>
            //    {
            //        int exitCode = gbdbmailerProcess.ExitCode;
            //        if (exitCode == 0)
            //            MessageConfigurationState = 6;
            //        else
            //            MessageConfigurationState = exitCode;
            //    };

            //    MessageConfigurationState = 5;
            //    gbdbmailerProcess.Start();
            //}
            //catch (Exception ex)
            //{
            //    // Log the exception.
            //    //ex.Message
            //    MessageConfigurationState = 8;
            //}
        }


        private Visibility _pushbulletConfigurationMessageVisibility;
        public Visibility PushbulletConfigurationMessageVisibility
        {
            get{ return _pushbulletConfigurationMessageVisibility; }
            set
            {
                if (_pushbulletConfigurationMessageVisibility == value)
                    return;
                _pushbulletConfigurationMessageVisibility = value;
                RaisePropertyChanged(() => PushbulletConfigurationMessageVisibility);
            }
        }

        private void ConfigurePushBulletExecute(object obj)
        {
            if (_PushbulletService.IsAccessTokenExists())
            {
                var sMessage = "Pushbullet is already configured. Do you want to reconfigure?";
                sMessage = sMessage + Environment.NewLine + "Choose Yes to delete old and create a new configuration.";
                sMessage = sMessage + Environment.NewLine + "Choose No to continue with the existing configuration.";
                var mbResult = MessageBox.Show(sMessage, "Pushbullet configuration", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (mbResult == MessageBoxResult.No)
                    return;
            }

            if (_PushbulletService.ConfigurePushbullet())
                SetPushbulletConfigurationMessageVisibility();
            else
            {
                // Some error.
            }
        }

        private void SetPushbulletConfigurationMessageVisibility()
        {
            if (_PushbulletService.IsAccessTokenExists())
                PushbulletConfigurationMessageVisibility = Visibility.Collapsed;
            else
                PushbulletConfigurationMessageVisibility = Visibility.Visible;
        }
    }
}