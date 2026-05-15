using System.Windows;
using System.Windows.Controls;

namespace EmployeeApp
{
    public partial class EmployeeWindow : Window
    {
        public Employee? Result { get; private set; }

        public EmployeeWindow()
        {
            InitializeComponent();
            Title = "Добавление сотрудника";
        }

        public EmployeeWindow(Employee employee) : this()
        {
            Title = "Редактирование сотрудника";
            FullNameBox.Text = employee.FullName;
            SalaryBox.Text = employee.Salary.ToString();

            foreach (object item in PositionBox.Items)
            {
                if (item is ComboBoxItem cbi && (string)cbi.Content == employee.Position)
                {
                    PositionBox.SelectedItem = cbi;
                    break;
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string fullName = FullNameBox.Text.Trim();
            if (string.IsNullOrEmpty(fullName))
            {
                MessageBox.Show("Введите ФИО.");
                return;
            }

            if (PositionBox.SelectedItem is not ComboBoxItem positionItem)
            {
                MessageBox.Show("Выберите должность.");
                return;
            }

            if (!decimal.TryParse(SalaryBox.Text.Trim(), out decimal salary) || salary < 0)
            {
                MessageBox.Show("Введите корректный оклад.");
                return;
            }

            Result = new Employee
            {
                FullName = fullName,
                Position = (string)positionItem.Content,
                Salary = salary
            };

            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
