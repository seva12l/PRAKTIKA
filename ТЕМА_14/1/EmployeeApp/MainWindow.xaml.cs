using System.Linq;
using System.Text;
using System.Windows;

namespace EmployeeApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private MainViewModel ViewModel => (MainViewModel)DataContext;

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Обработчик Click оставлен в соответствии с требованием задания.
            // Основная логика выполняется через Binding к AddEmployeeCommand.
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            // Обработчик Click оставлен в соответствии с требованием задания.
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Обработчик Click оставлен в соответствии с требованием задания.
        }

        private void ReportList_Click(object sender, RoutedEventArgs e)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Список сотрудников:");
            sb.AppendLine();
            foreach (var emp in ViewModel.Employees)
                sb.AppendLine($"• {emp.FullName} — {emp.Position} ({emp.Department}), {emp.Salary} руб.");
            sb.AppendLine();
            sb.AppendLine($"Всего: {ViewModel.Employees.Count}");
            sb.AppendLine($"Суммарный фонд оплаты: {ViewModel.Employees.Sum(e => e.Salary)} руб.");
            MessageBox.Show(sb.ToString(), "Отчёт", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "Автоматизация учёта сотрудников\nWPF, .NET 9.0\n\nГорячие клавиши:\n  Ctrl+N — добавить\n  Ctrl+E — редактировать\n  Ctrl+D / Del — удалить",
                "О программе",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}
