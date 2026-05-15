using System.Linq;
using System.Text;
using System.Windows;
using EmployeeApp.ViewModels;

namespace EmployeeApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Closed += (_, _) => ViewModel.Dispose();
        }

        private EmployeeViewModel ViewModel => (EmployeeViewModel)DataContext;

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
                "Автоматизация учёта сотрудников\nWPF, .NET 9.0, MVVM\n\n" +
                "Хранение: employees.json, users.json (Data/)\n" +
                "Чат между отделами: Named Pipes\n" +
                "Уведомления: Memory-Mapped Files\n" +
                "Отслеживание изменений: FileSystemWatcher\n\n" +
                "Горячие клавиши:\n  Ctrl+N — добавить\n  Ctrl+E — редактировать\n" +
                "  Ctrl+D / Del — удалить\n  Ctrl+S — сохранить\n  F5 — перезагрузить",
                "О программе",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}
