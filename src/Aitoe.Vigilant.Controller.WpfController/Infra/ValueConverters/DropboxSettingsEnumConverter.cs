using Aitoe.Vigilant.Controller.WpfController.Infra.ViewModelStateEnums;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Aitoe.Vigilant.Controller.WpfController.Infra.ValueConverters
{
    public class DropboxSettingsEnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DropboxSettingsState enumValue = (DropboxSettingsState)value;
            object retObj;
            switch (parameter.ToString())
            {
                case "Content":
                    retObj = enumValue.Description();
                    break;
                case "BorderBrush":
                        retObj = enumValue.BackgroundColor();
                        break;
                case "Foreground":
                    retObj = enumValue.BackgroundColor();
                    break;
                case "ConfigureButtonText":
                    retObj = enumValue.ConfigureButtonText();
                    break;

                //ConfigureButtonText
                //case "IsEnabled":
                //    retObj = enumValue.ButtonIsEnabled();
                //    break;
                //case "Visibility":
                //    retObj = enumValue.ButtonVisibility();
                //    break;
                //case "SendMessageViewVisibility":
                //    retObj = enumValue.SendMessageViewVisibility();
                //    break;
                default:
                    retObj = "";
                    break;
            }
            return retObj;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var enumDesc = value as string;
            DropboxSettingsState val;

            if (Enum.TryParse(enumDesc, out val))
                return val;

            return DependencyProperty.UnsetValue;
        }
    }
}
