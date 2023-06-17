using System;
using System.Windows.Forms;
using System.Windows;
using Dropbox.Api;
using Dropbox.Api.Files;
using System.Threading.Tasks;

namespace Aitoe.Vigilant.Controller.BL.ServiceInterfaces
{
    public interface IDropboxService
    {
        string CurrentApplication{ get; set; }
        bool? IsFolderExists(string folderPath);
        bool? DeleteDropBoxFolder(string folderPath);
        Task<FileMetadata> UploadFileToDropBox(string fileToUpload, string folder);
        bool? CreateDropBoxFolder(string folderPath);
        SearchResult SearchDropBoxFolder(DropboxClient client, string rootFolder, string folderToSearch);
        Window GetAppMainWindow();
        bool? IsLoginSuccessfull { get; }
        IntPtr? GetHandle();
        DialogResult? ShowDialog();
        bool? SetAccessToken();
        string GetAccessToken();
        bool? IsAccessTokenExists();
        string GetRootFolder();
        Exception GetError();
    }
}
