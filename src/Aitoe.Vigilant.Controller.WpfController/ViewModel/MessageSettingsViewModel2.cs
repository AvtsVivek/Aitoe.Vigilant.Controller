using Aitoe.Vigilant.Controller.WpfController.Properties;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace Aitoe.Vigilant.Controller.WpfController.ViewModel
{
    //public abstract class MessageSettingsViewModel2 : ViewModelBase
    //{
    //    protected string _gbdbmailerPath = "Gbdbmailer.exe";
    //    private string _messageAttachmentFilePath = string.Empty;

    //    private int? _messageConfigurationState = 1;

    //    public int? MessageConfigurationState
    //    {
    //        get
    //        {
    //            return _messageConfigurationState;
    //        }
    //        set
    //        {
    //            if (value == _messageConfigurationState)
    //                return;
    //            _messageConfigurationState = value;
    //            RaisePropertyChanged(() => MessageConfigurationState);
    //        }
    //    }

    //    public string ToAddress { get; set; }
    //    public string ToTitle { get; set; }
    //    public string ToBody { get; set; }

    //    protected string _sCommandArgs = string.Empty;
    //    public RelayCommand<string> MessageAttachmentFileBrowse { get; private set; }
    //    public RelayCommand<string> SendMessage { get; private set; }
    //    public string MessageAttachmentFilePath
    //    {
    //        get
    //        {
    //            return _messageAttachmentFilePath;
    //        }
    //        set
    //        {
    //            if (!File.Exists(value))
    //                return;
    //            if (_messageAttachmentFilePath == value)
    //                return;
    //            _messageAttachmentFilePath = value;
    //            RaisePropertyChanged(() => MessageAttachmentFilePath);
    //        }
    //    }
    //    public MessageSettingsViewModel2()
    //    {
    //        if (!File.Exists(_gbdbmailerPath))
    //            _gbdbmailerPath = Path.Combine(Settings.Default.GbdbmailerDirectoryLocation, @"GbDbMailer.exe");

    //        if (!File.Exists(_gbdbmailerPath))
    //        {
    //            // To do. Log the message for the controller.
    //        }

    //        MessageAttachmentFileBrowse = new RelayCommand<string>(MessageAttachmentFileBrowseExecute);
    //        SendMessage = new RelayCommand<string>(SendMessageExecute);
    //    }

    //    private void MessageAttachmentFileBrowseExecute(string obj)
    //    {
    //        var ofd = new OpenFileDialog() { Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif|All Files (*.*)|*.*" };
    //        var result = ofd.ShowDialog();
    //        if (result == false) return;
    //        MessageAttachmentFilePath = ofd.FileName;
    //    }

    //    protected virtual void SendMessageExecute(string obj)
    //    {
    //        try
    //        {
    //            //_sCommandArgs = "-t " + "\"Test Mail-GbdbMailer 1\" " + "-b \"" + "This is from GbDbMailer. Tesing p option WITH 1 attachment\" " + "-e \"" + "Vivekanand.Swamy@gmail.com\"" + " -m 1 -p 0 -i \"" + "D:\\Lotus123.jpg" + "\"";
    //            var gbdbmailerProcess = new Process();
    //            var processStartInfo = new ProcessStartInfo();
    //            processStartInfo.CreateNoWindow = true;
    //            processStartInfo.UseShellExecute = false;
    //            processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
    //            processStartInfo.FileName = _gbdbmailerPath;

    //            if (!string.IsNullOrEmpty(MessageAttachmentFilePath))
    //                _sCommandArgs = _sCommandArgs + " -i " + "\"" + MessageAttachmentFilePath + "\"";

    //            processStartInfo.Arguments = _sCommandArgs;

    //            gbdbmailerProcess.StartInfo = processStartInfo;

    //            gbdbmailerProcess.EnableRaisingEvents = true;

    //            gbdbmailerProcess.Exited += (s, ea) =>
    //            {
    //                int exitCode = gbdbmailerProcess.ExitCode;
    //                if (exitCode == 0)
    //                    MessageConfigurationState = 6;
    //                else
    //                    MessageConfigurationState = exitCode;
    //            };

    //            MessageConfigurationState = 5;
    //            gbdbmailerProcess.Start();
    //        }
    //        catch (Exception ex)
    //        {
    //            // Log the exception.
    //            //ex.Message
    //            MessageConfigurationState = 8;
    //        }
    //    }
    //}
}
