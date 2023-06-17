using Aitoe.Vigilant.Controller.BL.ServiceInterfaces;
using Aitoe.Vigilant.Controller.DL.Properties;
using Nemiro.OAuth.LoginForms;
using System;
using System.Windows.Forms;
using System.Windows;
using Dropbox.Api;
using System.Threading.Tasks;
using Dropbox.Api.Files;
using System.IO;
using System.Globalization;
using Aitoe.Vigilant.Controller.BL.Infra;

namespace Aitoe.Vigilant.Controller.DL.Services
{
    public class DropboxService : IDropboxService
    {
        private string _InnerFolder = DateTime.Now.Year.ToString() + "-" + CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(DateTime.Now.Month).ToUpper() + "-" + DateTime.Now.Day.ToString();
        private DropboxLogin _Login;
        public DropboxService()
        {
            CurrentApplication = "aiSentinel";
        }
        private string GetFullFolderPath(string sPath)
        {
            //var p = Path.Combine("/", CurrentApp.TrimFwdSlashes());
            //p = Path.Combine(p, "/");
            //p = Path.Combine(p, CreateFolderPath.TrimFwdSlashes());
            //, "/", CreateFolderPath.TrimFwdSlashes()
            //var sFullPathWithCurrentAppTrimmed = Path.Combine("/", folderPath.Trim(new Char[] { ' ', '/' })).Replace("\\", "/");
            var p = "/" + CurrentApplication.TrimFwdSlashes() + "/" + sPath.TrimFwdSlashes() ;
            return p;
        }
        public bool? DeleteDropBoxFolder(string folderPath)
        {
            // Prefex AitoeData
            //var fullpath = Path.Combine(Path.Combine("/", Settings.Default.DropboxSaveToFolder), folderPath.Trim(new Char[] { ' ', '/' })).Replace("\\", "/");
            string sFullPathWithCurrentApp = GetFullFolderPath(folderPath);
            //Task<FolderMetadata> fm = client.Files.CreateFolderAsync(sFullPathWithCurrentApp);
            //fm.Wait();
            //return fm.Result;

            // Try catch block has to be removed.
            Task<Metadata> task = null;
            Metadata metaData = null;
            try
            {
                DropboxClient client = new DropboxClient(GetAccessToken());
                //string fullpath = Path.Combine(Path.Combine("/", Settings.Default.DropboxSaveToFolder), folderPath.Trim(new Char[] { ' ', '/' })).Replace("\\", "/");
                //string fullpath = Path.Combine("/", folderPath.Trim(new Char[] { ' ', '/' })).Replace("\\", "/");

                //string aitoeDataFolderPath = Path.Combine("/", Settings.Default.DropboxSaveToFolder);
                //task = client.Files.GetMetadataAsync(aitoeDataFolderPath);
                //task.Wait();
                //metaData = task.Result;

                //task = client.Files.GetMetadataAsync(fullpath);
                //task.Wait();
                //metaData = task.Result;               

                task = client.Files.DeleteAsync(sFullPathWithCurrentApp);
                task.Wait();
                metaData = task.Result;
                return true;
            }
            catch (AggregateException agExcep)
            {
                string messages = "";
                foreach (Exception ex in agExcep.InnerExceptions)
                {
                    messages += ex.Message + "\r\n";
                }
                return false;
            }
            catch (Exception excep)
            {
                return false;
            }
        }

        public bool? IsFolderExists(string folderPath)
        {
            var bFolderExits = false;
            // Try catch block has to be removed.
            Task<Metadata> task = null;
            Metadata metaData = null;
            try
            {
                DropboxClient client = new DropboxClient(GetAccessToken());
                //string fullpath = Path.Combine(Path.Combine("/", Settings.Default.DropboxSaveToFolder), folderPath.Trim(new Char[] { ' ', '/' })).Replace("\\", "/");
                //string fullpath = Path.Combine("/", folderPath.Trim(new Char[] { ' ', '/' })).Replace("\\", "/");
                //string aitoeDataFolderPath = Path.Combine("/", Settings.Default.DropboxSaveToFolder);
                //task = client.Files.GetMetadataAsync(aitoeDataFolderPath);
                //task.Wait();
                //metaData = task.Result;
                string sFullPathWithCurrentApp = GetFullFolderPath(folderPath);
                task = client.Files.GetMetadataAsync(sFullPathWithCurrentApp);
                task.Wait();
                metaData = task.Result;
                bFolderExits = true;
            }
            catch (AggregateException agExcep)
            {
                string messages = "";
                foreach (Exception ex in agExcep.InnerExceptions)
                {
                    messages += ex.Message + "\r\n";
                }
                if (messages.Contains(@"path/not_found/"))
                {
                    bFolderExits = false;
                }
            }
            return bFolderExits;
        }

