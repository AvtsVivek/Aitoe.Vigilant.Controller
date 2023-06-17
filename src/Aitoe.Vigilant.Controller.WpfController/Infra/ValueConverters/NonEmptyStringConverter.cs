using System;
using System.Globalization;
using System.Windows.Data;

namespace Aitoe.Vigilant.Controller.WpfController.Infra.ValueConverters
{
    public class NonEmptyStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool bIsNullOrEmpty;
            if (value == null)
            {
                bIsNullOrEmpty = true;
                return !bIsNullOrEmpty;
            }
            bIsNullOrEmpty = string.IsNullOrEmpty(value.ToString());
            return !bIsNullOrEmpty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }
    }
}
