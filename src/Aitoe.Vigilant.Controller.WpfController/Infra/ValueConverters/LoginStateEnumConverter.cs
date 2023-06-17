using Aitoe.Vigilant.Controller.WpfController.Infra.ViewModelStateEnums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Aitoe.Vigilant.Controller.WpfController.Infra.ValueConverters
{
    public class LoginStateEnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            LoginState enumValue = (LoginState)value;
            object retObj;
            switch (parameter.ToString())
            {
                case "Content":
                    retObj = enumValue.Description();
                    break;
                case "BackgroundColor":
                    retObj = enumValue.BackgroundColor();
                    break;
                case "LoginTextBoxesEnableDisable":
                    retObj = enumValue.LoginTextBoxesEnableDisable();
                    break;
                default:
                    retObj = "";
                    break;
            }
            return retObj;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
