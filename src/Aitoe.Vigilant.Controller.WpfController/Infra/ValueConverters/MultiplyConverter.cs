using System;
using System.Globalization;
using System.Windows.Data;

namespace Aitoe.Vigilant.Controller.WpfController.Infra.ValueConverters
{
    public class MultiplyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double parameterResult, valueResult;
            double.TryParse(parameter.ToString(), out parameterResult);
            double.TryParse(value.ToString(), out valueResult);
            //double mult = (double)value * parameterResult;
            double mult = valueResult * parameterResult;
            return Math.Ceiling(mult);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
