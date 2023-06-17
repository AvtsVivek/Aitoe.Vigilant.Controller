using Aitoe.Vigilant.Controller.BL.ServiceInterfaces;
using Aitoe.Vigilant.Controller.WpfController.Infra;
using Aitoe.Vigilant.Controller.WpfController.Infra.Messages;
using Aitoe.Vigilant.Controller.WpfController.Properties;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Aitoe.Vigilant.Controller.WpfController.ViewModel
{
    public class MultiControllerViewModel : AitoeVigilantViewModelBase, IPageViewModel
    {
        private Uri _pinImageSource = new Uri("/Icons/PinOn.png", UriKind.Relative);

        private string _showhideLeftMenu = "Show";

        private bool _isGridOn = false;

        private bool _isGridHeaderOn = false;

        private int _cellHeight;

        private int _cellWidth;

        private double _cellAspectRatio;

        private int _rows;

        private MessageType<FromMultiControllerVMToVigilantGridVM> _messageType = new MessageType<FromMultiControllerVMToVigilantGridVM>();
        private MessageType<FromMultiControllerVMToMultiControllerHomeVM> _messageTypeToHomeVM = new MessageType<FromMultiControllerVMToMultiControllerHomeVM>();
        private int _columns;

        private bool IsAnimationRunning = false;

        private bool isConfigLoaded = false;

        public string BindingKey
        {
            get
            {
                return "G";
            }
        }

        public string Name
        {
            get
            {
                return "Camera _Grid View";
            }
        }      

        public Uri PinImageSource
        {
            get
            {
                return this._pinImageSource;
            }
            set
            {
                bool flag = this._pinImageSource.OriginalString == value.OriginalString;
                if (!flag)
                {
                    _pinImageSource = value;
                    RaisePropertyChanged(() => PinImageSource);
                }
            }
        }

        public string ShowHideLeftMenu
        {
            get
            {
                return this._showhideLeftMenu;
            }
            set
            {
                if (!IsAnimationRunning)
                {
                    bool flag = _showhideLeftMenu == "Show" && _pinImageSource.ToString().EndsWith("PinOn.png");
                    if (!flag)
                    {
                        if (this._showhideLeftMenu == value)
                            return;
                        
                        _showhideLeftMenu = value;
                        RaisePropertyChanged(() => ShowHideLeftMenu);
                    }
                }
            }
        }

        public bool IsGridOn
        {
            get
            {
                return _isGridOn;
            }
            set
            {
                if (_isGridOn == value)
                    return;

                _isGridOn = value;
                RaisePropertyChanged(() => IsGridOn);
                ShowHideHeaders.RaiseCanExecuteChanged();
            }
        }
        public bool IsGridHeaderOn
        {
            get
            {
                return _isGridHeaderOn;
            }
            set
            {
                if (_isGridHeaderOn == value)
                    return;
                
                _isGridHeaderOn = value;
                RaisePropertyChanged(() => IsGridHeaderOn);
            }
        }
        public int CellHeight
        {
            get
            {
                return _cellHeight;
            }
            set
            {
                if (value < 150)
                    return;

                bool flag = _cellHeight == value;
                if (!flag)
                {
                    _cellHeight = value;
                    CellWidth = (int)Math.Ceiling(_cellAspectRatio * CellHeight);
                    _messageType.Value = new FromMultiControllerVMToVigilantGridVM(Rows, Columns, value, CellWidth);
                    _messageType.Message = "CellHeight Changed";
                    _messageType.Event = BroadCastEvents.CellHeightChanged;
                    MessengerInstance.Send(new NotificationMessage<MessageType<FromMultiControllerVMToVigilantGridVM>>(_messageType, _messageType.Message));
                    RaisePropertyChanged(() => CellHeight);
                }
            }
        }
        public int CellWidth
        {
            get
            {
                return _cellWidth;
            }
            set
            { 
                bool flag = _cellWidth == value;
                if (!flag)
                {
                    _cellWidth = value;
                    RaisePropertyChanged(() => CellWidth);
                }
            }
        }
        public int Rows
        {
            get
            {
                return this._rows;
            }
            set
            {
                bool flag = this._rows == value;
                if (!flag)
                {
                    _rows = value;
                    _messageType.Value = new FromMultiControllerVMToVigilantGridVM(value, Columns);
                    _messageType.Message = "Rows Changed";
                    _messageType.Event = BroadCastEvents.RowSizeChanged;
                    MessengerInstance.Send(new NotificationMessage<MessageType<FromMultiControllerVMToVigilantGridVM>>(_messageType, _messageType.Message));
                    IsGridOn = true;
                    RaisePropertyChanged(() => Rows);
                }
            }
        }
        public int Columns
        {
            get
            {
                return _columns;
            }
            set
            {
                bool flag = _columns == value;
                if (!flag)
                {
                    _columns = value;
                    _messageType.Value = new FromMultiControllerVMToVigilantGridVM(Rows, value);
                    _messageType.Message = "Columns Changed";
                    _messageType.Event = BroadCastEvents.ColumnSizeChanged;
                    MessengerInstance.Send(new NotificationMessage<MessageType<FromMultiControllerVMToVigilantGridVM>>(_messageType, _messageType.Message));
                    IsGridOn = true;
                    RaisePropertyChanged(() => Columns);
                }
            }
        }
        public RelayCommand<object> PinSet
        {
            get;
            private set;
        }

        public RelayCommand<object> BorderMouseLeave
        {
            get;
            private set;
        }

        public RelayCommand<object> ViewLoaded
        {
            get;
            private set;
        }
        public RelayCommand<object> BorderMouseEnter
        {
            get;
            private set;
        }

        public RelayCommand<object> ConfigureView
        {
            get;
            private set;
        }

        public RelayCommand<object> StartAllCameras
        {
            get;
            private set;
        }

        public RelayCommand<object> StopAllCameras
        {
            get;
            private set;
        }

        public RelayCommand<object> ShowHideHeaders
        {
            get;
            private set;
        }

        public RelayCommand<object> StoryBoardCompleted
        {
            get;
            private set;
        }

        public RelayCommand<object> StoryboardCurrentTimeInvalidated
        {
            get;
            private set;
        }

        public VigilantGridViewModel VigilantGridVM
        {
            get;
        }

        public RelayCommand<object> PushbulletConfiguration
        {
            get;
            private set;
        }

        public RelayCommand<object> EmailConfiguration
        {
            get;
            private set;
        }

        public RelayCommand<object> DropboxConfiguration
        {
            get;
            private set;
        }

        private Visibility _pushbulletConfigurationVisibility;
        public Visibility PushbulletConfigurationVisibility
        {
            get
            {
                return _pushbulletConfigurationVisibility;
            }

            private set
            {
                _pushbulletConfigurationVisibility = value;
                RaisePropertyChanged();
            }
        }

        private bool isPushbulletConfigured;

        public bool IsPushbulletConfigured
        {
            get { return isPushbulletConfigured; }
            set { isPushbulletConfigured = value;
                RaisePropertyChanged();
            }
        }

        private bool isEmailConfigured;

        public bool IsEmailConfigured
        {
            get { return isEmailConfigured; }
            set
            {
                isEmailConfigured = value;
                RaisePropertyChanged();
            }
        }

        private bool isDropboxConfigured;
        public bool IsDropboxConfigured
        {
            get { return isDropboxConfigured; }
            set
            {
                isDropboxConfigured = value;
                RaisePropertyChanged();
            }
        }
        
        private readonly IEmailService _EmailService;
        private readonly IDropboxService _DropboxService;
        private readonly IAuthenticationService _AuthenticationService;

        private int camerasRunning;
        public int CamerasRunning
        {
            get {
                return camerasRunning;
            }
            set {
                camerasRunning = value;
                RaisePropertyChanged();
            }
        }

        private int camerasLicensed;

        public int CamerasLicensed
        {
            get { return camerasLicensed; }
            set
            {
                camerasLicensed = value;
                RaisePropertyChanged();
            }
        }

        public Uri HomeBackgroundOpaque { get; set; }

        public MultiControllerViewModel(VigilantGridViewModel Vgvm, 
            IEmailService emailService, 
            IDropboxService dropboxService,
            IAuthenticationService authService            
            )
        {
            if (emailService == null)
                throw new ArgumentNullException("Email service is null");
            _EmailService = emailService;

            if (dropboxService == null)
                throw new ArgumentNullException("Dropbox service is null");
            _DropboxService = dropboxService;


            if (authService == null)
                throw new ArgumentNullException("Auth service is null");
            _AuthenticationService = authService;
            

            if (Vgvm == null)
            {
                throw new ArgumentNullException("VigilantGridViewModel is null");
            }

            VigilantGridVM = Vgvm;

            VigilantGridVM.PropertyChanged += VigilantGridVM_PropertyChanged;

            SetPushbulletConfigurationVisibility();
            SetEmailConfigurationVisibility();
            SetDropboxConfigurationVisibility();

            HomeBackgroundOpaque = new Uri("/Icons/HomeBackgroundOpaque.png", UriKind.Relative);

            //EmailConfigurationVisibility = Visibility.Collapsed;
            PushbulletConfigurationVisibility = Visibility.Collapsed;
            _cellAspectRatio = Settings.Default.CellAspectRatio;
            PinSet = new RelayCommand<object>(PinSetExecute);
            BorderMouseLeave = new RelayCommand<object>(BorderMouseLeaveExecute);
            BorderMouseEnter = new RelayCommand<object>(BorderMouseEnterExecute);
            ViewLoaded = new RelayCommand<object>(ViewLoadedExecute);
            ConfigureView = new RelayCommand<object>(ConfigureViewExecute);
            StartAllCameras = new RelayCommand<object>(StartAllCamerasExecute, CanStartAllCamerasExecute);
            StopAllCameras = new RelayCommand<object>(StopAllCamerasExecute, CanStopAllCamerasExecute);
            ShowHideHeaders = new RelayCommand<object>(ShowHideHeadersExecute);
            StoryBoardCompleted = new RelayCommand<object>(StoryBoardCompletedExecuted);
            StoryboardCurrentTimeInvalidated = new RelayCommand<object>(StoryboardCurrentTimeInvalidatedExecuted);
            PushbulletConfiguration = new RelayCommand<object>(PushbulletConfigurationExecute);
            EmailConfiguration = new RelayCommand<object>(EmailConfigurationExecute);
            DropboxConfiguration = new RelayCommand<object>(DropboxConfigurationExecute);
            MessengerInstance.Register<NotificationMessage<MessageType<int>>>(this, NotifyMe);
        }

        private void VigilantGridVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CamerasRunning")
            {
                CamerasRunning = VigilantGridVM.CamerasRunning;
            }
        }

        private async void ViewLoadedExecute(object obj)
        {
            var camsLicensed = _AuthenticationService.GetCamerasLicensed();
            if (camsLicensed == null)
                CamerasLicensed = 0;
            else
                CamerasLicensed = _AuthenticationService.GetCamerasLicensed().Value;
            VigilantGridVM.CamerasLicensed = CamerasLicensed;
            if (camerasLicensed == 0)
            {
                var authDetails = _AuthenticationService.GetAuthDetails();
                var UnsetActivatedFlag = _AuthenticationService.UnsetActivatedFlag(authDetails);
                await UnsetActivatedFlag;

                _messageTypeToHomeVM.Value = new FromMultiControllerVMToMultiControllerHomeVM("LoginView");
                _messageTypeToHomeVM.Message = "Login View to be shown";
                _messageTypeToHomeVM.Event = BroadCastEvents.ShowLoginView;
                MessengerInstance.Send(new NotificationMessage<MessageType<FromMultiControllerVMToMultiControllerHomeVM>>(_messageTypeToHomeVM, _messageTypeToHomeVM.Message));


                return;
            }

            LoadSettingsFormConfig();
            SetEmailConfigurationVisibility();
            SetDropboxConfigurationVisibility();
            SetPushbulletConfigurationVisibility();
        }

        private void PushbulletConfigurationExecute(object obj)
        {
            _messageTypeToHomeVM.Value = new FromMultiControllerVMToMultiControllerHomeVM("PushbulletView");
            _messageTypeToHomeVM.Message = "Pushbullet View to be shown";
            _messageTypeToHomeVM.Event = BroadCastEvents.ShowPushbulletSettingsView;
            MessengerInstance.Send(new NotificationMessage<MessageType<FromMultiControllerVMToMultiControllerHomeVM>>(_messageTypeToHomeVM, _messageTypeToHomeVM.Message));
        }

        private void EmailConfigurationExecute(object obj)
        {
            _messageTypeToHomeVM.Value = new FromMultiControllerVMToMultiControllerHomeVM("EmailView");
            _messageTypeToHomeVM.Message = "Email View to be shown";
            _messageTypeToHomeVM.Event = BroadCastEvents.ShowEmailSettingsView;
            MessengerInstance.Send(new NotificationMessage<MessageType<FromMultiControllerVMToMultiControllerHomeVM>>(_messageTypeToHomeVM, _messageTypeToHomeVM.Message));
        }

        private void DropboxConfigurationExecute(object obj)
        {
            _messageTypeToHomeVM.Value = new FromMultiControllerVMToMultiControllerHomeVM("DropboxView");
            _messageTypeToHomeVM.Message = "Dropbox View to be shown";
            _messageTypeToHomeVM.Event = BroadCastEvents.ShowDropboxSettingsView;
            MessengerInstance.Send(new NotificationMessage<MessageType<FromMultiControllerVMToMultiControllerHomeVM>>(_messageTypeToHomeVM, _messageTypeToHomeVM.Message));
        }

        private void SetPushbulletConfigurationVisibility()
        {
            if (IsAccessTokenExists())
            {
                IsPushbulletConfigured = true;
                PushbulletConfigurationVisibility = Visibility.Visible;
            }
            else
            {
                IsPushbulletConfigured = false;
                PushbulletConfigurationVisibility = Visibility.Visible;
            }
        }

        private void SetEmailConfigurationVisibility()
        {
            if (_EmailService.IsSMTPHostSet().HasValue && _EmailService.IsSMTPHostSet().Value)
                IsEmailConfigured = true;
            else
                IsEmailConfigured = false;            
        }

        private void SetDropboxConfigurationVisibility()
        {
            var bIsTokenExists = _DropboxService.IsAccessTokenExists();
            if (bIsTokenExists.HasValue && bIsTokenExists.Value)
                IsDropboxConfigured = true;
            else
                IsDropboxConfigured = false;
        }

        private bool IsAccessTokenExists()
        {
            string dataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Pushbullet.WPF");
            string accessToken = Path.Combine(dataDirectory, "access_token.bin");
            if (File.Exists(accessToken))
                return true;
            return false;
        }

        private bool CanStopAllCamerasExecute(object arg)
        {
            return true;
        }

        private bool CanStartAllCamerasExecute(object arg)
        {
            return true;
        }

        private void NotifyMe(NotificationMessage<MessageType<int>> notificationMessage)
        {
            MessageType<int> content = notificationMessage.Content;
            switch (content.Event)
            {
                case BroadCastEvents.MainWindowClosing:
                    {
                        if (isConfigLoaded)
                        {
                            _messageType.Value = new FromMultiControllerVMToVigilantGridVM();
                            _messageType.Message = "Main Window Closing";
                            _messageType.Event = BroadCastEvents.MainWindowClosing;
                            MessengerInstance.Send(new NotificationMessage<MessageType<FromMultiControllerVMToVigilantGridVM>>(_messageType, _messageType.Message));

                            Settings.Default.CellHeight = CellHeight;
                            Settings.Default.Rows = Rows;
                            Settings.Default.Columns = Columns;
                            Settings.Default.IsGridOn = IsGridOn;
                            Settings.Default.IsGridHeaderOn = IsGridHeaderOn;
                            Settings.Default.Save();
                        }
                    }
                    break;
                case BroadCastEvents.LoadGridView:
                    {
                        VigilantGridVM.CamerasLicensed = CamerasLicensed;
                        LoadSettingsFormConfig();
                    }
                    break;
            }
        }       

        private void StoryboardCurrentTimeInvalidatedExecuted(object param)
        {
            IsAnimationRunning = true;
        }

        private void StoryBoardCompletedExecuted(object param)
        {
            IsAnimationRunning = false;
        }

        private void LoadSettingsFormConfig()
        {
            if (isConfigLoaded)
                return;
            _rows = Settings.Default.Rows;
            _columns = Settings.Default.Columns;
            _cellHeight = Settings.Default.CellHeight;

            CellWidth = (int)Math.Ceiling(_cellAspectRatio * CellHeight);
            _isGridOn = Settings.Default.IsGridOn;
            _isGridHeaderOn = Settings.Default.IsGridHeaderOn;

            if (_rows == 0)
                _rows = 1;
            if (_columns == 0)
                _columns = 1;
            if (_cellHeight == 0)
                _cellHeight = 45;

            RaisePropertyChanged(() => Rows);
            RaisePropertyChanged(() => Columns);
            RaisePropertyChanged(() => CellHeight);
            RaisePropertyChanged(() => IsGridOn);
            RaisePropertyChanged(() => IsGridHeaderOn);

            

            ShowHideHeaders.RaiseCanExecuteChanged();
             
            _messageType.Value = new FromMultiControllerVMToVigilantGridVM(Rows, Columns, CellHeight, CellWidth, IsGridOn, IsGridHeaderOn);
            _messageType.Message = "MultiControllerVM Config Loaded";
            _messageType.Event = BroadCastEvents.MultiControllerVMConfigLoaded;
            MessengerInstance.Send(new NotificationMessage<MessageType<FromMultiControllerVMToVigilantGridVM>>(_messageType, _messageType.Message));
            isConfigLoaded = true;
        }

        private void BorderMouseLeaveExecute(object param)
        {
            ShowHideLeftMenu = "Hide";
        }

        private void BorderMouseEnterExecute(object param)
        {
            ShowHideLeftMenu = "Show";
        }

        private void StartAllCamerasExecute(object param)
        {
            _messageType.Message = "Start all the cameras";
            _messageType.Event = BroadCastEvents.StartAllCameras;
            MessengerInstance.Send(new NotificationMessage<MessageType<FromMultiControllerVMToVigilantGridVM>>(_messageType, _messageType.Message));
        }

        private void StopAllCamerasExecute(object param)
        {
            _messageType.Message = "Stop all the cameras";
            _messageType.Event = BroadCastEvents.StopAllCameras;
            MessengerInstance.Send(new NotificationMessage<MessageType<FromMultiControllerVMToVigilantGridVM>>(_messageType, _messageType.Message));
        }

        private void ConfigureViewExecute(object param)
        {
            //IsGridOn = !IsGridOn;
            //_messageType.Value = new FromMultiControllerVMToVigilantGridVM(Rows, Columns, CellHeight, CellWidth, IsGridOn, IsGridHeaderOn);
            //_messageType.Message = (IsGridOn ? "View should be configured" : "View should be cleared");
            //_messageType.Event = BroadCastEvents.ConfigureViewChanged;
            //MessengerInstance.Send(new NotificationMessage<MessageType<FromMultiControllerVMToVigilantGridVM>>(_messageType, _messageType.Message));
        }

        private void ShowHideHeadersExecute(object param)
        {
            IsGridHeaderOn = !IsGridHeaderOn;
            _messageType.Value = new FromMultiControllerVMToVigilantGridVM(Rows, Columns, CellHeight, CellWidth, IsGridOn, IsGridHeaderOn);
            _messageType.Message = (IsGridHeaderOn ? "Hide Headers" : "Show Headers");
            _messageType.Event = BroadCastEvents.ShowHideHeadersChanged;
            MessengerInstance.Send(new NotificationMessage<MessageType<FromMultiControllerVMToVigilantGridVM>>(_messageType, _messageType.Message));
        }

        private void PinSetExecute(object param)
        {
            if (PinImageSource.OriginalString == @"/Icons/PinOn.png")
                PinImageSource = new Uri(@"/Icons/PinRemoved.png", UriKind.Relative);
            else if (PinImageSource.OriginalString == @"/Icons/PinRemoved.png")
                PinImageSource = new Uri(@"/Icons/PinOn.png", UriKind.Relative);
        }

        private void ShowHideMenu(string storyboard, StackPanel pnl)
        {
            bool flag = this.PinImageSource.OriginalString.EndsWith("PinRemoved.png") && !this.IsAnimationRunning;
            if (flag)
            {
            }
        }
    }
}
