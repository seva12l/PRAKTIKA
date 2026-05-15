using System.Threading.Tasks;
using System.Windows;
using EmployeeApp.Models;
using EmployeeApp.Services;

namespace EmployeeApp
{
    public partial class EmployeeWindow : Window
    {
        private readonly EmployeeModel employee;
        private readonly EmployeeModel backup;
        private readonly EmployeeService service;

        public EmployeeWindow(EmployeeModel employee, EmployeeService service)
        {
            InitializeComponent();
            this.employee = employee;
            this.service = service;
            backup = new EmployeeModel
            {
                FullName = employee.FullName,
                Position = employee.Position,
                Department = employee.Department,
                Salary = employee.Salary
            };
            DataContext = employee;
            Loaded += async (_, _) => await LoadDepartmentsAsync();
        }

        private async Task LoadDepartmentsAsync()
        {
            var deps = await service.LoadDepartmentsAsync();
            DepartmentBox.ItemsSource = deps;
            if (!string.IsNullOrEmpty(employee.Department))
                DepartmentBox.SelectedValue = employee.Department;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!service.Validate(employee, out string error))
            {
                MessageBox.Show(error);
                return;
            }
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            employee.FullName = backup.FullName;
            employee.Position = backup.Position;
            employee.Department = backup.Department;
            employee.Salary = backup.Salary;
            DialogResult = false;
            Close();
        }
    }
}
