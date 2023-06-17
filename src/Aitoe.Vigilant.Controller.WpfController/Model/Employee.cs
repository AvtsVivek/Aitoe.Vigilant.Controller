using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;

namespace Aitoe.Vigilant.Controller.WpfController.Model
{
    public class Employee : ObservableObject
    {
        private int id;
        private string name;
        private int age;
        private decimal salary;
                
        public int ID
        {
            get
            {
                return id; 
            }
            set
            {
                Set<int>(() => this.ID, ref id, value);
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                Set<string>(() => this.Name, ref name, value);
            }
        }

        public int Age
        {
            get
            {
                return age;
            }
            set
            {
                Set<int>(() => this.Age, ref age, value);
            }
        }

        public decimal Salary
        {
            get
            {
                return salary;
            }
            set
            {
                Set<decimal>(() => this.Salary, ref salary, value);
            }
        }

        public static ObservableCollection<Employee> GetSampleEmployees()
        {
            ObservableCollection<Employee> employees = new ObservableCollection<Employee>();
            for (int i = 0; i < 30; ++i)
            {
                employees.Add(new Employee
                {
                    ID = i + 1,
                    Name = "Name " + (i + 1).ToString(),
                    Age = 20 + i,
                    Salary = 20000 + (i * 10)
                });
            }
            return employees;
        }
    }
}
