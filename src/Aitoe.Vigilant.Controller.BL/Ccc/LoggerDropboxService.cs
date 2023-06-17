using Aitoe.Vigilant.Controller.BL.ServiceInterfaces;
using Dropbox.Api;
using Dropbox.Api.Files;
using log4net;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace Aitoe.Vigilant.Controller.BL.Ccc
{
    public class LoggerDropboxService : IDropboxService
    {
        private readonly IDropboxService _DropboxService;
        private readonly ILog _Log;
        public LoggerDropboxService(IDropboxService dropboxService, ILog log)
        {
            if (dropboxService == null)
                throw new ArgumentNullException("Dropbox Service is null");
            _DropboxService = dropboxService;

            if (log == null)
                throw new ArgumentNullException("Log is null");
            _Log = log;
            _Log.Info("LoggerDropboxService");
        }
        public string CurrentApplication
        {
            get
            {
                _Log.Info("CurrentApplication");
                return _DropboxService.CurrentApplication;
            }

            set
            {
                _Log.Info("CurrentApplication");
                _DropboxService.CurrentApplication = value;
            }
        }

        public bool? IsLoginSuccessfull
        {
            get
            {
                _Log.Info("IsLoginSuccessfull");
                return _DropboxService.IsLoginSuccessfull;
            }
        }

        public bool? CreateDropBoxFolder(string folderPath)
        {
            _Log.Info("CreateDropBoxFolder");
            return _DropboxService.CreateDropBoxFolder(folderPath);
        }

        public bool? DeleteDropBoxFolder(string folderPath)
        {
            _Log.Info("DeleteDropBoxFolder");
            return _DropboxService.DeleteDropBoxFolder(folderPath);
        }

        public string GetAccessToken()
        {
            _Log.Info("GetAccessToken");
            return _DropboxService.GetAccessToken();
        }

        public Window GetAppMainWindow()
        {
            _Log.Info("GetAppMainWindow");
            return _DropboxService.GetAppMainWindow();
        }

        public Exception GetError()
        {
            _Log.Info("GetError");
            return _DropboxService.GetError();
        }

        public IntPtr? GetHandle()
        {
            _Log.Info("GetHandle");
            return _DropboxService.GetHandle();
        }

        public string GetRootFolder()
        {
            _Log.Info("GetRootFolder");
            return _DropboxService.GetRootFolder();
        }

        public bool? IsAccessTokenExists()
        {
            _Log.Info("IsAccessTokenExists");
            return _DropboxService.IsAccessTokenExists();
        }

        public bool? IsFolderExists(string folderPath)
        {
            _Log.Info("IsFolderExists");
            return _DropboxService.IsFolderExists(folderPath);
        }

        public SearchResult SearchDropBoxFolder(DropboxClient client, string rootFolder, string folderToSearch)
        {
            _Log.Info("SearchDropBoxFolder");
            return _DropboxService.SearchDropBoxFolder(client, rootFolder, folderToSearch);
        }

        public bool? SetAccessToken()
        {
            _Log.Info("SetAccessToken");
            return _DropboxService.SetAccessToken();
        }

        public DialogResult? ShowDialog()
        {
            _Log.Info("ShowDialog");
            return _DropboxService.ShowDialog();
        }

        public async Task<FileMetadata> UploadFileToDropBox(string fileToUpload, string folder)
        {
            _Log.Info("UploadFileToDropBox");
            return await _DropboxService.UploadFileToDropBox(fileToUpload, folder);
        }
    }
}
