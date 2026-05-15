using System.Windows;

namespace EmployeeApp
{
    public partial class EmployeeWindow : Window
    {
        private readonly Employee employee;
        private readonly Employee backup;
        private readonly bool isNew;

        public EmployeeWindow(Employee employee, bool isNew)
        {
            InitializeComponent();
            this.employee = employee;
            this.isNew = isNew;
            // Резервная копия для отмены изменений
            backup = new Employee
            {
                FullName = employee.FullName,
                Position = employee.Position,
                Department = employee.Department,
                Salary = employee.Salary
            };
            DataContext = employee;
            Title = isNew ? "Добавление сотрудника" : "Редактирование сотрудника";
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(employee.FullName))
            {
                MessageBox.Show("Введите ФИО.");
                return;
            }
            if (string.IsNullOrWhiteSpace(employee.Position))
            {
                MessageBox.Show("Выберите должность.");
                return;
            }
            if (string.IsNullOrWhiteSpace(employee.Department))
            {
                MessageBox.Show("Выберите отдел.");
                return;
            }
            if (employee.Salary < 0)
            {
                MessageBox.Show("Оклад не может быть отрицательным.");
                return;
            }

            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Откат изменений (TwoWay-биндинги уже применили их к объекту)
            employee.FullName = backup.FullName;
            employee.Position = backup.Position;
            employee.Department = backup.Department;
            employee.Salary = backup.Salary;
            DialogResult = false;
            Close();
        }
    }
}
