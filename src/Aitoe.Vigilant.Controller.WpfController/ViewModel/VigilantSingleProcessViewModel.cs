using Aitoe.Vigilant.Controller.BL.Entites;
using Aitoe.Vigilant.Controller.BL.Infra;
using Aitoe.Vigilant.Controller.BL.Infra.Events;
using Aitoe.Vigilant.Controller.WpfController.Infra;
using Aitoe.Vigilant.Controller.WpfController.Infra.Messages;
using AutoMapper;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Threading;
using System.Windows;

namespace Aitoe.Vigilant.Controller.WpfController.ViewModel
{
    public class VigilantSingleProcessViewModel : CellDescBase
    {
        public event EventHandler<ReadOnlyEventArgs<AitoeRedProcessStatus>> ProcessChangeEvent;
        private IMapper InternalMapper { get; set; }
        public IAitoeRedCell AitoeRedCellModel { get; private set; }
        
        private Visibility _formVisibility = Visibility.Visible;

        public Visibility FormVisibility
        {
            get { return _formVisibility; }
            set
            {
                if (_formVisibility == value)
                    return;

                _formVisibility = value;
                
                RaisePropertyChanged();
            }
        }

        private Visibility _vigilantVisibility = Visibility.Collapsed;

        public Visibility VigilantVisibility
        {
            get { return _vigilantVisibility; }
            set
            {
                if (_vigilantVisibility == value)
                    return;

                _vigilantVisibility = value;
                
                RaisePropertyChanged();
            }
        }

        private int _cellHeight;
        public int CellHeight
        {
            get
            {
                return _cellHeight;
            }

            set
            {
                if (_cellHeight == value)
                {
                    return;
                }
                _cellHeight = value;
                
                RaisePropertyChanged();
            }
        }

        private int _cellWidth;
        public int CellWidth
        {
            get
            {
                return _cellWidth;
            }

            set
            {
                if (_cellWidth == value)
                {
                    return;
                }
                _cellWidth = value;
                
                RaisePropertyChanged();
            }
        }

        private string sVigilantPath = string.Empty;

        internal int AitoeRedProcessId { get; set; }
        internal IntPtr AitoeRedProcessMainWindowHandle { get; set; }
        
        private ProcInfo _procInfo;
        public ProcInfo VigilantProcInfo
        {
            get
            {
                return _procInfo;
            }
            
            set
            {
                if (_procInfo != null && value != null)
                    if (_procInfo.ProcId == value.ProcId && _procInfo.ProcMainWindowHandle == value.ProcMainWindowHandle)
                        return;
                _procInfo = value;
                RaisePropertyChanged();
            }
        }
        private string _ipAddress;
        public string IpAddress
        {
            get
            {
                return _ipAddress;
            }
            set
            {
                if (_ipAddress == value)
                    return;
                _ipAddress = value;
                BroadcastVigilantSingleProcessVMIpAddressChanges();
                RaisePropertyChanged();
            }
        }
        private string _loginId;
        public string LoginId
        {
            get
            {
                return _loginId;
            }
            set
            {
                if (_loginId == value)
                    return;
                _loginId = value;
            }
        }

        private string _password = string.Empty;
        public string Password
        {
            get
            {
                return _password;
            }
            set {
                if (_password == value)
                    return;
                _password = value;
                RaisePropertyChanged();
            }
        }

        private bool _isWebCam;

        public bool IsWebCam
        {
            get { return _isWebCam; }
            set {
                _isWebCam = value;
                BroadcastVigilantSingleProcessVMWebCamChanges();
                RaisePropertyChanged();
            }
        }

        private MessageType<FromVigilantSingleProcessVMToVigilantGridVM> _messageToGridVM = new MessageType<FromVigilantSingleProcessVMToVigilantGridVM>();
        private void BroadcastVigilantSingleProcessVMIpAddressChanges()
        {
            if (isInitializing)
                return;

            if (string.IsNullOrWhiteSpace(IpAddress))
                return;

            _messageToGridVM.Value = new FromVigilantSingleProcessVMToVigilantGridVM(Row, Column, CellName, IsWebCam, IpAddress);
            _messageToGridVM.Message = "IpAddressChanged";
            _messageToGridVM.Event = BroadCastEvents.SingleProcessIpAddressChanged;
            MessengerInstance.Send(new NotificationMessage<MessageType<FromVigilantSingleProcessVMToVigilantGridVM>>(_messageToGridVM, _messageToGridVM.Message));
        }
        private void BroadcastVigilantSingleProcessVMWebCamChanges()
        {
            if (isInitializing)
                return;

            if (!_isWebCam)
                return;

            _messageToGridVM.Value = new FromVigilantSingleProcessVMToVigilantGridVM(Row, Column, CellName, IsWebCam, IpAddress);
            _messageToGridVM.Message = "WebCamChanged";
            _messageToGridVM.Event = BroadCastEvents.SingleProcessWebCamChanged;
            MessengerInstance.Send(new NotificationMessage<MessageType<FromVigilantSingleProcessVMToVigilantGridVM>>(_messageToGridVM, _messageToGridVM.Message));
        }


        public RelayCommand<object> StartVigilantProcess
        {
            get;
            set;
        }

