using System.Windows.Media;

namespace Aitoe.Vigilant.Controller.WpfController.Infra.ViewModelStateEnums
{
    public enum LoginState
    {
        InitialMessage,
        ClickLoginToContinue,
        Wait,
        LoginSuccessful,
        LicenseActivated,
        OrderIncomplete,
        LoginFailure,
        OrderNoIncorrect,
        FailureSavingLoginDetails,
        OtherFailure,
        LoginDetailsSet,
        LogoutSuccessful,
        LogoutUnsuccessful,
        LoginAgain,
        LicenseProblem
    }
    public static class LoginStateEnumExtensions
    {
        public static string Description(this LoginState enumValue)
        {
            string sDesc = string.Empty;
            switch (enumValue)
            {
                case LoginState.InitialMessage:
                    sDesc = "Please Enter your details and press login...";
                    break;
                case LoginState.ClickLoginToContinue:
                    sDesc = "Click login to continue...";
                    break;
                case LoginState.Wait:
                    sDesc = "Please wait...";
                    break;
                case LoginState.LoginSuccessful:
                    sDesc = "Login Successful";
                    break;
                case LoginState.LicenseActivated:
                    sDesc = "License Activated";
                    break;
                case LoginState.OrderIncomplete:
                    sDesc = "Order is incomplete";
                    break;
                case LoginState.LoginFailure:
                    sDesc = "Login Failure";
                    break;
                case LoginState.OrderNoIncorrect:
                    sDesc = "Order number is incorrect";
                    break;
                case LoginState.OtherFailure:
                    sDesc = "Request could not be processed, please see logs for more details";
                    break;
                case LoginState.LoginDetailsSet:
                    sDesc = "Login details set. Click anywhere to continue...";
                    break;
                case LoginState.LogoutSuccessful:
                    sDesc = "Logout successful";
                    break;
                case LoginState.LogoutUnsuccessful:
                    sDesc = "Logout UNsuccessful. Please try again later.";
                    break;
                //LoginAgain
                case LoginState.LoginAgain:
                    sDesc = "Please login again.";
                    break;
                case LoginState.LicenseProblem:
                    sDesc = "License Problem. Please relogin.";
                    break;
            }
            return sDesc;
        }

        public static Brush BackgroundColor(this LoginState enumValue)
        {
            Brush c = Brushes.Black;
            switch (enumValue)
            {
                case LoginState.InitialMessage:
                    c = Brushes.Black;
                    break;
                case LoginState.ClickLoginToContinue:
                    c = Brushes.Black;
                    break;
                case LoginState.Wait:
                    c = Brushes.Gray;
                    break;
                case LoginState.LoginSuccessful:
                    c = Brushes.DarkGreen;
                    break;
                case LoginState.LicenseActivated:
                    c = Brushes.Red;
                    break;
                case LoginState.OrderIncomplete:
                    c = Brushes.Red;
                    break;
                case LoginState.LoginFailure:
                    c = Brushes.Red; 
                    break;
                case LoginState.OrderNoIncorrect:
                    c = Brushes.Red; 
                    break;
                case LoginState.OtherFailure:
                    c = Brushes.Red; 
                    break;
                case LoginState.LoginDetailsSet:
                    c = Brushes.Green;
                    break;
                case LoginState.LogoutSuccessful:
                    c = Brushes.Green;
                    break;
                case LoginState.LogoutUnsuccessful:
                    c = Brushes.Red;
                    break;
                case LoginState.LoginAgain:
                    c = Brushes.Red;
                    break;
                case LoginState.LicenseProblem:
                    c = Brushes.Red;
                    break;
            }
            return c;
        }

        public static bool LoginTextBoxesEnableDisable(this LoginState enumValue)
        {
            bool c = true;
            switch (enumValue)
            {
                case LoginState.InitialMessage:
                    c = true;
                    break;
                case LoginState.ClickLoginToContinue:
                    c = true;
                    break;
                case LoginState.Wait:
                    c = false;
                    break;
                case LoginState.LoginSuccessful:
                    c = false;
                    break;
                case LoginState.LicenseActivated:
                    c = false;
                    break;
                case LoginState.OrderIncomplete:
                    c = true;
                    break;
                case LoginState.LoginFailure:
                    c = true;
                    break;
                case LoginState.OrderNoIncorrect:
                    c = true;
                    break;
                case LoginState.OtherFailure:
                    c = true;
                    break;
                case LoginState.LoginDetailsSet:
                    c = false;
                    break;

            }
            return c;
        }

    }
}
