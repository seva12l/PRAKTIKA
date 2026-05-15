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
        private string filterDepartment = "Все";

        public ObservableCollection<Employee> Employees { get; } = new();
        public ObservableCollection<Employee> FilteredEmployees { get; } = new();

        public ObservableCollection<string> Departments { get; } = new()
        {
            "Все", "Управление", "Разработка", "Финансы", "Дизайн"
        };

        public Employee? SelectedEmployee
        {
            get => selectedEmployee;
            set { selectedEmployee = value; OnPropertyChanged(nameof(SelectedEmployee)); }
        }

        public string FilterDepartment
        {
            get => filterDepartment;
            set
            {
                filterDepartment = value;
                OnPropertyChanged(nameof(FilterDepartment));
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
            Employees.Add(new Employee { FullName = "Иванов Иван", Position = "Менеджер", Department = "Управление", Salary = 60000 });
            Employees.Add(new Employee { FullName = "Петров Пётр", Position = "Разработчик", Department = "Разработка", Salary = 95000 });
            Employees.Add(new Employee { FullName = "Сидорова Анна", Position = "Бухгалтер", Department = "Финансы", Salary = 70000 });
            Employees.Add(new Employee { FullName = "Кузнецов Алексей", Position = "Разработчик", Department = "Разработка", Salary = 110000 });
            Employees.Add(new Employee { FullName = "Смирнова Ольга", Position = "Дизайнер", Department = "Дизайн", Salary = 75000 });
        }

        private void ApplyFilter()
        {
            FilteredEmployees.Clear();
            var source = filterDepartment == "Все"
                ? (System.Collections.Generic.IEnumerable<Employee>)Employees
                : Employees.Where(e => e.Department == filterDepartment);
            foreach (var e in source)
                FilteredEmployees.Add(e);
        }

        private void AddEmployee()
        {
            var emp = new Employee();
            var window = new EmployeeWindow(emp, isNew: true) { Owner = Application.Current.MainWindow };
            if (window.ShowDialog() == true)
            {
                Employees.Add(emp);
                ApplyFilter();
            }
        }

        private void EditEmployee()
        {
            if (SelectedEmployee == null) return;
            var window = new EmployeeWindow(SelectedEmployee, isNew: false) { Owner = Application.Current.MainWindow };
            if (window.ShowDialog() == true)
                ApplyFilter();
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
