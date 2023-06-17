using Aitoe.Vigilant.Controller.BL.Entites;
using Aitoe.Vigilant.Controller.BL.ServiceInterfaces;
using Aitoe.Vigilant.Controller.WpfController.Infra;
using Aitoe.Vigilant.Controller.WpfController.Infra.Messages;
using Aitoe.Vigilant.Controller.WpfController.Infra.ViewModelStateEnums;
using AutoMapper;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Aitoe.Vigilant.Controller.WpfController.ViewModel
{
    public class LoginViewModel : AitoeVigilantViewModelBase, IPageViewModel
    {
        public string Name
        {
            get
            {
                return "Login";
            }
        }

        private LoginState currentLoginState;

        public LoginState CurrentLoginState
        {
            get { return currentLoginState; }
            set
            {
                currentLoginState = value;
                EnableDisableMainMenu();
                RaisePropertyChanged();
            }
        }

        private void EnableDisableMainMenu()
        {
            if (CurrentLoginState == LoginState.LoginSuccessful ||
                CurrentLoginState == LoginState.LoginDetailsSet)

                _messageTypeToHomeVM.Event = BroadCastEvents.EnableMainMenu;
            else
                _messageTypeToHomeVM.Event = BroadCastEvents.DisableMainMenu;

            _messageTypeToHomeVM.Value = new ToMultiControllerHomeVM("MessageForMainMenu");
            _messageTypeToHomeVM.Message = "";
            MessengerInstance.Send<NotificationMessage<MessageType<ToMultiControllerHomeVM>>, MultiControllerHomeViewModel>(new NotificationMessage<MessageType<ToMultiControllerHomeVM>>(_messageTypeToHomeVM, _messageTypeToHomeVM.Message));
        }

        private string userId;

        public string UserId
        {
            get { return userId; }
            set
            {
                userId = value;
                RaisePropertyChanged();
                Login.RaiseCanExecuteChanged();
            }
        }

        private string password;

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                RaisePropertyChanged();
                Login.RaiseCanExecuteChanged();
            }
        }

        private string orderId;

        public string OrderId
        {
            get { return orderId; }
            set
            {
                orderId = value;
                RaisePropertyChanged();
                Login.RaiseCanExecuteChanged();
            }
        }

        public DateTime LicenseCheckDate { get; set; }

        public RelayCommand<string> Login { get; private set; }
        public RelayCommand<string> Logout { get; private set; }
        public RelayCommand<object> LoginViewLoaded { get; private set; }
        public RelayCommand<object> ViewClicked { get; private set; }
        public Uri SecurityKeyImageSource { get; set; }
        public Uri HelpImageSource { get; set; }
        private IAuthenticationService _AuthenticationService;
        private IMapper _InternalMapper;
        public LoginViewModel(IAuthenticationService authenticationService,
            IMapper mapper)
        {
            LicenseCheckDate = DateTime.Now;
            Login = new RelayCommand<string>(LoginExecute, CanLoginExecute);
            Logout = new RelayCommand<string>(LogoutExecute, CanLogoutExecute);
            LoginViewLoaded = new RelayCommand<object>(LoginViewLoadedExecute);
            ViewClicked = new RelayCommand<object>(ViewClickedExecute);
            SecurityKeyImageSource = new Uri("/Icons/Key-PNG.png", UriKind.Relative);
            HelpImageSource = new Uri("/Icons/Help-button.png", UriKind.Relative);

            if (authenticationService == null)
                throw new ArgumentNullException("Authentication Service is null");

            _AuthenticationService = authenticationService;

            if (mapper == null)
                throw new ArgumentNullException("Auto mapper is null");

            _InternalMapper = mapper;

            Task checkProcStatusTask = new Task(() => TempTrial());
            checkProcStatusTask.Start();
            CurrentLoginState = LoginState.InitialMessage;
        }

        private async void TempTrial()
        {
            bool? isDone = false;

            while (true)
            {
                // When debugging or testing make the sleep time as 
                // 3 secs or 3000 milli secs.
                Thread.Sleep(3*60*60*1000);

                var authDetails = _AuthenticationService.GetAuthDetails();

                if (CurrentLoginState == LoginState.LicenseProblem)
                    continue;

                if (CurrentLoginState == LoginState.Wait)
                    continue;

                if (authDetails == null)
                    continue;

                if (string.IsNullOrEmpty(authDetails.OrderId))
                    continue;

                if (string.IsNullOrEmpty(authDetails.UserId))
                    continue;

                // When debugging or testing, you can change the 
                // DateTime.Now.AddDays(-5) to DateTime.Now.AddMinutes(-1)
                if (DateTime.Now.AddDays(-5) > authDetails.LicenseCheckDate)
                {
                    var temp = _AuthenticationService.ValidateUserForOrder(authDetails);
                    //var t = _AuthenticationService.GetStatusCodeAsync(authDetails);
                    //CurrentLoginState = LoginState.Wait;
                    await temp;
                    if (temp.Result.HasValue)
                    {
                        if (temp.Result.Value == true)
                        {
                            var noOfCamsTask = _AuthenticationService.ExtractNoOfCameras(authDetails);
                            //CurrentLoginState = LoginState.Wait;
                            await noOfCamsTask;

                            if (noOfCamsTask.Result.HasValue)
                            {
                                if (noOfCamsTask.Result.Value == 0)
                                {
                                    var UnsetActivatedFlag = _AuthenticationService.UnsetActivatedFlag(authDetails);
                                    await UnsetActivatedFlag;

                                    if (UnsetActivatedFlag.Result.HasValue)
                                    {
                                        if (UnsetActivatedFlag.Result.Value)
                                        {
                                            //UserId = string.Empty;
                                            //OrderId = string.Empty;
                                            //Password = string.Empty;
                                        }
                                        //else
                                            //CurrentLoginState = LoginState.LogoutUnsuccessful;

                                    }
                                    Login.RaiseCanExecuteChanged();
                                    Logout.RaiseCanExecuteChanged();
                                    
                                    _messageTypeToHomeVM.Value = new ToMultiControllerHomeVM("LoginView");
                                    _messageTypeToHomeVM.Message = "Login View to be shown.";
                                    _messageTypeToHomeVM.Event = BroadCastEvents.LicenseProblem;
                                    MessengerInstance.Send(new NotificationMessage<MessageType<ToMultiControllerHomeVM>>(_messageTypeToHomeVM, _messageTypeToHomeVM.Message));



                                    //isDone = _AuthenticationService.PersistAuthDetails(authDetails);
                                    CurrentLoginState = LoginState.LicenseProblem;
                                }
                            }
                        }
                        else
                        {
                            CurrentLoginState = LoginState.OrderNoIncorrect;
                        }
                    }
                }
            }
        }

        private bool bHelpClicked = false;

        private void ViewClickedExecute(object parm)
        {
            if (parm.ToString() == "GoToMultiCamera")
            {
                if (bHelpClicked)
                {
                    bHelpClicked = false;
                    return;
                }

                if (CurrentLoginState != LoginState.LoginDetailsSet)
                    return;
                _messageTypeToHomeVM.Value = new ToMultiControllerHomeVM("ProcessGridView");
                _messageTypeToHomeVM.Message = "Process Grid View to be shown";
                _messageTypeToHomeVM.Event = BroadCastEvents.ShowProcessGridView;
                MessengerInstance.Send(new NotificationMessage<MessageType<ToMultiControllerHomeVM>>(_messageTypeToHomeVM, _messageTypeToHomeVM.Message));
            }
            else if (parm.ToString() == "helpPlease")
            {
                bHelpClicked = true;
                _messageTypeToHomeVM.Value = new ToMultiControllerHomeVM("ShowHelpView");
                _messageTypeToHomeVM.Message = "Help should be shown";
                _messageTypeToHomeVM.Event = BroadCastEvents.ShowHelpView;
                MessengerInstance.Send(new NotificationMessage<MessageType<ToMultiControllerHomeVM>>(_messageTypeToHomeVM, _messageTypeToHomeVM.Message));
            }
        }

        //private bool bAppJustStarted = false;

        private void LoginViewLoadedExecute(object obj)
        {
            var authDetails = _AuthenticationService.GetAuthDetails();
            _InternalMapper.Map(authDetails, this);

            Login.RaiseCanExecuteChanged();
            Logout.RaiseCanExecuteChanged();

            if (CurrentLoginState == LoginState.LicenseProblem)
                return;

            var bSet = _AuthenticationService.IsLoginDetailSet();
            if (bSet.HasValue && bSet.Value)
            {
                CurrentLoginState = LoginState.LoginDetailsSet;
            }
            else
                CurrentLoginState = LoginState.InitialMessage;

            Login.RaiseCanExecuteChanged();
            Logout.RaiseCanExecuteChanged();
        }

        private bool CanLogoutExecute(string arg)
        {
            if (!string.IsNullOrEmpty(UserId) &&
                !string.IsNullOrEmpty(OrderId) &&
                !string.IsNullOrEmpty(Password)
            )
            {
                var bSet = _AuthenticationService.IsLoginDetailSet();
                if (bSet.HasValue && bSet.Value)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
        private async void LogoutExecute(string obj)
        {
            var authDetails = _AuthenticationService.GetAuthDetails();
            var UnsetActivatedFlag = _AuthenticationService.UnsetActivatedFlag(authDetails);
            CurrentLoginState = LoginState.Wait;
            await UnsetActivatedFlag;

            if (UnsetActivatedFlag.Result.HasValue)
            {
                if (UnsetActivatedFlag.Result.Value)
                {
                    CurrentLoginState = LoginState.LogoutSuccessful;
                    UserId = string.Empty;
                    OrderId = string.Empty;
                    Password = string.Empty;
                    
                }
                else
                    CurrentLoginState = LoginState.LogoutUnsuccessful;
                
            }
            Login.RaiseCanExecuteChanged();
            Logout.RaiseCanExecuteChanged();
        }

        private bool CanLoginExecute(string arg)
        {
            if (!string.IsNullOrEmpty(UserId) &&
                !string.IsNullOrEmpty(OrderId) &&
                !string.IsNullOrEmpty(Password)
                )
            {
                if (CurrentLoginState == LoginState.LoginSuccessful)
                    return false;
                else if (CurrentLoginState == LoginState.LoginDetailsSet)
                    return false;
                else
                    return true;
            }
            else
                return false;
        }
        private MessageType<ToMultiControllerHomeVM> _messageTypeToHomeVM = new MessageType<ToMultiControllerHomeVM>();

        private async void LoginExecute(string obj)
        {
            System.IO.File.Delete("LogFile.txt");
            var authDetails = _InternalMapper.Map<IAuthDetails>(this);
            authDetails.LicenseCheckDate = DateTime.Now;
            var loginTask = _AuthenticationService.Login(authDetails);
            CurrentLoginState = LoginState.Wait;
            await loginTask;

            bool? isDone = false;

            if (loginTask.Result.HasValue)
                if (loginTask.Result.Value)
                    CurrentLoginState = LoginState.LoginSuccessful;
                else
                    CurrentLoginState = LoginState.LoginFailure;
            else
                CurrentLoginState = LoginState.OtherFailure;

            if (CurrentLoginState == LoginState.LoginSuccessful)
            {
                var temp = _AuthenticationService.ValidateUserForOrder(authDetails);

                CurrentLoginState = LoginState.Wait;

                await temp;

                if (temp.Result.HasValue)
                {
                    if (temp.Result.Value == true)
                    {
                        CurrentLoginState = LoginState.LoginSuccessful;
                    }
                    else
                    {
                        CurrentLoginState = LoginState.OrderNoIncorrect;
                    }
                }
            }


            if (CurrentLoginState == LoginState.LoginSuccessful)
            {
                var activatedFlag = _AuthenticationService.GetActivatedFlag(authDetails);
                CurrentLoginState = LoginState.Wait;
                await activatedFlag;

                if (activatedFlag.Result.HasValue)
                {
                    if (!activatedFlag.Result.Value)
                    {
                        // If it is not activaed, then go ahead and then extract the cameras.
                        var noOfCamsTask = _AuthenticationService.ExtractNoOfCameras(authDetails);

                        CurrentLoginState = LoginState.Wait;

                        await noOfCamsTask;

                        if (noOfCamsTask.Result.HasValue)
                        {
                            if (noOfCamsTask.Result.Value == -2)
                            {
                                CurrentLoginState = LoginState.OrderIncomplete;
                            }
                            else if (noOfCamsTask.Result.Value == -3)
                            {
                                CurrentLoginState = LoginState.LoginFailure;
                            }
                            else if (noOfCamsTask.Result.Value == -4)
                            {
                                CurrentLoginState = LoginState.OrderNoIncorrect;
                            }
                            else if (noOfCamsTask.Result.Value == -5)
                            {
                                CurrentLoginState = LoginState.OrderIncomplete;
                            }
                            else if (noOfCamsTask.Result.Value > 0)
                            {
                                CurrentLoginState = LoginState.LoginSuccessful;

                                var setActivatedFlag = _AuthenticationService.SetActivatedFlag(authDetails);

                                CurrentLoginState = LoginState.Wait;

                                await setActivatedFlag;

                                if (setActivatedFlag.Result.HasValue)
                                {
                                    if (setActivatedFlag.Result.Value)
                                    {
                                        _messageTypeToHomeVM.Value = new ToMultiControllerHomeVM("ProcessGridView");
                                        _messageTypeToHomeVM.Message = "Process Grid View to be shown";
                                        _messageTypeToHomeVM.Event = BroadCastEvents.ShowProcessGridView;
                                        MessengerInstance.Send(new NotificationMessage<MessageType<ToMultiControllerHomeVM>>(_messageTypeToHomeVM, _messageTypeToHomeVM.Message));
                                        isDone = _AuthenticationService.PersistAuthDetails(authDetails);
                                        CurrentLoginState = LoginState.LoginSuccessful;
                                    }
                                }
                            }
                        }
                        else
                        {
                            CurrentLoginState = LoginState.OtherFailure;
                        }

                        if (isDone.HasValue && !isDone.Value)
                            CurrentLoginState = LoginState.FailureSavingLoginDetails;
                        else if (!isDone.HasValue)
                            CurrentLoginState = LoginState.OtherFailure;
                    }
                    else
                    {
                        CurrentLoginState = LoginState.LicenseActivated;
                    }
                }
            }
            else
                CurrentLoginState = LoginState.LoginFailure;

            Login.RaiseCanExecuteChanged();
            Logout.RaiseCanExecuteChanged();
        }
    }
}
