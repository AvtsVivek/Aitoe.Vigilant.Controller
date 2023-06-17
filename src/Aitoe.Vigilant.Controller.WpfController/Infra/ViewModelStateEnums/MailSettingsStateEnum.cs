using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Aitoe.Vigilant.Controller.WpfController.Infra.ViewModelStateEnums
{
    public enum MailSettingsStateEnum
    {
        SendersEmailNotConfigured,
        SendersPasswordsDoNotMatch,
        SendersEmailDetailsReadyToBeSaved,
        SendersEmailDetailsSaved,
        SendersEmailDetailsSaveFailed
    }

    public static class MailSettingsStateEnumExtension
    {
        public static string Description(this MailSettingsStateEnum enumValue)
        {
            string sDesc = string.Empty;
            switch (enumValue)
            {
                case MailSettingsStateEnum.SendersEmailNotConfigured:
                    sDesc = "Sender's Email Not Configured";
                    break;
                case MailSettingsStateEnum.SendersPasswordsDoNotMatch:
                    sDesc = "Sender's Passwords Do Not Match";
                    break;
                case MailSettingsStateEnum.SendersEmailDetailsReadyToBeSaved:
                    sDesc = "Sender's Settings Ready to be saved";
                    break;
                case MailSettingsStateEnum.SendersEmailDetailsSaved:
                    sDesc = "Sender's Settings Saved";
                    break;
                case MailSettingsStateEnum.SendersEmailDetailsSaveFailed:
                    sDesc = "Sender's Settings Save Failed";
                    break;
            }
            return sDesc;
        }
        //SendMessageViewVisibility
        public static Visibility SendMessageViewVisibility(this MailSettingsStateEnum enumValue)
        {
            Visibility buttonVisibility = Visibility.Hidden;
            switch (enumValue)
            {
                case MailSettingsStateEnum.SendersEmailNotConfigured:
                    buttonVisibility = Visibility.Hidden;
                    break;
                case MailSettingsStateEnum.SendersPasswordsDoNotMatch:
                    buttonVisibility = Visibility.Hidden;
                    break;
                case MailSettingsStateEnum.SendersEmailDetailsReadyToBeSaved:
                    buttonVisibility = Visibility.Hidden;
                    break;
                case MailSettingsStateEnum.SendersEmailDetailsSaved:
                    buttonVisibility = Visibility.Visible;
                    break;
                case MailSettingsStateEnum.SendersEmailDetailsSaveFailed:
                    buttonVisibility = Visibility.Visible;
                    break;
            }
            return buttonVisibility;
        }

        public static Visibility ButtonVisibility(this MailSettingsStateEnum enumValue)
        {
            Visibility buttonVisibility = Visibility.Hidden;
            switch (enumValue)
            {
                case MailSettingsStateEnum.SendersEmailNotConfigured:
                    buttonVisibility = Visibility.Hidden;
                    break;
                case MailSettingsStateEnum.SendersPasswordsDoNotMatch:
                    buttonVisibility = Visibility.Hidden;
                    break;
                case MailSettingsStateEnum.SendersEmailDetailsReadyToBeSaved:
                    buttonVisibility = Visibility.Hidden;
                    break;
                case MailSettingsStateEnum.SendersEmailDetailsSaved:
                    buttonVisibility = Visibility.Visible;
                    break;
                case MailSettingsStateEnum.SendersEmailDetailsSaveFailed:
                    buttonVisibility = Visibility.Visible;
                    break;
            }
            return buttonVisibility;
        }

        public static bool ButtonIsEnabled(this MailSettingsStateEnum enumValue)
        {
            bool bIsEnabled = false;
            switch (enumValue)
            {
                case MailSettingsStateEnum.SendersEmailNotConfigured:
                    bIsEnabled = false;
                    break;
                case MailSettingsStateEnum.SendersPasswordsDoNotMatch:
                    bIsEnabled = false;
                    break;
                case MailSettingsStateEnum.SendersEmailDetailsReadyToBeSaved:
                    bIsEnabled = true;
                    break;
                case MailSettingsStateEnum.SendersEmailDetailsSaved:
                    bIsEnabled = true;
                    break;
                case MailSettingsStateEnum.SendersEmailDetailsSaveFailed:
                    bIsEnabled = true;
                    break;
            }
            return bIsEnabled;
        }

        public static Brush BackgroundColor(this MailSettingsStateEnum enumValue)
        {
            Brush c = Brushes.Red;
            switch (enumValue)
            {
                case MailSettingsStateEnum.SendersEmailNotConfigured:
                    c = Brushes.Red;
                    break;
                case MailSettingsStateEnum.SendersPasswordsDoNotMatch:
                    c = Brushes.DarkRed;
                    break;
                case MailSettingsStateEnum.SendersEmailDetailsReadyToBeSaved:
                    c = Brushes.LightGreen;
                    break;
                case MailSettingsStateEnum.SendersEmailDetailsSaved:
                    c = Brushes.Green;
                    break;
                case MailSettingsStateEnum.SendersEmailDetailsSaveFailed:
                    c = Brushes.Red;
                    break;
            }
            return c;
        }
    }
}
