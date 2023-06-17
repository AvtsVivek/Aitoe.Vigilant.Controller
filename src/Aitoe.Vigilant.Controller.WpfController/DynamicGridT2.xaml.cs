using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using Aitoe.Vigilant.Controller.WpfController.Infra;
using System.Windows.Media;
using Aitoe.Vigilant.Controller.WpfController.Properties;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Threading;
using System.Windows.Threading;
using Aitoe.Vigilant.Controller.WpfController.Infra.Extensions;
using Aitoe.Vigilant.Controller.BL.Infra;

namespace Aitoe.Vigilant.Controller.WpfController
{
    /// <summary>
    /// Interaction logic for DynamicGridT2.xaml
    /// </summary>
    public partial class DynamicGridT2 : Window, INotifyPropertyChanged
    {
        public enum CpuUsage
        {
            Low,
            Mid,
            High
        }

        private List<TwoDProcessMatrix> ProcessMatrixDictionary;

        private SortedDictionary<int, string> ColNameDictionary = new SortedDictionary<int, string>();

        string sVigilantPath = @"Vigilant.exe";

        string sVigilantConfigPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\AppData\aiToeRED";

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        private void SaveProcessMatrix()
        {

            Settings.Default.CellHeight = int.Parse(txtCellHeight.Text);

            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, ProcessMatrixDictionary);
                ms.Position = 0;
                byte[] buffer = new byte[(int)ms.Length];
                ms.Read(buffer, 0, buffer.Length);
                Settings.Default.SortedMatrix = Convert.ToBase64String(buffer);
            }
            Settings.Default.Save();
        }

        private List<TwoDProcessMatrix> LoadProcessMatrix()
        {
            using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(Settings.Default.SortedMatrix)))
            {
                if (ms.Length == 0)
                    return null;
                BinaryFormatter bf = new BinaryFormatter();
                var tup = bf.Deserialize(ms);
                if (tup == null)
                    return null;
                else
                    return (List<TwoDProcessMatrix>)tup;
            }
        }


        public int RowSize
        {
            get
            {
                return ProcessMatrixDictionary.Max(t => t.Row);
            }
            set
            {
                int currentRowSize = ProcessMatrixDictionary.Max(t => t.Row);
                if (currentRowSize == value)
                {
                    return;
                }
                else if (currentRowSize < value)
                {
                    // Need to add few more rows.
                    for (int c = 1; c <= ColumnSize; c++)
                    {
                        for (int r = currentRowSize + 1; r <= value; r++)
                        {
                            var m = new TwoDProcessMatrix(c, r, false, 0);
                            ProcessMatrixDictionary.Add(m);
                        }
                    }
                }
                else
                {
                    // Need to remove some rows.
                    for (int c = 1; c <= ColumnSize; c++)
                    {
                        // Example value = 3, and currentNoOfRows = 5
                        for (int r = value + 1; r <= currentRowSize; r++)
                        {
                            ProcessMatrixDictionary.Remove(ProcessMatrixDictionary.Where(t => t.Column == c && t.Row == r).FirstOrDefault());
                        }
                    }
                }
                OnPropertyChanged(new PropertyChangedEventArgs("RowSize"));
            }
        }



        private string _showhideLeftMenu;

        public string ShowHideLeftMenu
        {
            get
            {
                return _showhideLeftMenu;
            }

            set
            {
                if (IsAnimationRunning)
                    return; // Do not set the value as the animation is running.
                //_showhideLeftMenu != null &&
                if (_showhideLeftMenu == "Show" && imgPinPanel.Source.ToString().EndsWith("PinOn.png"))
                    return;
                if (_showhideLeftMenu != value)
                {
                    _showhideLeftMenu = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("ShowHideLeftMenu"));
                }
            }
        }

        public int ColumnSize
        {
            get
            {
                return ProcessMatrixDictionary.Max(t => t.Column);
            }
            set
            {
                int currentColumnSize = ProcessMatrixDictionary.Max(t => t.Column);
                if (currentColumnSize == value)
                {
                    return;
                }
                else if (currentColumnSize < value)
                {
                    // Need to add few more columns.
                    for (int c = currentColumnSize + 1; c <= value; c++)
                    {
                        for (int r = 1; r <= RowSize; r++)
                        {
                            var m = new TwoDProcessMatrix(c, r, false, 0);
                            ProcessMatrixDictionary.Add(m);
                        }
                    }
                }
                else
                {
                    for (int c = value + 1; c <= currentColumnSize; c++)
                    {
                        for (int r = 1; r <= RowSize; r++)
                        {
                            ProcessMatrixDictionary.Remove(ProcessMatrixDictionary.Where(t => t.Column == c && t.Row == r).FirstOrDefault());
                        }
                    }
                }

                OnPropertyChanged(new PropertyChangedEventArgs("ColumnSize"));
            }
        }

        public DynamicGridT2()
        {
            InitializeComponent();
            var processes = Process.GetProcessesByName("Vigilant").ToList();
            processes.ForEach(p => p.Kill());
            this.PropertyChanged += CamViewModel_PropertyChanged;
            DataContext = this;
            InitializeColumnNameDictionaryStrings();


            Rows = new ObservableCollection<RowDescriptor>();
            Columns = new ObservableCollection<ColumnDescriptor>();

            //ProcessMatrix = LoadTuples();

            sVigilantPath = Settings.Default.VigilantPath;
            //string sVigilantPath = @"D:\aitoeva\aitoe_surveillance\aitoe_red\single_camera\build\release\Vigilant.exe";

            if (string.IsNullOrEmpty(sVigilantPath) || string.IsNullOrWhiteSpace(sVigilantPath) || !File.Exists(sVigilantPath))
                sVigilantPath = @"Vigilant.exe";

            ProcessMatrixDictionary = LoadProcessMatrix();

            txtCellHeight.Text = Settings.Default.CellHeight.ToString();

            if (ProcessMatrixDictionary == null)
            {
                ProcessMatrixDictionary = new List<TwoDProcessMatrix>();
                ProcessMatrixDictionary.Add(new TwoDProcessMatrix());
            }

            if (ProcessMatrixDictionary.Count == 0)
                ProcessMatrixDictionary.Add(new TwoDProcessMatrix());

            AddHeader(null);

            for (int i = 0; i < RowSize; i++)
                AddRow(null);

            for (int i = 0; i < ColumnSize; i++)
                AddColumn(null);

            if (Settings.Default.CpuLoadThreshold == 25)
                rbCpuUsage25.IsChecked = true;
            else if (Settings.Default.CpuLoadThreshold == 50)
                rbCpuUsage50.IsChecked = true;
            else if (Settings.Default.CpuLoadThreshold == 75)
                rbCpuUsage75.IsChecked = true;
            else
                rbCpuUsage25.IsChecked = true; // Default in case none works.

            if (String.IsNullOrEmpty(Settings.Default.SliderPanelPinStateUrlPath))
                imgPinPanel.Source = new BitmapImage(new Uri(@"/Icons/PinOn.png", UriKind.Relative));
            else
            {
                imgPinPanel.Source = new BitmapImage(new Uri(Settings.Default.SliderPanelPinStateUrlPath, UriKind.Relative));

                if (imgPinPanel.Source.ToString().EndsWith("PinOn.png"))
                    ShowHideLeftMenu = "Show";
            }

            bAppStarted = true;

            SetPushbulletConfigurationMessageVisibility();
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AddCellsForGridAndStartProcesses();
            ShowHideGridHeaders();

            Thread t = new Thread(new ThreadStart(HelloWordlInLoop));
            t.Start();

        }

        private void HelloWordlInLoop()
        {
            //int i = 0;
            while (true)
            {
                //i = i + 1;
                Thread.Sleep(Settings.Default.PollingDuration);
                base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                    int columnNo, rowNo;

                    for (columnNo = 1; columnNo < MainGrid.ColumnDefinitions.Count; columnNo++)
                    {
                        for (rowNo = 1; rowNo < MainGrid.RowDefinitions.Count; rowNo++)
                        {
                            var host = GetElementAtCell<VigilantCamStream>(columnNo, rowNo);
                            if (host == null)
                            {
                                var pm = ProcessMatrixDictionary.Where(p => p.Column == columnNo && p.Row == rowNo).FirstOrDefault();
                                if (pm.ProcessStatus)
                                {
                                    host = CreateHostForProcessAndAddToGrid(columnNo, rowNo);
                                    CreateProcessAndAttachToHost(host);
                                }
                            }
                            else
                            {
                                if (!IsProcessRunningInCell(host.ColumnNo, host.RowNo))
                                {
                                    var pm = ProcessMatrixDictionary.Where(p => p.Column == host.ColumnNo && p.Row == host.RowNo).FirstOrDefault();
                                    if (pm.ProcessStatus)
                                        CreateProcessAndAttachToHost(host);
                                }
                            }
                        }
                    }
                });
            }
        }

        private void CamViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "ColumnSize":
                    {
                        ResetColumns();
                    }
                    break;

                case "RowSize":
                    {
                        ResetRows();
                    }
                    break;

                default:
                    break;
            }
        }

        private void ResetRows()
        {
            ClearGrid();
            Rows.Clear();
            AddHeader(null);
            for (int i = 0; i < RowSize; i++)
            {
                AddRow(null);
            }
        }

        private void ResetColumns()
        {
            ClearGrid();
            Columns.Clear();
            AddHeader(null);
            for (int i = 0; i < ColumnSize; i++)
            {
                AddColumn(null);
            }
        }

        private void AddColumn(string s)
        {
            string columnHeaderText = ColNameDictionary[Columns.Count() - 1];
            Columns.Add(new ColumnDescriptor { ColumnHeaderText = columnHeaderText });
        }

        private void AddRow(string s)
        {
            Rows.Add(new RowDescriptor { RowNumber = this.Rows.Count().ToString() });
        }

        private void AddHeader(string s)
        {
            if (Columns.Count == 0)
            {
                Columns.Add(this.Columns.FirstOrDefault(d => d.DisplayMember == s));
            }

            if (Rows.Count == 0)
            {
                Rows.Add(this.Rows.FirstOrDefault(d => d.RowHeaderText == s));
            }
        }

        public ObservableCollection<RowDescriptor> Rows { get; private set; }

        public ObservableCollection<ColumnDescriptor> Columns { get; private set; }

        private void InitializeColumnNameDictionaryStrings()
        {
            const string sA = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            var sL = sA.Split(',').ToList();
            sL.Sort();
            int i = 0;
            sL.ForEach(c => ColNameDictionary.Add(i++, c));
        }

        private void btnAddGrid_Click(object sender, RoutedEventArgs e)
        {
            AddCellsForGrid();
        }

        private void btnAddVigilat_Click(object sender, RoutedEventArgs e)
        {
            AddCellsForGridAndStartProcesses();
        }

        private void btnStopAll_Click(object sender, RoutedEventArgs e)
        {
            StopAllProcesses();
        }
        private void btnClearGrid_Click(object sender, RoutedEventArgs e)
        {
            ClearGrid();
        }

        private void ClearGrid()
        {
            StopAllProcesses();
            MainGrid.Children.Clear();
        }

        private void btnStartProcessInCell_Click(object sender, RoutedEventArgs e)
        {
            AddCellsForGrid();
            int columnNo, rowNo;
            if (int.TryParse(txtColNo.Text, out columnNo) && int.TryParse(txtRowNo.Text, out rowNo))
            {
                var host = GetElementAtCell<VigilantCamStream>(columnNo, rowNo);
                if (host == null)
                    host = CreateHostForProcessAndAddToGrid(columnNo, rowNo);
                CreateProcessAndAttachToHost(host);
            }
        }

        private void btnStopProcProcessInCell_Click(object sender, RoutedEventArgs e)
        {
            int columnNo, rowNo;
            if (int.TryParse(txtStopColNo.Text, out columnNo) && int.TryParse(txtStopRowNo.Text, out rowNo))
            {
                var host = GetElementAtCell<VigilantCamStream>(columnNo, rowNo);
                if (host != null)
                    KillHostProcess(host);
            }
        }

        private bool IsProcessRunningInCell(int columnNo, int rowNo)
        {
            var host = GetElementAtCell<VigilantCamStream>(columnNo, rowNo);
            if (host == null)
                return false;
            else
            {
                var process = host.VigilantProcess;
                if (process != null && process.IsRunning() && !process.HasExited)
                    return true;
            }
            return false;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            SaveProcessMatrix(); // Save and then stop. Not the other way.
            StopAllProcesses();  // If you stop and then save, the processStatus becomes false for all the processes.
        }

        /// <summary>
        /// Finds VigilantCamStream at the given cell(columnNo, rowNo) and returns its reference.
        /// If not found, null is returned.
        /// </summary>
        /// <param name="columnNo"></param>
        /// <param name="rowNo"></param>
        private T GetElementAtCell<T>(int columnNo, int rowNo) where T : UIElement
        {
            T element = default(T);
            var uiElements = MainGrid.Children.Cast<UIElement>().Where(uiE => Grid.GetRow(uiE) == rowNo && Grid.GetColumn(uiE) == columnNo);
            foreach (var uiElement in uiElements)
                if (uiElement.GetType() == typeof(T))
                    return (T)uiElement;
            return element; // return null;
        }

        private void AddVigilantProcess()
        {
            if (MainGrid == null)
                return;
            int rowNo, columnNo;
            for (columnNo = 1; columnNo < MainGrid.ColumnDefinitions.Count; columnNo++)
            {
                for (rowNo = 1; rowNo < MainGrid.RowDefinitions.Count; rowNo++)
                {
                    StartVigilantProcessAndAddToGrid(columnNo, rowNo);
                }
            }
        }

        private void AddCellsForGrid()
        {
            int rowNo, columnNo;
            for (columnNo = 0; columnNo < MainGrid.ColumnDefinitions.Count; columnNo++)
            {
                for (rowNo = 0; rowNo < MainGrid.RowDefinitions.Count; rowNo++)
                {
                    AddCell(columnNo, rowNo);
                }
            }
        }
        private void HideGridHeaders()
        {
            int rowNo, columnNo;
            for (columnNo = 0; columnNo < MainGrid.ColumnDefinitions.Count; columnNo++)
            {
                for (rowNo = 0; rowNo < MainGrid.RowDefinitions.Count; rowNo++)
                {
                    HideHeaders(columnNo, rowNo);
                }
            }
        }

        private void ShowHideGridHeaders()
        {
            int rowNo, columnNo;
            for (columnNo = 0; columnNo < MainGrid.ColumnDefinitions.Count; columnNo++)
            {
                for (rowNo = 0; rowNo < MainGrid.RowDefinitions.Count; rowNo++)
                {
                    if (Settings.Default.IsHeadersHidden)
                        HideHeaders(columnNo, rowNo);
                    else
                        ShowHeaders(columnNo, rowNo);
                }
            }
            ToggleShowHideHeaders();
        }


        private void ShowGridHeaders()
        {
            int rowNo, columnNo;
            for (columnNo = 0; columnNo < MainGrid.ColumnDefinitions.Count; columnNo++)
            {
                for (rowNo = 0; rowNo < MainGrid.RowDefinitions.Count; rowNo++)
                {
                    ShowHeaders(columnNo, rowNo);
                }
            }
        }

        private void StartVigilantProcessAndAddToGrid(int columnNo, int rowNo)
        {
            var host = CreateHostForProcessAndAddToGrid(columnNo, rowNo);
            CreateProcessAndAttachToHost(host);
        }

        private VigilantCamStream CreateHostForProcessAndAddToGrid(int columnNo, int rowNo)
        {
            if (rowNo >= MainGrid.RowDefinitions.Count)
                return null;

            if (columnNo >= MainGrid.ColumnDefinitions.Count)
                return null;

            VigilantCamStream host;

            if (IsProcessRunningInCell(columnNo, rowNo))
            {
                host = GetElementAtCell<VigilantCamStream>(columnNo, rowNo);
                return host;
            }

            host = new VigilantCamStream();
            host.RowNo = rowNo;
            host.ColumnNo = columnNo;
            //host.Height = 250;
            //host.Width = 1.21 * host.Height;
            host.Height = double.Parse(txtCellHeight.Text);
            host.Width = double.Parse(lblCellWidth.Content.ToString());
            host.SetValue(Grid.ColumnProperty, columnNo);
            host.SetValue(Grid.RowProperty, rowNo);
            Binding b1 = new Binding("Value");
            b1.Source = slCellHeight;
            b1.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            host.SetBinding(VigilantCamStream.HeightProperty, b1);

            Binding b2 = new Binding("Content");
            b2.Source = lblCellWidth;
            b2.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            host.SetBinding(VigilantCamStream.WidthProperty, b2);

            //host.VigilantPanel.Height = (int)host.Height;
            //host.VigilantPanel.Width = (int)host.Width;

            MainGrid.Children.Add(host);
            return host;
        }

        private void CreateProcessAndAttachToHost(VigilantCamStream host)
        {
            if (host == null)
                return;

            if (IsProcessRunningInCell(host.ColumnNo, host.RowNo))
                return;

            int columnSize = MainGrid.ColumnDefinitions.Count - 1;
            int rowNo = host.RowNo;
            int columnNo = host.ColumnNo;
            int iProcessArg = (rowNo - 1) * columnSize + columnNo;
            bool bStartedCorrectly = false;

            for (int i = 0; i < 5; i++)
            {
                var p = new Process();
                p.StartInfo = new ProcessStartInfo(sVigilantPath);
                p.StartInfo.Arguments = "GOP1N@TH108 " + iProcessArg.ToString() + " " + Settings.Default.IsDebug.ToString();
                
                host.VigilantProcess = p;
                host.PWS = ProcessWindowStyle.Minimized;

                if (host.StartVigilatProcess())
                {
                    bStartedCorrectly = true;
                    UpdateProcessMatrix(host, true);
                    break;
                }
                else
                    KillHostProcess(host);
            }

            if (!bStartedCorrectly)
                //throw new Exception(String.Format("Process Could not start for column {0} and row {1}", host.ColumnNo, host.RowNo));
                MessageBox.Show("Could not start Aitoe-Red Process.", "Aitoe-Process Failure", MessageBoxButton.OK, MessageBoxImage.Error);
                        
            //host.WindowMoved += Host_WindowMoved;
        }

        private void UpdateProcessMatrix(VigilantCamStream host, bool processStatus)
        {
            int iProcessArg;
            if (processStatus)
                iProcessArg = int.Parse(host.VigilantProcess.StartInfo.Arguments.Split()[1]);
            else
                iProcessArg = 0; // For the process that stoped; process status is false.

            var pm = ProcessMatrixDictionary.Where(p => p.Column == host.ColumnNo && p.Row == host.RowNo).FirstOrDefault();
            if (pm != null)
                pm.UpdateProcessMatrix(processStatus, iProcessArg);
        }

        private void Host_WindowMoved(object sender, WindowMovedEventArgs e)
        {
            /*var host = (VigilantCamStream)sender;

            if(host.ColumnNo == 1)
                MyLable1.Content = e.SomeInfo;
            if (host.ColumnNo == 2)
                MyLable2.Content = e.SomeInfo;*/

        }

        private void ShowHeaders(int columnNo, int rowNo)
        {
            var label = GetElementAtCell<Label>(columnNo, rowNo);
            if (label != null)
                ShowHeader(columnNo, rowNo, label);
        }

        private void HideHeaders(int columnNo, int rowNo)
        {
            var label = GetElementAtCell<Label>(columnNo, rowNo);
            if (label != null)
                HideHeader(columnNo, rowNo, label);
        }

        private void AddCell(int columnNo, int rowNo)
        {
            var label = GetElementAtCell<Label>(columnNo, rowNo);
            if (label == null)
                CreateLableAndAddToGrid(columnNo, rowNo);
        }

        private void HideHeader(int columnNo, int rowNo, Label label)
        {
            if (rowNo == 0 && columnNo == 0)
                return;

            if (rowNo == 0)
            {
                label.Content = null;
                label.BorderThickness = new Thickness(0);
            }
            else if (columnNo == 0)
            {
                label.Content = null;
                label.BorderThickness = new Thickness(0);
            }
        }


        private void CreateLableAndAddToGrid(int columnNo, int rowNo)
        {
            if (rowNo == 0 && columnNo == 0)
                return;
            //if (rowNo != 0 && columnNo != 0)
            //  return;

            var label = new Label();
            if (rowNo == 0)
                label.Content = ColNameDictionary[columnNo - 1];
            else if (columnNo == 0)
                label.Content = rowNo.ToString();

            if (rowNo != 0 && columnNo != 0)
            {
                Binding b1 = new Binding("Value");
                b1.Source = slCellHeight;
                b1.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                label.SetBinding(Label.HeightProperty, b1);

                Binding b2 = new Binding("Content");
                b2.Source = lblCellWidth;
                b2.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                label.SetBinding(Label.WidthProperty, b2);
            }
            label.SetValue(Grid.RowProperty, rowNo);
            label.SetValue(Grid.ColumnProperty, columnNo);
            label.HorizontalContentAlignment = HorizontalAlignment.Center;
            label.VerticalContentAlignment = VerticalAlignment.Center;
            label.FontWeight = FontWeights.ExtraBold;
            label.BorderBrush = new SolidColorBrush(Colors.Gray);
            label.BorderThickness = new Thickness(2);
            MainGrid.Children.Add(label);
        }

        private void ShowHeader(int columnNo, int rowNo, Label label)
        {
            if (rowNo == 0 && columnNo == 0)
                return;

            if (rowNo == 0)
                label.Content = ColNameDictionary[columnNo - 1];
            else if (columnNo == 0)
                label.Content = rowNo.ToString();
            else
                return;

            label.HorizontalContentAlignment = HorizontalAlignment.Center;
            label.VerticalContentAlignment = VerticalAlignment.Center;
            label.FontWeight = FontWeights.ExtraBold;
            label.BorderBrush = new SolidColorBrush(Colors.Gray);
            label.BorderThickness = new Thickness(2);
        }


        private void AddCellsForGridAndStartProcesses()
        {
            AddCellsForGrid();
            AddVigilantProcess();
        }

        private void StopAllProcesses()
        {
            if (MainGrid == null)
                return;

            List<VigilantCamStream> toBeRemovedHosts = new List<VigilantCamStream>();

            foreach (var item in MainGrid.Children)
            {
                if (item is VigilantCamStream)
                {
                    var host = item as VigilantCamStream;
                    KillHostProcess(host);
                    toBeRemovedHosts.Add(host);
                }
            }

            toBeRemovedHosts.ForEach(h => MainGrid.Children.Remove(h));
        }

        private int GetRunningProcessCount()
        {
            int processCount = 0;

            if (MainGrid == null)
                return 0;
            foreach (var item in MainGrid.Children)
            {
                if (item is VigilantCamStream)
                {
                    var host = item as VigilantCamStream;
                    Process process = host.VigilantProcess;
                    if (process.IsRunning() && process != null && !process.HasExited)
                        processCount++;
                }
            }
            return processCount;
        }

        private void KillHostProcess(VigilantCamStream host)
        {
            Process process = host.VigilantProcess;
            if (process.IsRunning() && process != null && !process.HasExited)
            {
                process.Kill();
            }
            UpdateProcessMatrix(host, false);
        }

        //private void ShowHideMenu(string storyboard, Button btnHide, Button btnShow, StackPanel pnl)
        //{
        //    if (imgPinPanel.Source.ToString().EndsWith("PinRemoved.png") && !bAnimationRunning)
        //    {
        //        Storyboard sb = Resources[storyboard] as Storyboard;
        //        sb.Begin(pnl);
        //    }
        //}

        private void Border_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ShowHideLeftMenu = "Hide";
        }

        private void Border_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ShowHideLeftMenu = "Show";
        }

        private void btnShowHideHeaders_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default.IsHeadersHidden = !Settings.Default.IsHeadersHidden;
            ShowHideGridHeaders();
        }
                
        private void btnPinPanel_Click(object sender, RoutedEventArgs e)
        {
            if (imgPinPanel.Source.ToString().EndsWith("PinRemoved.png"))
            {
                imgPinPanel.Source = new BitmapImage(new Uri(@"/Icons/PinOn.png", UriKind.Relative));
                Settings.Default.SliderPanelPinStateUrlPath = @"/Icons/PinOn.png";
            }
            else if (imgPinPanel.Source.ToString().EndsWith("PinOn.png"))
            {
                imgPinPanel.Source = new BitmapImage(new Uri(@"/Icons/PinRemoved.png", UriKind.Relative));
                Settings.Default.SliderPanelPinStateUrlPath = @"/Icons/PinRemoved.png";
            }
        }

        private void Storyboard_Completed(object sender, EventArgs e)
        {
            IsAnimationRunning = false;
        }

        private bool IsAnimationRunning = false;

        private void Storyboard_CurrentTimeInvalidated(object sender, EventArgs e)
        {
            IsAnimationRunning = true;
        }

        private bool bAppStarted = false;

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

            if (!bAppStarted)
                return;

            if (GetRunningProcessCount() > 0)
            {
                var sMessage = "Are you sure you want to change the cpu load settings.";
                sMessage = sMessage + Environment.NewLine + "Running processes will be stoped.";

                MessageBoxResult mbResult = MessageBox.Show(sMessage, "Confirm CPU Setting change", MessageBoxButton.YesNo);
                if (mbResult == MessageBoxResult.Yes)
                {
                    StopAllProcesses();
                    WriteAitoeConfigFiles(sender);
                }
                else if (mbResult == MessageBoxResult.No)
                {
                    return;
                }
            }
            else
            {
                StopAllProcesses();
                WriteAitoeConfigFiles(sender);
            }
            //AddVigilantProcess();
        }

        private void WriteAitoeConfigFiles(object sender)
        {
            var content = ((RadioButton)sender).Content.ToString();

            Settings.Default.CpuLoadThreshold = int.Parse(content);

            foreach (string file in Directory.EnumerateFiles(sVigilantConfigPath, "*.ini"))
            {
                //File.ReadAllText(file);
                List<string> lines = File.ReadLines(file).ToList();

                int line_to_edit = 0;

                for (line_to_edit = 0; line_to_edit < lines.Count(); line_to_edit++)
                {
                    if (lines[line_to_edit].StartsWith("CAP_CPU_LOAD_THRESHOLD"))
                        break;
                }

                line_to_edit = line_to_edit + 1;

                string lineToWrite = "CAP_CPU_LOAD_THRESHOLD=" + content;

                string[] lines1 = File.ReadAllLines(file);

                // Write the new file over the old file.
                using (StreamWriter writer = new StreamWriter(file))
                {
                    for (int currentLine = 1; currentLine <= lines1.Length; ++currentLine)
                    {
                        if (currentLine == line_to_edit)
                        {
                            writer.WriteLine(lineToWrite);
                        }
                        else
                        {
                            writer.WriteLine(lines[currentLine - 1]);
                        }
                    }
                }
            }
        }

        private Visibility _pushbulletConfigurationMessageVisibility;
        public Visibility PushbulletConfigurationMessageVisibility
        {
            get { return _pushbulletConfigurationMessageVisibility; }
            set
            {
                if (_pushbulletConfigurationMessageVisibility == value)
                    return;
                _pushbulletConfigurationMessageVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PushbulletConfigurationMessageVisibility"));
            }
        }

        private bool IsAccessTokenExists()
        {
            string dataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Pushbullet.WPF");
            string accessToken = Path.Combine(dataDirectory, "access_token.bin");
            if (File.Exists(accessToken))
                return true;
            return false;
        }

        private void btnConfigurePushBullet_Click(object sender, RoutedEventArgs e)
        {
            if (IsAccessTokenExists())
            {
                var sMessage = "Pushbullet is already configured. Do you want to reconfigure?";
                sMessage = sMessage + Environment.NewLine + "Choose Yes to delete old and create a new configuration.";
                sMessage = sMessage + Environment.NewLine + "Choose No to continue with the existing configuration.";
                var mbResult = MessageBox.Show(sMessage, "Pushbullet configuration", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (mbResult == MessageBoxResult.No)
                    return;
            }

            SetPushbulletConfigurationMessageVisibility();
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = @"Pushbullet.Cli.exe";
            psi.Arguments = "--rp";
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            using (Process pushBulletWPFProcess = Process.Start(psi))
            {
                pushBulletWPFProcess.WaitForExit();
            }

            SetPushbulletConfigurationMessageVisibility();
        }

        private void SetPushbulletConfigurationMessageVisibility()
        {
            if (IsAccessTokenExists())
                PushbulletConfigurationMessageVisibility = Visibility.Collapsed;
            else
                PushbulletConfigurationMessageVisibility = Visibility.Visible;
        }

        private void ToggleShowHideHeaders()
        {
            if (Settings.Default.IsHeadersHidden)
                btnShowHideHeaders.Content = "Show Headers";
            else           
                btnShowHideHeaders.Content = "Hide Headers";            
        }
    }
}