using Aitoe.Vigilant.Controller.BL.ServiceInterfaces;
using Aitoe.Vigilant.Controller.WpfController.Infra;
using Aitoe.Vigilant.Controller.WpfController.Infra.Messages;
using Aitoe.Vigilant.Controller.WpfController.Infra.ViewModelStateEnums;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace Aitoe.Vigilant.Controller.WpfController.ViewModel
{
    public class DropboxSettingsViewModel : AitoeVigilantViewModelBase, IPageViewModel
    {
        public string Name
        {
            get
            {
                return "Dropbox Settings";
            }
        }

        private string _deleteFolderPath;

        public string DeleteFolderPath
        {
            get { return _deleteFolderPath; }
            set
            {
                _deleteFolderPath = value;
                DeleteDropboxFolder.RaiseCanExecuteChanged();
            }
        }

        private DropboxSettingsState _dropboxSettingState;

        public DropboxSettingsState DropboxSettingState
        {
            get { return _dropboxSettingState; }
            set
            {
                if (_dropboxSettingState == value)
                    return;

                _dropboxSettingState = value;
                RaisePropertyChanged();
            }
        }

        private string _createFolderPath;

        public string CreateFolderPath
        {
            get { return _createFolderPath; }
            set
            {
                _createFolderPath = value;
                CreateDropboxFolder.RaiseCanExecuteChanged();
            }
        }

        private string _dropboxUploadToFolderPath;

        public string DropboxUploadToFolderPath
        {
            get { return _dropboxUploadToFolderPath; }
            set
            {
                _dropboxUploadToFolderPath = value;
                UploadFileToDropboxFolder.RaiseCanExecuteChanged();
            }
        }

        private string dropboxUploadFilePath;

        public string DropboxUploadFilePath
        {
            get { return dropboxUploadFilePath; }
            set
            {
                if (dropboxUploadFilePath == value)
                    return;
                dropboxUploadFilePath = value;
                UploadFileToDropboxFolder.RaiseCanExecuteChanged();
                RaisePropertyChanged();
            }
        }

        public Uri DropboxImageSource { get; set; }
        public RelayCommand<string> BackToGridView { get; private set; }
        public RelayCommand<object> ConfigureDropbox { get; private set; }
        public RelayCommand<object> DeleteDropboxFolder { get; private set; }
        public RelayCommand<object> CreateDropboxFolder { get; private set; }
        public RelayCommand<object> DropboxUploadFileBrowse { get; private set; }
        //
        public RelayCommand<object> UploadFileToDropboxFolder { get; private set; }
        public RelayCommand<object> ViewClicked { get; private set; }
        private IDropboxService _DropboxService;
        private IMessageBoxService _MBService;
        public DropboxSettingsViewModel(IDropboxService dbService, IMessageBoxService mbService)
        {
            if (dbService == null)
                throw new ArgumentNullException("IDropboxService is null");

            _DropboxService = dbService;

            if (mbService == null)
                throw new ArgumentNullException("MessageBoxService is null");

            _MBService = mbService;

            DropboxImageSource = new Uri("/Icons/DropBox.png", UriKind.Relative);

            CheckDropboxToken();

            BackToGridView = new RelayCommand<string>(BackToGridViewExecute);
            ConfigureDropbox = new RelayCommand<object>(ConfigureDropboxExecute);
            DeleteDropboxFolder = new RelayCommand<object>(DeleteDropboxFolderExecute, CanDeleteDropboxFolderExecute);
            CreateDropboxFolder = new RelayCommand<object>(CreateDropboxFolderExecute, CanCreateDropboxFolderExecute);
            DropboxUploadFileBrowse = new RelayCommand<object>(DropboxUploadFileBrowseExecute);
            UploadFileToDropboxFolder = new RelayCommand<object>(UploadFileToDropboxFolderExecute, CanUploadFileToDropboxFolderExecute);
            ViewClicked = new RelayCommand<object>(ViewClickedExecute);
            CurrentApp = "aiSentinel";
        }

        private bool CanUploadFileToDropboxFolderExecute(object arg)
        {
            if (!string.IsNullOrWhiteSpace(DropboxUploadFilePath) && !string.IsNullOrWhiteSpace(DropboxUploadToFolderPath))
                return true;
            else
                return false;
        }

        private async void UploadFileToDropboxFolderExecute(object obj)
        {
            var metaDataTask = _DropboxService.UploadFileToDropBox(DropboxUploadFilePath, DropboxUploadToFolderPath);

            DropboxSettingState = DropboxSettingsState.FileUploadInprogress;

            var metaData = await metaDataTask;
            
            var currentApp = _DropboxService.CurrentApplication;

            var fileName = Path.GetFileName(DropboxUploadFilePath);

            var finalPathOnDropbox = "/" + currentApp + "/" + DropboxUploadToFolderPath.Trim(new Char[] { ' ', '/' }) + "/" + fileName;

            var finalPathOnDropboxFromMetadata = metaData.PathDisplay;

            if (finalPathOnDropbox == finalPathOnDropboxFromMetadata)
            {
                // things are ok. So 
                DropboxSettingState = DropboxSettingsState.FileUploadedSuccessfully;
            }
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
        private void DropboxUploadFileBrowseExecute(object obj)
        {
            var ofd = new OpenFileDialog() { Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif|All Files (*.*)|*.*" };
            var result = ofd.ShowDialog();
            if (result == false) return;
            DropboxUploadFilePath = ofd.FileName;
        }

        private bool CanDeleteDropboxFolderExecute(object arg)
        {
            if (string.IsNullOrWhiteSpace(DeleteFolderPath))
                return false;
            else
                return true;
        }

        private bool CanCreateDropboxFolderExecute(object arg)
        {
            if (string.IsNullOrWhiteSpace(CreateFolderPath))
                return false;
            else
                return true;
        }

        private void DeleteDropboxFolderExecute(object obj)
        {
            if (!CheckDropboxToken())
                return;

            var bFolderExists = _DropboxService.IsFolderExists(DeleteFolderPath);

            if (bFolderExists.HasValue && !bFolderExists.Value)
            {
                DropboxSettingState = DropboxSettingsState.FolderDoesNotExist;
                return;
            }
            var deleteStatus = _DropboxService.DeleteDropBoxFolder(DeleteFolderPath);

            if (deleteStatus.HasValue && deleteStatus.Value)
                DropboxSettingState = DropboxSettingsState.FolderDeletedSuccessfully;
            else
                DropboxSettingState = DropboxSettingsState.FolderDeleteFailure;
        }

        public string CurrentApp { get; set; }

        //private string GetFullFolderPath(string sPath)
        //{
        //    //var p = Path.Combine("/", CurrentApp.TrimFwdSlashes());
        //    //p = Path.Combine(p, "/");
        //    //p = Path.Combine(p, CreateFolderPath.TrimFwdSlashes());
        //    //, "/", CreateFolderPath.TrimFwdSlashes()

        //    var p = "/" + CurrentApp.TrimFwdSlashes() + "/" + sPath.TrimFwdSlashes() + "/";
        //    return p;
        //}

        private void CreateDropboxFolderExecute(object obj)
        {
            if (!CheckDropboxToken())
                return;

            var bFolderExists = _DropboxService.IsFolderExists(DeleteFolderPath);
            
            if (bFolderExists.HasValue && bFolderExists.Value)
            {
                DropboxSettingState = DropboxSettingsState.FolderExist;
                return;
            }

            var isFolder = _DropboxService.CreateDropBoxFolder(CreateFolderPath);

            if (isFolder.HasValue && isFolder.Value)
                DropboxSettingState = DropboxSettingsState.FolderCreatedSuccessfully;
            else
                DropboxSettingState = DropboxSettingsState.FolderCreateFailure;
        }

        private void ConfigureDropboxExecute(object obj)
        {
            var win = _DropboxService.GetAppMainWindow();
            WindowInteropHelper helper = new WindowInteropHelper(win);
            helper.Owner = _DropboxService.GetHandle().Value;
            _DropboxService.ShowDialog();

            if (_DropboxService.IsLoginSuccessfull.HasValue && _DropboxService.IsLoginSuccessfull.Value)
                _DropboxService.SetAccessToken();
            else
                _MBService.Show("Error Occured Authenticating dropbox", "Dropbox Authentication", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);

            CheckDropboxToken();
        }

        private MessageType<ToMultiControllerHomeVM> _messageTypeToHomeVM = new MessageType<ToMultiControllerHomeVM>();

        private void BackToGridViewExecute(string obj)
        {
            _messageTypeToHomeVM.Value = new ToMultiControllerHomeVM("ProcessGridView");
            _messageTypeToHomeVM.Message = "Process Grid View to be shown";
            _messageTypeToHomeVM.Event = BroadCastEvents.ShowProcessGridView;
            MessengerInstance.Send(new NotificationMessage<MessageType<ToMultiControllerHomeVM>>(_messageTypeToHomeVM, _messageTypeToHomeVM.Message));
        }

        private bool CheckDropboxToken()
        {
            var bIsTokenExists = _DropboxService.IsAccessTokenExists();
            if (bIsTokenExists.HasValue && bIsTokenExists.Value)
                DropboxSettingState = DropboxSettingsState.DropboxConfigured;
            else
                DropboxSettingState = DropboxSettingsState.DropboxNotConfigured;
            return bIsTokenExists.HasValue && bIsTokenExists.Value;
        }
    }
}
