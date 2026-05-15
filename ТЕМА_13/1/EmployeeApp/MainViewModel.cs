using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace EmployeeApp
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private Employee? selectedEmployee;
        private string filterPosition = "Все";

        public ObservableCollection<Employee> Employees { get; } = new();
        public ObservableCollection<Employee> FilteredEmployees { get; } = new();

        public Employee? SelectedEmployee
        {
            get => selectedEmployee;
            set { selectedEmployee = value; OnPropertyChanged(nameof(SelectedEmployee)); }
        }

        public string FilterPosition
        {
            get => filterPosition;
            set
            {
                filterPosition = value;
                OnPropertyChanged(nameof(FilterPosition));
                ApplyFilter();
            }
        }

        public ICommand AddEmployeeCommand { get; }
        public ICommand EditEmployeeCommand { get; }
        public ICommand DeleteEmployeeCommand { get; }
        public ICommand ExitCommand { get; }

        public MainViewModel()
        {
            AddEmployeeCommand = new RelayCommand(_ => AddEmployee());
            EditEmployeeCommand = new RelayCommand(_ => EditEmployee(), _ => SelectedEmployee != null);
            DeleteEmployeeCommand = new RelayCommand(_ => DeleteEmployee(), _ => SelectedEmployee != null);
            ExitCommand = new RelayCommand(_ => Application.Current.Shutdown());

            LoadSampleData();
            ApplyFilter();
        }

        private void LoadSampleData()
        {
            Employees.Add(new Employee { FullName = "Иванов Иван", Position = "Менеджер", Salary = 60000 });
            Employees.Add(new Employee { FullName = "Петров Пётр", Position = "Разработчик", Salary = 95000 });
            Employees.Add(new Employee { FullName = "Сидорова Анна", Position = "Бухгалтер", Salary = 70000 });
            Employees.Add(new Employee { FullName = "Кузнецов Алексей", Position = "Разработчик", Salary = 110000 });
            Employees.Add(new Employee { FullName = "Смирнова Ольга", Position = "Дизайнер", Salary = 75000 });
        }

        private void ApplyFilter()
        {
            FilteredEmployees.Clear();
            var source = filterPosition == "Все"
                ? Employees
                : (System.Collections.Generic.IEnumerable<Employee>)Employees.Where(e => e.Position == filterPosition);
            foreach (var e in source)
                FilteredEmployees.Add(e);
        }

        private void AddEmployee()
        {
            var window = new EmployeeWindow { Owner = Application.Current.MainWindow };
            if (window.ShowDialog() == true && window.Result != null)
            {
                Employees.Add(window.Result);
                ApplyFilter();
            }
        }

        private void EditEmployee()
        {
            if (SelectedEmployee == null) return;
            var window = new EmployeeWindow(SelectedEmployee) { Owner = Application.Current.MainWindow };
            if (window.ShowDialog() == true && window.Result != null)
            {
                SelectedEmployee.FullName = window.Result.FullName;
                SelectedEmployee.Position = window.Result.Position;
                SelectedEmployee.Salary = window.Result.Salary;
                ApplyFilter();
            }
        }

        private void DeleteEmployee()
        {
            if (SelectedEmployee == null) return;
            var result = MessageBox.Show(
                "Удалить сотрудника \"" + SelectedEmployee.FullName + "\"?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Employees.Remove(SelectedEmployee);
                SelectedEmployee = null;
                ApplyFilter();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