        public async Task<FileMetadata> UploadFileToDropBox(string fileToUpload, string folder)
        {

            var config = new DropboxClientConfig();
            config.HttpClient = new System.Net.Http.HttpClient();
            config.HttpClient.Timeout = new TimeSpan(0, 20, 0);

            DropboxClient client = new DropboxClient(GetAccessToken(), config);
            
            using (var mem = new MemoryStream(File.ReadAllBytes(fileToUpload)))
            {
                string filename = Path.GetFileName(fileToUpload);
                string megapath = GetFullFolderPath(folder);
                string megapathWithFile = Path.Combine(megapath, Path.GetFileName(Path.GetFileName(filename))).Replace("\\", "/");
                var updated = client.Files.UploadAsync(megapathWithFile, WriteMode.Overwrite.Instance, body: mem);
                await updated;
                return updated.Result;
            }
        }       

        public bool? CreateDropBoxFolder(string folderPath)
        {
            var metaData = CreateDropBoxFolderMetadata(folderPath.Trim(new Char[] { ' ', '/' }));
            return metaData.IsFolder;
        }

        public string GetRootFolder()
        {
            return Settings.Default.DropboxSaveToFolder;
        }
        public bool IsRootFolderSet()
        {
            if (string.IsNullOrWhiteSpace(Settings.Default.DropboxSaveToFolder))
                return false;
            else
                return true;
        }

        private FolderMetadata CreateDropBoxFolderMetadata(string folderPath)
        {

            // Prefex AitoeData
            //var fullpath = Path.Combine(Path.Combine("/", Settings.Default.DropboxSaveToFolder), folderPath.Trim(new Char[] { ' ', '/' })).Replace("\\", "/");
            string sFullPathWithCurrentApp = GetFullFolderPath(folderPath);
            DropboxClient client = new DropboxClient(GetAccessToken());
            Task<FolderMetadata> fm = client.Files.CreateFolderAsync(sFullPathWithCurrentApp);
            fm.Wait();
            return fm.Result;
        }
        public SearchResult SearchDropBoxFolder(DropboxClient client, string rootFolder, string folderToSearch)
        {
            Task<SearchResult> sr = client.Files.SearchAsync(rootFolder, folderToSearch);
            sr.Wait();
            return sr.Result;
        }
        public bool? IsLoginSuccessfull
        {
            get
            {
                return _Login.IsSuccessfully;
            }
        }

        public string CurrentApplication
        {
            get; set;
        }

        public DialogResult? ShowDialog()
        {
            return _Login.ShowDialog();
        }
        public DialogResult ShowDialog(IWin32Window owner)
        {
            return _Login.ShowDialog(owner);
        }
        public IntPtr? GetHandle()
        {
            //if (_Login == null)
            _Login = new DropboxLogin(GetClientId(), GetClientSecret());
            return _Login.Handle;
        }
        public string GetAccessToken()
        {
            var token = Settings.Default.DropBoxAccessToken;
            return token;
        }
        private string GetClientId()
        {
            //DropboxLogin login = new DropboxLogin("hybzb9wviyzgwy6", "m6sty81eei7m60r");
            var clientId = Settings.Default.DropBoxClientId;
            if (string.IsNullOrWhiteSpace(clientId) || clientId != "gh6ynhc8uh7wgt0")
            {
                Settings.Default.DropBoxClientId = "gh6ynhc8uh7wgt0";
                Settings.Default.Save();
                clientId = Settings.Default.DropBoxClientId;
            }
            return clientId;
        }
        private string GetClientSecret()
        {
            var clientSecret = Settings.Default.DropBoxClientSecret;
            if (string.IsNullOrWhiteSpace(clientSecret) || clientSecret != "92c61mclup55euk")
            {
                Settings.Default.DropBoxClientSecret = "92c61mclup55euk";
                Settings.Default.Save();
                clientSecret = Settings.Default.DropBoxClientSecret;
            }
            return clientSecret;
        }
        public Exception GetError()
        {
            return null;
        }        
        public bool? IsAccessTokenExists()
        {
            if (string.IsNullOrWhiteSpace(GetAccessToken()))
                return false;
            else
                return true;
        }
        public bool? SetAccessToken()
        {
            Settings.Default.DropBoxAccessToken = _Login.AccessToken.Value;
            Settings.Default.Save();
            return true;
        }
        public Window GetAppMainWindow()
        {
            var win = new Window();
            win.Owner = System.Windows.Application.Current.MainWindow;
            return win;
        }

    }
}
