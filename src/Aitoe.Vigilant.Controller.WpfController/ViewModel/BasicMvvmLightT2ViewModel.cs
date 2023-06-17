//using Aitoe.Vigilant.Controller.WpfController.Model;
//using GalaSoft.MvvmLight;
//using GalaSoft.MvvmLight.Command;
//using GalaSoft.MvvmLight.Messaging;
//using System.Collections.ObjectModel;
//using System.Windows.Input;

//namespace Aitoe.Vigilant.Controller.WpfController.ViewModel
//{
//    public class BasicMvvmLightT2ViewModel : ViewModelBase
//    {
//        //http://dotnetpattern.com/mvvm-light-toolkit-example
//        private ObservableCollection<Employee> employees;
//        private Employee selectedEmployee;

//        public BasicMvvmLightT2ViewModel()
//        {
//            LoadEmployeesCommand = new RelayCommand(LoadEmployeesMethod);
//            SaveEmployeesCommand = new RelayCommand(SaveEmployeesMethod);
//        }

//        public ICommand LoadEmployeesCommand { get; private set; }
//        public ICommand SaveEmployeesCommand { get; private set; }

//        public ObservableCollection<Employee> EmployeeList
//        {
//            get
//            {
//                return employees;
//            }
//        }

//        public Employee SelectedEmployee
//        {
//            get
//            {
//                return selectedEmployee;
//            }
//            set
//            {
//                selectedEmployee = value;
//                RaisePropertyChanged("SelectedEmployee");
//            }
//        }

//        public void SaveEmployeesMethod()
//        {
//            Messenger.Default.Send<NotificationMessage>(new NotificationMessage("Employees Saved."));
//        }

//        private void LoadEmployeesMethod()
//        {
//            employees = Employee.GetSampleEmployees();
//            this.RaisePropertyChanged(() => this.EmployeeList);
//            Messenger.Default.Send<NotificationMessage>(new NotificationMessage("Employees Loaded."));
//        }
//    }
//}
