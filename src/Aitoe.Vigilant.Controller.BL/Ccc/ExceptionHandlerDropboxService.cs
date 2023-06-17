using Aitoe.Vigilant.Controller.BL.Infra;
using Aitoe.Vigilant.Controller.BL.ServiceInterfaces;
using Dropbox.Api;
using Dropbox.Api.Files;
using log4net;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace Aitoe.Vigilant.Controller.BL.Ccc
{
    public class ExceptionHandlerDropboxService : IDropboxService
    {
        private readonly IDropboxService _DropboxService;
        private Exception _DropboxException = null;
        private readonly ILog _Log;
        public ExceptionHandlerDropboxService(IDropboxService dbService, ILog log)
        {
            if (dbService == null)
                throw new ArgumentNullException("Dropbox Service is null");
            _DropboxService = dbService;

            if (log == null)
                throw new ArgumentNullException("Log is null");
            _Log = log;
        }

        public string CurrentApplication
        {
            get
            {
                try
                {
                    return _DropboxService.CurrentApplication;
                }
                catch (Exception ex)
                {
                    _Log.Error(MethodBase.GetCurrentMethod().Name + " " + ex.GetaAllMessages());
                    _DropboxException = ex.GetSummaryAitoeBaseException();
                    return null;
                }
            }

            set
            {
                try
                {
                    _DropboxService.CurrentApplication = value;
                }
                catch (Exception ex)
                {
                    _Log.Error(MethodBase.GetCurrentMethod().Name + " " + ex.GetaAllMessages());
                    _DropboxException = ex.GetSummaryAitoeBaseException();
                }
            }
        }

        public bool? IsLoginSuccessfull
        {
            get
            {
                try
                {
                    return _DropboxService.IsLoginSuccessfull;
                }
                catch (Exception ex)
                {
                    _Log.Error(MethodBase.GetCurrentMethod().Name + " " + ex.GetaAllMessages());
                    _DropboxException = ex.GetSummaryAitoeBaseException();
                    return null;
                }
            }
        }

        public bool? CreateDropBoxFolder(string folderPath)
        {
            try
            {
                return _DropboxService.CreateDropBoxFolder(folderPath);
            }
            catch (Exception ex)
            {
                _Log.Error(MethodBase.GetCurrentMethod().Name + " " + ex.GetaAllMessages());
                _DropboxException = ex.GetSummaryAitoeBaseException();
                return null;
            }
        }

        public bool? DeleteDropBoxFolder(string folderPath)
        {
            try
            {
                return _DropboxService.DeleteDropBoxFolder(folderPath);
            }
            catch (Exception ex)
            {
                _Log.Error(MethodBase.GetCurrentMethod().Name + " " + ex.GetaAllMessages());
                _DropboxException = ex.GetSummaryAitoeBaseException();
                return false;
            }
        }

        public string GetAccessToken()
        {
            try
            {
                return _DropboxService.GetAccessToken();
            }
            catch (Exception ex)
            {
                _Log.Error(MethodBase.GetCurrentMethod().Name + " " + ex.GetaAllMessages());
                _DropboxException = ex.GetSummaryAitoeBaseException();
                return null;
            }
        }

        public Window GetAppMainWindow()
        {
            try
            {
                return _DropboxService.GetAppMainWindow();
            }
            catch (Exception ex)
            {
                _Log.Error(MethodBase.GetCurrentMethod().Name + " " + ex.GetaAllMessages());
                _DropboxException = ex.GetSummaryAitoeBaseException();
                return null;
            }
        }

        public Exception GetError()
        {
            return _DropboxException;
        }

        public IntPtr? GetHandle()
        {
            try
            {
                return _DropboxService.GetHandle();
            }
            catch (Exception ex)
            {
                _Log.Error(MethodBase.GetCurrentMethod().Name + " " + ex.GetaAllMessages());
                _DropboxException = ex.GetSummaryAitoeBaseException();
                return null;
            }
        }

        public string GetRootFolder()
        {
            try
            {
                return _DropboxService.GetRootFolder();
            }
            catch (Exception ex)
            {
                _Log.Error(MethodBase.GetCurrentMethod().Name + " " + ex.GetaAllMessages());
                _DropboxException = ex.GetSummaryAitoeBaseException();
                return null;
            }
        }

        public bool? IsAccessTokenExists()
        {
            try
            {
                return _DropboxService.IsAccessTokenExists();
            }
            catch (Exception ex)
            {
                _Log.Error(MethodBase.GetCurrentMethod().Name + " " + ex.GetaAllMessages());
                _DropboxException = ex.GetSummaryAitoeBaseException();
                return null;
            }
        }

        public bool? IsFolderExists(string folderPath)
        {
            try
            {
                return _DropboxService.IsFolderExists(folderPath);
            }
            catch (Exception ex)
            {
                _Log.Error(MethodBase.GetCurrentMethod().Name + " " + ex.GetaAllMessages());
                _DropboxException = ex.GetSummaryAitoeBaseException();
                return null;
            }
        }

        public SearchResult SearchDropBoxFolder(DropboxClient client, string rootFolder, string folderToSearch)
        {
            try
            {
                return _DropboxService.SearchDropBoxFolder(client, rootFolder, folderToSearch);
            }
            catch (Exception ex)
            {
                _Log.Error(MethodBase.GetCurrentMethod().Name + " " + ex.GetaAllMessages());
                _DropboxException = ex.GetSummaryAitoeBaseException();
                return null;
            }
        }

        public bool? SetAccessToken()
        {
            try
            {
                return _DropboxService.SetAccessToken();
            }
            catch (Exception ex)
            {
                _Log.Error(MethodBase.GetCurrentMethod().Name + " " + ex.GetaAllMessages());
                _DropboxException = ex.GetSummaryAitoeBaseException();
                return null;
            }
        }

        public DialogResult? ShowDialog()
        {
            try
            {
                return _DropboxService.ShowDialog();
            }
            catch (Exception ex)
            {
                _Log.Error(MethodBase.GetCurrentMethod().Name + " " + ex.GetaAllMessages());
                _DropboxException = ex.GetSummaryAitoeBaseException();
                return null;
            }
        }

        public async Task<FileMetadata> UploadFileToDropBox(string fileToUpload, string folder)
        {
            try
            {
                return await _DropboxService.UploadFileToDropBox(fileToUpload, folder);
            }
            catch (Exception ex)
            {
                _Log.Error(MethodBase.GetCurrentMethod().Name + " " + ex.GetaAllMessages());
                _DropboxException = ex.GetSummaryAitoeBaseException();
                return null;
            }
        }
    }
}
