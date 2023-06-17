using Aitoe.Vigilant.Controller.BL.Infra;
using Aitoe.Vigilant.Controller.BL.Infra.Events;
using Aitoe.Vigilant.Controller.BL.RepositoryInterfaces;
using Aitoe.Vigilant.Controller.WpfController.Infra;
using Aitoe.Vigilant.Controller.WpfController.Infra.Extensions;
using Aitoe.Vigilant.Controller.WpfController.Infra.Messages;
using AutoMapper;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Aitoe.Vigilant.Controller.WpfController.ViewModel
{
    public class VigilantGridViewModel : ViewModelBase, IPageViewModel
    {
        public string Name
        {
            get
            {
                return "Aitoe Red _Grid View";
            }
        }

        private int _rows;
        public int Rows
        {
            get
            {
                return _rows;
            }

            set
            {
                if (_rows == value)
                {
                    return;
                }
                _rows = value;
                RaisePropertyChanged(() => Rows);
            }
        }

        private int _columns;
        public int Columns
        {
            get
            {
                return _columns;
            }

            set
            {
                if (_columns == value)
                {
                    return;
                }
                _columns = value;
                RaisePropertyChanged(() => Columns);
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
                RaisePropertyChanged(() => CellHeight);
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
                RaisePropertyChanged(() => CellWidth);
            }
        }

        private ObservableCollection<CellDescBase> _cells;

        public ObservableCollection<CellDescBase> Cells
        {
            get
            {
                return _cells;
            }
            set
            {
                _cells = value;
                RaisePropertyChanged(() => Cells);
            }
        }      

        private ICamProcRepository _CameraRepository = null;

        private IMapper _InternalMapper { get; set; }

        private IMessageBoxService _MessageBoxService { get; set; }

        public VigilantGridViewModel(ICamProcRepository camRepo, IMapper mapper, IMessageBoxService messageBoxService)
        {
            if (camRepo == null)
                throw new ArgumentNullException("ICamProcRepository is null");

            _CameraRepository = camRepo;

            if (mapper == null)
                throw new ArgumentNullException("IMapper is null");

            _InternalMapper = mapper;

            if (messageBoxService == null)
                throw new ArgumentNullException("MessageBoxService is null");

            _MessageBoxService = messageBoxService;

            //MessengerInstance.Register<NotificationMessage<MessageType<int>>>(this, NotifyMultiControllerHomeVMChanges);
            MessengerInstance.Register<NotificationMessage<MessageType<FromMultiControllerVMToVigilantGridVM>>>(this, NotifyMultiControllerVMChanges);
            MessengerInstance.Register<NotificationMessage<MessageType<FromVigilantSingleProcessVMToVigilantGridVM>>>(this, NotifyVigilantSingleProcessVMChanges);
            //InitializeVigilantGridViewModel();
        }

        private void InitializeVigilantGridViewModel()
        {
            _CameraRepository.LoadProcInfoFromSettings();
            var redCells = _CameraRepository.GetAllAitoeRedCells();

            foreach (var redCell in redCells)
            {
                redCell.ProcessChangeEvent += RedCell_ProcessChangeEvent;
            }
            
            if (Cells == null)
                Cells = new ObservableCollection<CellDescBase>();

            if (redCells.Count != 0)
                Cells.AddCell(0, 0);

            int rowCount = redCells.GetRows();
            int columnCount = redCells.GetColumns();

            for (int i = 1; i <= rowCount; i++)
                Cells.AddCell(i, 0);

            for (int j = 1; j <= columnCount; j++)
                Cells.AddCell(0, j);

            foreach (var redCell in redCells)
            {
                var cell = new VigilantSingleProcessViewModel(redCell, _InternalMapper);
                Cells.Add(cell);
            }

            foreach (var cell in Cells)
            {
                if (cell is VigilantSingleProcessViewModel)
                {
                    var vigCell = cell as VigilantSingleProcessViewModel;
                    if (CamerasRunning <= CamerasLicensed - 1)
                        vigCell.AitoeRedCellModel.CheckForAitoeRedProcSerializedStatusAndTryStart();
                    else
                        vigCell.AitoeRedCellModel.SetAitoeRedProcessStatusToStopped();
                }
            }
        }

        private void RedCell_ProcessChangeEvent(object sender, ReadOnlyEventArgs<AitoeRedProcessStatus> e)
        {
            int iCount = 0;
            foreach (var cell in Cells)
            {
                if (cell is VigilantSingleProcessViewModel)
                {
                    var vigCell = cell as VigilantSingleProcessViewModel;
                    var redProcStatus = vigCell.AitoeRedCellModel.GetAitoeRedProcStatus();
                    if (redProcStatus == AitoeRedProcessStatus.Running)
                        iCount = iCount + 1;
                }
            }
            CamerasRunning = iCount;
        }

        private void NotifyVigilantSingleProcessVMChanges(NotificationMessage<MessageType<FromVigilantSingleProcessVMToVigilantGridVM>> notificationMessage)
        {
            var content = notificationMessage.Content;
            var currentCell = GetVigilantSingleProcessCell(content.Value.Row, content.Value.Column);

            switch (content.Event)
            {
                case BroadCastEvents.SingleProcessWebCamChanged:
                    {
                        foreach (var cell in Cells)
                        {
                            if (cell is VigilantSingleProcessViewModel)
                            {
                                var vspvmCell = cell as VigilantSingleProcessViewModel;
                                if (vspvmCell.Column == content.Value.Column && vspvmCell.Row == content.Value.Row)
                                    continue;
                                if (vspvmCell.IsWebCam)
                                {
                                    if (vspvmCell.VigilantVisibility == Visibility.Visible)
                                    {
                                        var message = string.Format("You are attempting setup Web cam in cell {0} (row {1} and column {2})", content.Value.CellName, content.Value.Row, content.Value.Column);
                                        message = message + Environment.NewLine;
                                        message = message + string.Format("Web cam is already selected and running in {0} ", vspvmCell.CellName);
                                        message = message + Environment.NewLine;
                                        message = message + "Please stop that and then continue.";
                                        var result = _MessageBoxService.Show(message, "Confirmation", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                                        currentCell.IsWebCam = false;
                                        //if (result == MessageBoxResult.Yes)
                                        //{
                                        //    // Do nothing.
                                        //}
                                        //else
                                        //{
                                        //    currentCell.IsWebCam = false;
                                        //}
                                    }
                                }
                            }
                        }
                    }
                    break;

                case BroadCastEvents.SingleProcessIpAddressChanged:
                    {
                        foreach (var cell in Cells)
                        {
                            if (cell is VigilantSingleProcessViewModel)
                            {
                                var vspvmCell = cell as VigilantSingleProcessViewModel;
                                if (vspvmCell.Column == content.Value.Column && vspvmCell.Row == content.Value.Row)
                                    continue;

                                if (vspvmCell.IpAddress == currentCell.IpAddress)
                                {

                                    if (vspvmCell.VigilantVisibility == Visibility.Visible)
                                    {
                                        var message = string.Format("You are attempting to setup ip camera in cell {0} (row {1} and column {2}) with ip address {3}", content.Value.CellName, content.Value.Row, content.Value.Column, content.Value.IpAddress);
                                        message = message + Environment.NewLine;
                                        message = message + string.Format("This ip address is already selected and started in {0} ", vspvmCell.CellName);
                                        message = message + Environment.NewLine;
                                        message = message + "Please stop that and then continue.";
                                        var result = _MessageBoxService.Show(message, "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                                        currentCell.IpAddress = string.Empty;
                                        //if (result == MessageBoxResult.Yes)
                                        //{
                                        //    // Do nothing.
                                        //}
                                        //else
                                        //{
                                        //    currentCell.IpAddress = string.Empty;
                                        //}
                                    }
                                }
                            }
                        }
                    }
                    break;
            }
        }

        private VigilantSingleProcessViewModel GetVigilantSingleProcessCell(int row, int column)
        {
            var cell = Cells.Where(c => c.Row == row && c.Column == column).FirstOrDefault();
            if (cell == null)
                return null;

            return cell as VigilantSingleProcessViewModel;
        }

        private void NotifyMultiControllerVMChanges(NotificationMessage<MessageType<FromMultiControllerVMToVigilantGridVM>> notificationMessage)
        {
            var content = notificationMessage.Content;
            switch (content.Event)
            {
                case BroadCastEvents.CellHeightChanged:
                    Rows = content.Value.Rows.HasValue ? content.Value.Rows.Value + 1 : 1;
                    Columns = content.Value.Columns.HasValue ? content.Value.Columns.Value + 1 : 1;
                    CellHeight = content.Value.CellHeight.HasValue ? content.Value.CellHeight.Value : 0;
                    CellWidth = content.Value.CellWidth.HasValue ? content.Value.CellWidth.Value : 0;
                    SetCellHeightWidth();
                    break;

                case BroadCastEvents.RowSizeChanged:
                    Rows = content.Value.Rows.HasValue ? content.Value.Rows.Value + 1 : 1;
                    Columns = content.Value.Columns.HasValue ? content.Value.Columns.Value + 1 : 1;
                    AdjustCellsforRowsAndColumns();
                    break;

                case BroadCastEvents.ColumnSizeChanged:
                    Rows = content.Value.Rows.HasValue ? content.Value.Rows.Value + 1 : 1;
                    Columns = content.Value.Columns.HasValue ? content.Value.Columns.Value + 1 : 1;
                    AdjustCellsforRowsAndColumns();
                    break;

                /*
            case BroadCastEvents.ConfigureViewChanged:
                if (content.Value.IsGridOn.HasValue)
                    if (content.Value.IsGridOn.Value)
                    {
                        AdjustCellsforRowsAndColumns();
                    }
                    else
                    {
                        StopAllVigilantInstances();
                        ClearView();
                        _CameraRepository.GetAllAitoeRedCells().Clear();
                    }
                else
                    throw new Exception("Problem in ConfigureViewChanged");
                break;
                */

                case BroadCastEvents.ShowHideHeadersChanged:
                    if (content.Value.IsGridHeaderOn.HasValue)
                        if (content.Value.IsGridHeaderOn.Value)
                            IsGridHeaderOn = true; // show headers
                        else
                            IsGridHeaderOn = false; // Dont show the headers.
                    else
                        throw new Exception("Problem in ShowHideHeadersChanged");
                    ShowHideHeaders();
                    break;

                case BroadCastEvents.MultiControllerVMConfigLoaded:

                    Rows = content.Value.Rows.HasValue ? content.Value.Rows.Value + 1 : 1;
                    Columns = content.Value.Columns.HasValue ? content.Value.Columns.Value + 1 : 1;
                    CellHeight = content.Value.CellHeight.HasValue ? content.Value.CellHeight.Value : 0;
                    CellWidth = content.Value.CellWidth.HasValue ? content.Value.CellWidth.Value : 0;
                    if (content.Value.IsGridOn.HasValue)
                    {
                        //if (content.Value.IsGridOn.Value)
                        //{
                        InitializeVigilantGridViewModel();
                        AdjustCellsforRowsAndColumns();
                        //}
                        //else
                        // View should be cleared
                        //{
                        //StopAllVigilantInstances();
                        //ClearView();
                        //}
                    }
                    else
                        throw new Exception("Problem in ConfigureViewChanged");

                    if (content.Value.IsGridHeaderOn.HasValue)
                        if (content.Value.IsGridHeaderOn.Value)
                            IsGridHeaderOn = true; // show headers
                        else
                            IsGridHeaderOn = false; // Dont show the headers.
                    else
                        throw new Exception("Problem in ShowHideHeadersChanged");
                    ShowHideHeaders();
                    break;
                case BroadCastEvents.StartAllCameras:
                    StartAllVigilantInstances();
                    break;
                case BroadCastEvents.StopAllCameras:
                    StopAllVigilantInstances("StopAllCameras");
                    break;
                case BroadCastEvents.MainWindowClosing:
                    {
                        SerializeGridCellsAndSaveInSettings();
                        StopAllVigilantInstances();
                        ClearView();
                    }
                    break;
                default:
                    break;
            }
        }

        //private void NotifyMultiControllerHomeVMChanges(NotificationMessage<MessageType<int>> notificationMessage)
        //{
        //    MessageType<int> changedValue = notificationMessage.Content;
        //    switch (changedValue.Event)
        //    {
        //        case BroadCastEvents.MainWindowClosing:
        //            {
        //                // Serrialize the cells and then store it the settings.
        //                //SerializeGridCellsAndSaveInSettings();
        //                //StopAllVigilantInstances();
        //                //ClearView();
        //            }
        //            break;
        //    }
        //}

        private void SerializeGridCellsAndSaveInSettings()
        {
            if (Cells != null)
            {
                var vigilantCellVMs = Cells.Where(c => c.GetType() == typeof(VigilantSingleProcessViewModel)).ToList();
                foreach (var cell in vigilantCellVMs)
                {
                    var cellVM = (VigilantSingleProcessViewModel)cell;
                    _InternalMapper.Map(cellVM, cellVM.AitoeRedCellModel);
                }
            }
            _CameraRepository.PersistProcInfoToSettings();
            _CameraRepository.CloseAllProcesses();
        }

        private bool _isGridHeaderOn = true;
        public bool IsGridHeaderOn
        {
            get { return _isGridHeaderOn; }
            set
            {
                if (_isGridHeaderOn == value)
                    return;
                _isGridHeaderOn = value;
                RaisePropertyChanged(() => IsGridHeaderOn);
            }
        }
        private void ClearView()
        {
            if (Cells == null)
                return;
            if (Cells.Count == 0)
                return;
            Cells.Clear();
        }
        private void StartAllVigilantInstances()
        {
            //Cells.Where(c => c.GetType() == typeof(VigilantSingleProcessViewModel)).ToList().ForEach(c => ((VigilantSingleProcessViewModel)c).StartVigilantProcessManually());

            foreach (var cell in Cells)
            {
                if (cell is VigilantSingleProcessViewModel)
                {
                    if (CamerasRunning <= CamerasLicensed - 1)
                    {
                        var vigCell = cell as VigilantSingleProcessViewModel;
                        vigCell.StartVigilantProcessManually();
                    }
                }
            }

        }

        private void StopAllVigilantInstances(string sScenario = null)
        {
            _CameraRepository.CloseAllProcesses(sScenario);
            return;
        }

        private void ShowHideHeaders()
        {
            if (Cells == null)
                return;
            if (!IsGridHeaderOn)
            {
                var cornerCellVMs = Cells.Where(c => c.GetType() == typeof(CornerHeaderCell)).ToList();
                foreach (var cornerCell in cornerCellVMs)
                    ((CornerHeaderCell)cornerCell).CellName = "";

                var rowHeaderCellVMs = Cells.Where(c => c.GetType() == typeof(RowHeaderCell)).ToList();
                foreach (var rowHeaderCell in rowHeaderCellVMs)
                    ((RowHeaderCell)rowHeaderCell).CellName = "";

                var columnHeaderCellVMs = Cells.Where(c => c.GetType() == typeof(ColumnHeaderCell)).ToList();
                foreach (var columnHeaderCell in columnHeaderCellVMs)
                    ((ColumnHeaderCell)columnHeaderCell).CellName = "";
            }
            else
            {
                var cornerCellVMs = Cells.Where(c => c.GetType() == typeof(CornerHeaderCell)).ToList();
                foreach (var cornerCell in cornerCellVMs)
                    ((CornerHeaderCell)cornerCell).CellName = " ";// Just a space.
                Cells.SetRowHeaderText();
                Cells.SetColumnHeaderText();
            }
        }

        private void SetCellHeightWidth()
        {
            var rowHeaderCells = Cells.Where(c => c.GetType() == typeof(RowHeaderCell)).ToList();
            foreach (var rowHeaderCell in rowHeaderCells)
            {
                var rhc = (RowHeaderCell)rowHeaderCell;
                rhc.CellHeight = CellHeight;
            }

            var columnHeaderCells = Cells.Where(c => c.GetType() == typeof(ColumnHeaderCell)).ToList();

            foreach (var columnHeaderCell in columnHeaderCells)
            {
                var chc = (ColumnHeaderCell)columnHeaderCell;
                chc.CellWidth = CellWidth;
            }

            var vigilantCellVMs = Cells.Where(c => c.GetType() == typeof(VigilantSingleProcessViewModel)).ToList();

            foreach (var vigilantCell in vigilantCellVMs)
            {
                var vc = (VigilantSingleProcessViewModel)vigilantCell;
                vc.CellWidth = CellWidth;
                vc.CellHeight = CellHeight;
            }
        }

        private void AdjustCellsforRowsAndColumns()
        {
            Cells.AddCell(0, 0); // Just a space.
            var camRepoCells = _CameraRepository.GetAllAitoeRedCells();
            var vigilantCellVMs = Cells.Where(c => c.GetType() == typeof(VigilantSingleProcessViewModel)).ToList();
            int camRepoCellsRowsCount = camRepoCells.GetRows();
            int camRepoCellsColumnCount = camRepoCells.GetColumns();
            if (camRepoCellsRowsCount > Rows - 1)
            {
                var rowHeaderCellVMs = Cells.Where(c => c.GetType() == typeof(RowHeaderCell)).ToList();
                for (int i = camRepoCellsRowsCount; i > Rows - 1; i--)
                {
                    foreach (var rowHeaderCell in rowHeaderCellVMs)
                        if (((RowHeaderCell)rowHeaderCell).Row == i)
                            Cells.Remove(rowHeaderCell);

                    foreach (var vigilantCellVM in vigilantCellVMs)
                        if (((VigilantSingleProcessViewModel)vigilantCellVM).Row == i)
                            Cells.RemoveCell((VigilantSingleProcessViewModel)vigilantCellVM, _CameraRepository);
                }
            }
            else
            {
                var rowToBeAdded = Rows - camRepoCellsRowsCount - 1;
                for (int i = camRepoCellsRowsCount + 1; i < Rows; i++)
                {
                    Cells.AddCell(i, 0); // Columns
                    for (int j = 1; j < Columns; j++)
                    {
                        var vigilantCell = Cells.AddCellWithAitoeRed(i, j, _CameraRepository, _InternalMapper);
                    }
                }
            }

            if (camRepoCellsColumnCount > Columns - 1)
            {
                var columnHeaderCellVMs = Cells.Where(c => c.GetType() == typeof(ColumnHeaderCell)).ToList();
                for (int i = camRepoCellsColumnCount; i > Columns - 1; i--)
                {
                    foreach (var HeaderCell in columnHeaderCellVMs)
                        if (((ColumnHeaderCell)HeaderCell).Column == i)
                            Cells.Remove(HeaderCell);

                    foreach (var vigilantCellVM in vigilantCellVMs)
                        if (((VigilantSingleProcessViewModel)vigilantCellVM).Column == i)
                            Cells.RemoveCell((VigilantSingleProcessViewModel)vigilantCellVM, _CameraRepository);
                }
            }
            else
            {
                var columnsToBeAdded = Columns - camRepoCellsColumnCount - 1;
                for (int j = camRepoCellsColumnCount + 1; j < Columns; j++)
                {
                    Cells.AddCell(0, j);
                    for (int i = 1; i < Rows; i++)
                    {
                        Cells.AddCellWithAitoeRed(i, j, _CameraRepository, _InternalMapper);
                    }
                }
            }
            SetCellHeightWidth();
            ShowHideHeaders();
            SubscribeForProcessChangeEvent();
        }

        private void SubscribeForProcessChangeEvent()
        {
            foreach (var cell in Cells)
            {
                if (cell is VigilantSingleProcessViewModel)
                {
                    var vigCell = cell as VigilantSingleProcessViewModel;
                    vigCell.AitoeRedCellModel.ProcessChangeEvent += RedCell_ProcessChangeEvent;
                    //cell.PropertyChanged += Cell_PropertyChanged;
                }
            }
        }

        private int camerasRunning;

        public int CamerasRunning
        {
            get { return camerasRunning; }
            set
            {
                camerasRunning = value;
                PublishCameraCount();
                RaisePropertyChanged();
            }
        }

        private MessageType<FromVigilantGridVMToVigilantSingleProcessVM> 
            _messageToGridVM = new MessageType<FromVigilantGridVMToVigilantSingleProcessVM>();

        private void PublishCameraCount()
        {
            _messageToGridVM.Value = new FromVigilantGridVMToVigilantSingleProcessVM(CamerasRunning, CamerasLicensed);
            _messageToGridVM.Message = "Running Camera Count Changed";
            _messageToGridVM.Event = BroadCastEvents.CameraCountChanged;
            MessengerInstance.Send(new NotificationMessage<MessageType<FromVigilantGridVMToVigilantSingleProcessVM>>(_messageToGridVM, _messageToGridVM.Message));
        }
        public int CamerasLicensed { get; set; }

    }
}