using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Aitoe.Vigilant.Controller.WpfController.Infra.ViewModelStateEnums
{
    public enum DropboxSettingsState
    {
        DropboxNotConfigured,
        DropboxConfigured,

        FolderCreatedSuccessfully,
        FolderDeletedSuccessfully,
        FileUploadedSuccessfully,
        FileUploadInprogress,
        FileDeletedSuccessfully,
        FolderExist,
        FolderDoesNotExist,
        FolderCreateFailure,
        FolderDeleteFailure,
        FileUploadFailure,
        FileDeleteFailure

    }

    public static class DropboxSettingsStateEnumExtension
    {
        public static string Description(this DropboxSettingsState enumValue)
        {
            string sDesc = string.Empty;
            switch (enumValue)
            {
                case DropboxSettingsState.DropboxNotConfigured:
                    sDesc = "Dropbox Not Configured";
                    break;
                case DropboxSettingsState.DropboxConfigured:
                    sDesc = "Dropbox is Configured";
                    break;
                case DropboxSettingsState.FolderCreatedSuccessfully:
                    sDesc = "Folder Created Successfully";
                    break;
                case DropboxSettingsState.FolderDeletedSuccessfully:
                    sDesc = "Folder Deleted Successfully";
                    break;
                case DropboxSettingsState.FolderExist:
                    sDesc = "Folder Exist";
                    break;
                case DropboxSettingsState.FolderDoesNotExist:
                    sDesc = "Folder Does Not Exist";
                    break;
                case DropboxSettingsState.FileUploadedSuccessfully:
                    sDesc = "File Uploaded Successfully";
                    break;
                case DropboxSettingsState.FileUploadInprogress:
                    sDesc = "File Uploaded in progress";
                    break;
                case DropboxSettingsState.FileDeletedSuccessfully:
                    sDesc = "File Deleted Succcessfully";
                    break;
            }
            return sDesc;
        }

        public static Brush BackgroundColor(this DropboxSettingsState enumValue)
        {
            Brush c = Brushes.Red;
            switch (enumValue)
            {
                case DropboxSettingsState.DropboxConfigured:
                    c = Brushes.White;
                    break;
                case DropboxSettingsState.DropboxNotConfigured:
                    c = Brushes.Red;
                    break;
                case DropboxSettingsState.FileDeletedSuccessfully:
                    c = Brushes.Green;
                    break;
                case DropboxSettingsState.FileUploadedSuccessfully:
                    c = Brushes.Green;
                    break;
                case DropboxSettingsState.FolderCreatedSuccessfully:
                    c = Brushes.Green;
                    break;
                case DropboxSettingsState.FolderDeletedSuccessfully:
                    c = Brushes.Green;
                    break;

                case DropboxSettingsState.FolderExist:
                    c = Brushes.LightGreen;
                    break;

                case DropboxSettingsState.FileUploadInprogress:
                    c = Brushes.Yellow;
                    break;

                case DropboxSettingsState.FolderDoesNotExist:
                    c = Brushes.Red;
                    break;

                case DropboxSettingsState.FileDeleteFailure:
                    c = Brushes.Red;
                    break;
                case DropboxSettingsState.FileUploadFailure:
                    c = Brushes.Red;
                    break;
                case DropboxSettingsState.FolderCreateFailure:
                    c = Brushes.Red;
                    break;
                case DropboxSettingsState.FolderDeleteFailure:
                    c = Brushes.Red;
                    break;
            }
            return c;
        }
        public static string ConfigureButtonText(this DropboxSettingsState enumValue)
        {
            var c = string.Empty;
            var s1 = "Configure Dropbox";
            var s2 = "Re-Configure Dropbox";
            switch (enumValue)
            {
                case DropboxSettingsState.DropboxConfigured:
                    c = s2;
                    break;
                case DropboxSettingsState.DropboxNotConfigured:
                    c = s1;
                    break;
                case DropboxSettingsState.FileDeletedSuccessfully:
                    c = s2;
                    break;
                case DropboxSettingsState.FileUploadedSuccessfully:
                    c = s2;
                    break;
                case DropboxSettingsState.FolderCreatedSuccessfully:
                    c = s2;
                    break;
                case DropboxSettingsState.FolderDeletedSuccessfully:
                    c = s2;
                    break;

                case DropboxSettingsState.FileUploadInprogress:
                    c = s2;
                    break;

                case DropboxSettingsState.FolderExist:
                    c = s2;
                    break;

                case DropboxSettingsState.FolderDoesNotExist:
                    c = s2;
                    break;

                case DropboxSettingsState.FileDeleteFailure:
                    c = s2;
                    break;
                case DropboxSettingsState.FileUploadFailure:
                    c = s2;
                    break;
                case DropboxSettingsState.FolderCreateFailure:
                    c = s2;
                    break;
                case DropboxSettingsState.FolderDeleteFailure:
                    c = s2;
                    break;
            }
            return c;
        }
    }
}
