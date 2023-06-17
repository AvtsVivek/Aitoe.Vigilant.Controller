using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace Aitoe.Vigilant.Controller.WpfController.Infra.ValueConverters
{
    public class PbVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility enumValue = (Visibility)value;
            Visibility retObj;
            if (enumValue == Visibility.Visible)
                retObj = Visibility.Collapsed;
            else
                retObj = Visibility.Visible;
            return retObj;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var enumDesc = value as string;
            Visibility val;

            if (Enum.TryParse(enumDesc, out val))
                return val;

            return DependencyProperty.UnsetValue;
        }
    }
}
