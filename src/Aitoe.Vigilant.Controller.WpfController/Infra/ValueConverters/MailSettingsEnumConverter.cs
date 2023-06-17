using Aitoe.Vigilant.Controller.WpfController.Infra.ViewModelStateEnums;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Aitoe.Vigilant.Controller.WpfController.Infra.ValueConverters
{
    public class MailSettingsEnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            MailSettingsStateEnum enumValue = (MailSettingsStateEnum)value;
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
                case "IsEnabled":
                    retObj = enumValue.ButtonIsEnabled();
                    break;
                case "Visibility":
                    retObj = enumValue.ButtonVisibility();
                    break;
                //SendMessageViewVisibility
                case "SendMessageViewVisibility":
                    retObj = enumValue.SendMessageViewVisibility();
                    break;
                default:
                    retObj = "";
                    break;
            }
            return retObj;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var enumDesc = value as string;
            MailSettingsStateEnum val;

            if (Enum.TryParse(enumDesc, out val))
                return val;

            return DependencyProperty.UnsetValue;
        }
    }

    public class MailSettingsEnumConverterMessage : IValueConverter
    {
        public object Convert(object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            MailSettingsStateEnum enumValue = (MailSettingsStateEnum)value;

            return enumValue.Description();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var enumDesc = value as string;
            MailSettingsStateEnum val;

            if (Enum.TryParse(enumDesc, out val))
                return val;

            return DependencyProperty.UnsetValue;
        }
    }
    public class MailSettingsEnumConverterBackgroundColor : IValueConverter
    {
        public object Convert(object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            MailSettingsStateEnum enumValue = (MailSettingsStateEnum)value;
            return enumValue.BackgroundColor();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var enumDesc = value as string;
            MailSettingsStateEnum val;

            if (Enum.TryParse(enumDesc, out val))
                return val;

            return DependencyProperty.UnsetValue;
        }
    }
}
