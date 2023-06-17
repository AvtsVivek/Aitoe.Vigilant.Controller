using Aitoe.Vigilant.Controller.WpfController.Infra;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Windows;
using Ninject;
using Aitoe.Vigilant.Controller.WpfController.Infra.Messages;
using Aitoe.Vigilant.Controller.BL.ServiceInterfaces;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Aitoe.Vigilant.Controller.WpfController.ViewModel
{
    public class MultiControllerHomeViewModel : ViewModelBase, IPageViewModel
    {
        public string Name
        {
            get
            {
                return "Main _Grid View";
            }
        }
        private List<IPageViewModel> _viewMenuViewModels;
        private List<IPageViewModel> _optionsMenuViewModels;
        private List<IPageViewModel> _helpMenuViewModels;
        private IPageViewModel _currentPageViewModel;
        private MessageType<int> _messageType = new MessageType<int>();
        public RelayCommand<IPageViewModel> ChangeViewMenu { get; private set; }
        public RelayCommand<IPageViewModel> ChangeOptionsMenu { get; private set; }
        public RelayCommand<IPageViewModel> ChangeHelpMenu { get; private set; }
        public RelayCommand<object> MainWindowClosing { get; private set; }
        public RelayCommand<object> ToggleFullScreen { get; private set; }
        public RelayCommand<object> StackPanelMouseEnter { get; private set; }

        private bool isLoginSuccessfull;

        public bool LoginSuccessfull
        {
            get {
                return isLoginSuccessfull;
            }
            set
            {
                isLoginSuccessfull = value;
                RaisePropertyChanged();
            }
        }

        private readonly IAuthenticationService _AuthenticationService;

        public MultiControllerHomeViewModel(MultiControllerViewModel MCvm, VigilantProcessListViewModel VPLvm, 
            MailSettingsViewModel MSvm, DropboxSettingsViewModel DBSvm, PushbulletSettingsViewModel PBSvm, 
            GeneralHelpViewModel GHvm, LoginViewModel Lvm, IAuthenticationService authenticationService)
        {
            
            if (authenticationService == null)
                throw new ArgumentNullException("Authentication Service is null");

            _AuthenticationService = authenticationService;
            MainWindowState = WindowState.Maximized;
            MainWindowStyle = WindowStyle.SingleBorderWindow;
            ChangeViewMenu = new RelayCommand<IPageViewModel>(ChangeViewMenuExecute);
            ChangeOptionsMenu = new RelayCommand<IPageViewModel>(ChangeOptionsMenuExecute);
            ChangeHelpMenu = new RelayCommand<IPageViewModel>(ChangeHelpMenuExecute);
            MainWindowClosing = new RelayCommand<object>(MainWindowClosingExecute);
            ToggleFullScreen = new RelayCommand<object>(ToggleFullScreenExecute);
            StackPanelMouseEnter = new RelayCommand<object>(StackPanelMouseEnterExecute);
            //MainWindowLoaded
            // Add available pages
            ViewMenuViewModels.Add(Lvm);
            ViewMenuViewModels.Add(MCvm);
            ViewMenuViewModels.Add(new MultiControllerViewModelFullScreen());
            //ViewMenuViewModels.Add(VPLvm);
            
            OptionsMenuViewModels.Add(MSvm);
            OptionsMenuViewModels.Add(PBSvm);
            OptionsMenuViewModels.Add(DBSvm);
            HelpMenuViewModels.Add(GHvm);
            HelpMenuViewModels.Add(Lvm);
            MessengerInstance.Register<NotificationMessage<MessageType<FromMultiControllerVMToMultiControllerHomeVM>>>(this, NotifyMultiControllerVMRequests);
            MessengerInstance.Register<NotificationMessage<MessageType<ToMultiControllerHomeVM>>>(this, NotifyMailSettingsVMRequests);
            Initialize();
        }      

        private void Initialize()
        {
            if (CheckLogin())
            {
                LoginSuccessfull = true;
                //CurrentPageViewModel = ViewMenuViewModels.Where(a => a.GetType() == typeof(MultiControllerViewModel)).FirstOrDefault();
                CurrentPageViewModel = ViewMenuViewModels.Where(a => a.GetType() == typeof(LoginViewModel)).FirstOrDefault();
            }
            else
            {
                LoginSuccessfull = false;
                CurrentPageViewModel = ViewMenuViewModels.Where(a => a.GetType() == typeof(LoginViewModel)).FirstOrDefault();
            }
        }

        private bool CheckLogin()
        {
            var authDetails = _AuthenticationService.GetAuthDetails();
            var bSet = _AuthenticationService.IsLoginDetailSet();
            if (bSet.HasValue && bSet.Value)
                return true;
            else
                return false;
        }

        private void StackPanelMouseEnterExecute(object obj)
        {
            if (IsFullscreen)
            {
                MainMenuVisibility = Visibility.Visible;
                MainWindowStyle = WindowStyle.SingleBorderWindow;
                IsFullscreen = false;
            }
        }

        private bool IsFullscreen = false;
        private void ToggleFullScreenExecute(object obj)
        {
            if (IsFullscreen)
            {
                MainMenuVisibility = Visibility.Visible;
                MainWindowStyle = WindowStyle.SingleBorderWindow;
                IsFullscreen = false;
            }
            else
            {
                MainMenuVisibility = Visibility.Collapsed;
                MainWindowStyle = WindowStyle.None;
                MainWindowState = WindowState.Maximized;
                IsFullscreen = true;
            }
        }

        private Visibility _mainMenuVisibility;
        public Visibility MainMenuVisibility
        {
            get { return _mainMenuVisibility; }
            set
            {
                _mainMenuVisibility = value;
                RaisePropertyChanged();
            }
        }

        private WindowStyle _winStyle;
        public WindowStyle MainWindowStyle
        {
            get { return _winStyle; }
            set
            {
                _winStyle = value;
                RaisePropertyChanged();
            }
        }

        private WindowState _winState;

        public WindowState MainWindowState
        {
            get { return _winState; }
            set
            {
                _winState = value;
                RaisePropertyChanged();
            }
        }

        private void NotifyMailSettingsVMRequests(NotificationMessage<MessageType<ToMultiControllerHomeVM>> notificationMessage)
        {
            MessageType<ToMultiControllerHomeVM> changedValue = notificationMessage.Content;
            if (changedValue.Event == BroadCastEvents.ShowProcessGridView)
            {
                LoginSuccessfull = true;
                CurrentPageViewModel = ViewMenuViewModels.Where(a => a.GetType() == typeof(MultiControllerViewModel)).FirstOrDefault();
            }
            else if (changedValue.Event == BroadCastEvents.LicenseProblem)
            {
                LoginSuccessfull = false;
                CurrentPageViewModel = ViewMenuViewModels.Where(a => a.GetType() == typeof(LoginViewModel)).FirstOrDefault();
            }
            else if (changedValue.Event == BroadCastEvents.ShowHelpView)
            {
                CurrentPageViewModel = HelpMenuViewModels.Where(a => a.GetType() == typeof(GeneralHelpViewModel)).FirstOrDefault();
                //.FirstOrDefault(vm => vm == pageVM);
            }
            else if (changedValue.Event == BroadCastEvents.EnableMainMenu)
            {
                LoginSuccessfull = true;
            }
            else
                LoginSuccessfull = false;
        }

        //private void NotifyLoginVMRequests(NotificationMessage<MessageType<FromLoginVMToMultiControllerHomeVM>> notificationMessage)
        //{
        //    MessageType<FromLoginVMToMultiControllerHomeVM> changedValue = notificationMessage.Content;
        //    if (changedValue.Event == BroadCastEvents.EnableMainMenu)
        //    {
        //        LoginSuccessfull = true;
        //    }
        //    else
        //        LoginSuccessfull = false;
        //}

        private void NotifyMultiControllerVMRequests(NotificationMessage<MessageType<FromMultiControllerVMToMultiControllerHomeVM>> notificationMessage)
        {
            MessageType<FromMultiControllerVMToMultiControllerHomeVM> changedValue = notificationMessage.Content;
            if (changedValue.Event == BroadCastEvents.ShowEmailSettingsView)
            {
                CurrentPageViewModel = OptionsMenuViewModels[0];// The Email page
            }
            else if (changedValue.Event == BroadCastEvents.ShowPushbulletSettingsView)
            {
                CurrentPageViewModel = OptionsMenuViewModels[1];// The pushbullet page
            }
            else if (changedValue.Event == BroadCastEvents.ShowLoginView)
            {
                LoginSuccessfull = false;
                //CurrentPageViewModel = ViewMenuViewModels.Where(a => a.GetType() == typeof(MultiControllerViewModel)).FirstOrDefault();
                CurrentPageViewModel = ViewMenuViewModels.Where(a => a.GetType() == typeof(LoginViewModel)).FirstOrDefault();

                //CurrentPageViewModel = OptionsMenuViewModels[1];// The Login View Page page
            }
            else if (changedValue.Event == BroadCastEvents.ShowDropboxSettingsView)
            {
                CurrentPageViewModel = OptionsMenuViewModels[2];// The Dropbox page
            }
            else
            {
                // Golmal
            }
        }

        private void MainWindowClosingExecute(object param)
        {
            if (!LoginSuccessfull)
                return;
            _messageType.Message = "MainWindowClosing";
            _messageType.Event = BroadCastEvents.MainWindowClosing;
            MessengerInstance.Send(new NotificationMessage<MessageType<int>>(_messageType, _messageType.Message));
        }

        private void ChangeViewMenuExecute(IPageViewModel pageVM)
        {
            if (!ViewMenuViewModels.Contains(pageVM))
                ViewMenuViewModels.Add(pageVM);

            if (pageVM is MultiControllerViewModelFullScreen)
            {
                //ToggleFullScreenExecute(null);
                // This is a hack. Without this the Full Screen menu is looking different than 
                // the other menu.
                ToggleFullScreen.Execute(null);
                return;
            }

            if (pageVM is MultiControllerViewModel)
            {
                _messageType.Message = "LoadGridView";
                _messageType.Event = BroadCastEvents.LoadGridView;
                MessengerInstance.Send(new NotificationMessage<MessageType<int>>(_messageType, _messageType.Message));
            }

            CurrentPageViewModel = ViewMenuViewModels
                .FirstOrDefault(vm => vm == pageVM);
        }

        private void ChangeOptionsMenuExecute(IPageViewModel pageVM)
        {
            if (!OptionsMenuViewModels.Contains(pageVM))
                OptionsMenuViewModels.Add(pageVM);

            CurrentPageViewModel = OptionsMenuViewModels
                .FirstOrDefault(vm => vm == pageVM);
        }

        private void ChangeHelpMenuExecute(IPageViewModel pageVM)
        {
            if (!HelpMenuViewModels.Contains(pageVM))
                HelpMenuViewModels.Add(pageVM);

            CurrentPageViewModel = HelpMenuViewModels
                .FirstOrDefault(vm => vm == pageVM);
        }

        public List<IPageViewModel> ViewMenuViewModels
        {
            get
            {
                if (_viewMenuViewModels == null)
                    _viewMenuViewModels = new List<IPageViewModel>();

                return _viewMenuViewModels;
            }
        }

        public List<IPageViewModel> OptionsMenuViewModels
        {
            get
            {
                if (_optionsMenuViewModels == null)
                    _optionsMenuViewModels = new List<IPageViewModel>();

                return _optionsMenuViewModels;
            }
        }

        public List<IPageViewModel> HelpMenuViewModels
        {
            get
            {
                if (_helpMenuViewModels == null)
                    _helpMenuViewModels = new List<IPageViewModel>();

                return _helpMenuViewModels;
            }
        }

        public IPageViewModel CurrentPageViewModel
        {
            get
            {
                return _currentPageViewModel;
            }
            set
            {
                if (_currentPageViewModel != value)
                {
                    _currentPageViewModel = value;
                    RaisePropertyChanged(() => this.CurrentPageViewModel);
                }
            }
        }
    }
}
