using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using EmployeeApp.Models;
using EmployeeApp.Services;

namespace EmployeeApp.ViewModels
{
    public class EmployeeViewModel : INotifyPropertyChanged
    {
        private readonly EmployeeService service = new();

        private EmployeeModel? selectedEmployee;
        private string filterDepartment = "Все";
        private bool isLoading;

        public ObservableCollection<EmployeeModel> Employees { get; } = new();
        public ObservableCollection<EmployeeModel> FilteredEmployees { get; } = new();
        public ObservableCollection<DepartmentModel> Departments { get; } = new();
        public ObservableCollection<string> FilterOptions { get; } = new() { "Все" };

        public EmployeeModel? SelectedEmployee
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

        public bool IsLoading
        {
            get => isLoading;
            set { isLoading = value; OnPropertyChanged(nameof(IsLoading)); }
        }

        public ICommand AddEmployeeCommand { get; }
        public ICommand EditEmployeeCommand { get; }
        public ICommand DeleteEmployeeCommand { get; }
        public ICommand ReloadCommand { get; }
        public ICommand ExitCommand { get; }

        public EmployeeViewModel()
        {
            AddEmployeeCommand = new RelayCommand(_ => AddEmployee());
            EditEmployeeCommand = new RelayCommand(_ => EditEmployee(), _ => SelectedEmployee != null);
            DeleteEmployeeCommand = new RelayCommand(_ => DeleteEmployee(), _ => SelectedEmployee != null);
            ReloadCommand = new RelayCommand(async _ => await LoadAsync());
            ExitCommand = new RelayCommand(_ => Application.Current.Shutdown());

            _ = LoadAsync();
        }

        public async Task LoadAsync()
        {
            IsLoading = true;
            try
            {
                var deps = await service.LoadDepartmentsAsync();
                Departments.Clear();
                FilterOptions.Clear();
                FilterOptions.Add("Все");
                foreach (var d in deps)
                {
                    Departments.Add(d);
                    FilterOptions.Add(d.Name);
                }

                var loaded = await service.LoadEmployeesAsync();
                Employees.Clear();
                foreach (var e in loaded)
                    Employees.Add(e);

                ApplyFilter();
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void ApplyFilter()
        {
            FilteredEmployees.Clear();
            foreach (var e in service.Filter(Employees, filterDepartment))
                FilteredEmployees.Add(e);
        }

        private void AddEmployee()
        {
            var emp = new EmployeeModel();
            var window = new EmployeeWindow(emp, service) { Owner = Application.Current.MainWindow };
            if (window.ShowDialog() == true)
            {
                Employees.Add(emp);
                ApplyFilter();
            }
        }

        private void EditEmployee()
        {
            if (SelectedEmployee == null) return;
            var window = new EmployeeWindow(SelectedEmployee, service) { Owner = Application.Current.MainWindow };
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