        private bool isInitializing = false;
        // This ctor is called when de-serialization happens and then vm cells are created.
        public VigilantSingleProcessViewModel(IAitoeRedCell redCell, IMapper mapper) 
        {
            isInitializing = true;
            InitializeVigilantSingleProcessViewModel();
            AitoeRedCellModel = redCell;
            AitoeRedCellModel.ProcessChangeEvent += AitoeRedCellModel_ProcessChangeEvent;
            InternalMapper = mapper;
            try
            {
                InternalMapper.Map(AitoeRedCellModel, this);
            }
            catch (Exception ex)
            {
                throw;
            }
            MessengerInstance.Register<NotificationMessage<MessageType<FromVigilantGridVMToVigilantSingleProcessVM>>>(this, NotifyVigilantGridVMChanges);
            //CheckAndStartAitoeProcess();
            isInitializing = false;
        }

        public VigilantSingleProcessViewModel(int row, int column, string cellName) : base(row, column, cellName)
        {
            MessengerInstance.Register<NotificationMessage<MessageType<FromVigilantGridVMToVigilantSingleProcessVM>>>(this, NotifyVigilantGridVMChanges);
            InitializeVigilantSingleProcessViewModel();
        }

        // This ctor is called when row or column no is increased by user and so a new cell is needed.
        public VigilantSingleProcessViewModel(int row, int column, string cellName, IMapper mapper) : base(row, column, cellName)
        {
            isInitializing = true;
            if (mapper == null)
                throw new ArgumentNullException("IMapper instance is null");

            InternalMapper = mapper;

            InitializeVigilantSingleProcessViewModel();
            AitoeRedCellModel = InternalMapper.Map<VigilantSingleProcessViewModel, IAitoeRedCell>(this);
            AitoeRedCellModel.ProcessChangeEvent += AitoeRedCellModel_ProcessChangeEvent;
            isInitializing = false;
            MessengerInstance.Register<NotificationMessage<MessageType<FromVigilantGridVMToVigilantSingleProcessVM>>>(this, NotifyVigilantGridVMChanges);
        }

        private void NotifyVigilantGridVMChanges(NotificationMessage<MessageType<FromVigilantGridVMToVigilantSingleProcessVM>> notificationMessage)
        {
            //http://stackoverflow.com/a/9732853/1977871
            //http://stackoverflow.com/a/10790098/1977871
            var content = notificationMessage.Content;
            switch (content.Event)
            {
                case BroadCastEvents.CameraCountChanged:
                    {
                        if (content.Value.CamerasRunning < content.Value.CamerasLicensed)
                            isLicenseLimitReached = false;
                        else
                            isLicenseLimitReached = true;

                        if (Application.Current != null)
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                StartVigilantProcess.RaiseCanExecuteChanged();
                            });
                        }
                    }
                    break;
            }
            //
        }


        public bool isLicenseLimitReached { get; set; }

        private bool CanStartVigilantProcessExecute(object arg)
        {
            if (isLicenseLimitReached)
                return false;
            else
                return true;
        }

        private void AitoeRedCellModel_ProcessChangeEvent(object sender, ReadOnlyEventArgs<AitoeRedProcessStatus> e)
        {
            OnProcessChangeEvent(e.Parameter);
            AitoeRedProcessStatus procStatus = e.Parameter;
            if (procStatus == AitoeRedProcessStatus.Running)
                SetUIForSuccessfulStart();
            else
                SetUIForUnSuccessfulStart();
        }      

        private void OnProcessChangeEvent(AitoeRedProcessStatus procStatus)
        {
            var procChangeEvent = ProcessChangeEvent;
            if (procChangeEvent != null)
            {
                var args = procChangeEvent.CreateArgs(procStatus);
                procChangeEvent(this, args);
            }
        }



        public void StartVigilantProcessManually()
        {
            InternalMapper.Map(this, AitoeRedCellModel);
            AitoeRedCellModel.TryStartAitoeRedProcess();
            //InitiateVigilantProcess();
        }
        private void InitializeVigilantSingleProcessViewModel()
        {
            StartVigilantProcess = new RelayCommand<object>(StartVigilantProcessExecute, CanStartVigilantProcessExecute);
            //_startVigilantProcess = new RelayCommand<object>(param => StartVigilantProcessExecute(param), CanStartVigilantProcessExecute);
            sVigilantPath = @"Vigilant.exe";
        }

        private void StartVigilantProcessExecute(object param)
        {
            StartVigilantProcessManually();
        }

        private void SetUIForSuccessfulStart()
        {
            InternalMapper.Map(AitoeRedCellModel, this);
            var vProcInfo = new ProcInfo(IpAddress, AitoeRedProcessId, AitoeRedProcessMainWindowHandle, Row, Column);
            VigilantProcInfo = vProcInfo;
            VigilantVisibility = Visibility.Visible;
            FormVisibility = Visibility.Collapsed;
            //AitoeRedCellModel.AitoeRedProcess.Exited += AitoeRedProcess_Exited;
        }

        private void SetUIForUnSuccessfulStart()
        {
            VigilantVisibility = Visibility.Collapsed;
            FormVisibility = Visibility.Visible;
        }
    }
}