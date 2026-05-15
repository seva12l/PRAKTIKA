using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace EmployeeApp
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<Employee> employees = new();

        public MainWindow()
        {
            InitializeComponent();
            LoadEmployees();
            ApplyFilter();
        }

        private void LoadEmployees()
        {
            employees.Add(new Employee { FullName = "Иванов Иван", Position = "Менеджер", Salary = 60000 });
            employees.Add(new Employee { FullName = "Петров Пётр", Position = "Разработчик", Salary = 95000 });
            employees.Add(new Employee { FullName = "Сидорова Анна", Position = "Бухгалтер", Salary = 70000 });
            employees.Add(new Employee { FullName = "Кузнецов Алексей", Position = "Разработчик", Salary = 110000 });
            employees.Add(new Employee { FullName = "Смирнова Ольга", Position = "Дизайнер", Salary = 75000 });
        }

        private void ApplyFilter()
        {
            if (EmployeeListBox == null)
                return;

            string? selected = GetSelectedPosition();

            if (selected == null)
            {
                EmployeeListBox.ItemsSource = employees;
            }
            else
            {
                EmployeeListBox.ItemsSource = employees.Where(e => e.Position == selected).ToList();
            }
        }

        private string? GetSelectedPosition()
        {
            if (FilterManager.IsChecked == true) return "Менеджер";
            if (FilterDeveloper.IsChecked == true) return "Разработчик";
            if (FilterAccountant.IsChecked == true) return "Бухгалтер";
            if (FilterDesigner.IsChecked == true) return "Дизайнер";
            return null;
        }

        private void Filter_Checked(object sender, RoutedEventArgs e)
        {
            ApplyFilter();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            EmployeeWindow window = new EmployeeWindow();
            window.Owner = this;
            if (window.ShowDialog() == true && window.Result != null)
            {
                employees.Add(window.Result);
                ApplyFilter();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeeListBox.SelectedItem is not Employee selected)
            {
                MessageBox.Show("Выберите сотрудника.");
                return;
            }

            EmployeeWindow window = new EmployeeWindow(selected);
            window.Owner = this;
            if (window.ShowDialog() == true && window.Result != null)
            {
                selected.FullName = window.Result.FullName;
                selected.Position = window.Result.Position;
                selected.Salary = window.Result.Salary;
                ApplyFilter();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeeListBox.SelectedItem is not Employee selected)
            {
                MessageBox.Show("Выберите сотрудника.");
                return;
            }

            if (MessageBox.Show("Удалить сотрудника \"" + selected.FullName + "\"?",
                                "Подтверждение",
                                MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                employees.Remove(selected);
                ApplyFilter();
            }
        }
    }
}
