using Aitoe.Vigilant.Controller.BL.ServiceInterfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using AutoMapper;
using Aitoe.Vigilant.Controller.BL.Entites;
using Aitoe.Vigilant.Controller.BL.ExceptionDefs;

namespace Aitoe.Vigilant.Controller.WpfController.ViewModel
{
    public abstract class MessageSettingsViewModel : AitoeVigilantViewModelBase
    {
        private string _toAddress = string.Empty;
        public string ToAddress
        {
            get { return _toAddress; }
            set
            {
                if (_toAddress == value)
                    return;
                _toAddress = value;
                SendMessage.RaiseCanExecuteChanged();
                RaisePropertyChanged();
            }
        }
        private string _toTitle = string.Empty;
        public string ToTitle
        {
            get { return _toTitle; }
            set
            {
                if (_toTitle == value)
                    return;
                _toTitle = value;
                SendMessage.RaiseCanExecuteChanged();
                RaisePropertyChanged();
            }
        }
        private string _toBody = string.Empty;
        public string ToBody
        {
            get { return _toBody; }
            set
            {
                if (_toBody == value)
                    return;
                _toBody = value;
                SendMessage.RaiseCanExecuteChanged();
                RaisePropertyChanged();
            }
        }

        private string _messageAttachmentFilePath = string.Empty;
        public string MessageAttachmentFilePath
        {
            get
            {
                return _messageAttachmentFilePath;
            }
            set
            {
                if (!File.Exists(value))
                    return;
                if (_messageAttachmentFilePath == value)
                    return;
                _messageAttachmentFilePath = value;
                RaisePropertyChanged();
            }
        }

        public List<string> Attachments
        {
            get
            {
                var list = new List<string>();
                if (!string.IsNullOrEmpty(MessageAttachmentFilePath))
                {
                    list.Add(MessageAttachmentFilePath);
                }
                return list;
            }
        }

        public List<string> ToAddressList
        {
            get
            {
                var list = new List<string>();
                if (!string.IsNullOrEmpty(ToAddress))
                {
                    list.Add(ToAddress);
                }
                return list;
            }
        }

        public RelayCommand<string> MessageAttachmentFileBrowse { get; private set; }
        public RelayCommand<string> SendMessage { get; private set; }

        public MessageSettingsViewModel()
        {
            MessageAttachmentFileBrowse = new RelayCommand<string>(MessageAttachmentFileBrowseExecute);
            SendMessage = new RelayCommand<string>(SendMessageExecute, CanSendMessageExecute);
        }

        protected virtual bool CanSendMessageExecute(string arg)
        {
            if (!string.IsNullOrEmpty(ToAddress) &&
                !string.IsNullOrEmpty(ToBody) &&
                !string.IsNullOrEmpty(ToTitle)
                )
                return true;
            else
                return false;
        }

        private void MessageAttachmentFileBrowseExecute(string obj)
        {
            var ofd = new OpenFileDialog() { Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif|All Files (*.*)|*.*" };
            var result = ofd.ShowDialog();
            if (result == false) return;
            MessageAttachmentFilePath = ofd.FileName;
        }

        private string  messageSendResult;

        public string MessageSendResult
        {
            get { return messageSendResult; }
            set
            {
                messageSendResult = value;
                RaisePropertyChanged();
            }
        }
        
        protected virtual void SendMessageExecute(string obj)
        {

        }
    }
}
