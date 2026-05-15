using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using EmployeeApp.Models;
using EmployeeApp.Services;

namespace EmployeeApp.ViewModels
{
    public class EmployeeViewModel : INotifyPropertyChanged, IDisposable
    {
        private readonly EmployeeService employeeService = new();
        private ChatService? chatService;
        private ScheduleService? scheduleService;

        private FileSystemWatcher? watcher;
        private DateTime lastWatchedWrite = DateTime.MinValue;

        private string filterDepartment = "Все отделы";
        private string searchText = string.Empty;
        private EmployeeModel? selectedEmployee;
        private string statusMessage = string.Empty;
        private bool isLoading;

        public ObservableCollection<EmployeeModel> Employees { get; } = new();
        public ObservableCollection<EmployeeModel> FilteredEmployees { get; } = new();
        public ObservableCollection<string> FilterOptions { get; } = new();
        public ObservableCollection<string> NotificationLog { get; } = new();

        public RelayCommand AddEmployeeCommand { get; }
        public RelayCommand EditEmployeeCommand { get; }
        public RelayCommand DeleteEmployeeCommand { get; }
        public RelayCommand ReloadCommand { get; }
        public RelayCommand SaveCommand { get; }
        public RelayCommand ExitCommand { get; }
        public RelayCommand OpenChatCommand { get; }
        public RelayCommand PublishScheduleCommand { get; }
        public RelayCommand ToggleAvailabilityCommand { get; }

        public string CurrentUserInfo
        {
            get
            {
                var u = App.CurrentUser;
                return u == null ? "—" : $"{u.Login} ({u.Department})";
            }
        }

        public string FilterDepartment
        {
            get => filterDepartment;
            set
            {
                if (filterDepartment == value) return;
                filterDepartment = value ?? "Все отделы";
                OnPropertyChanged(nameof(FilterDepartment));
                ApplyFilter();
            }
        }

        public string SearchText
        {
            get => searchText;
            set
            {
                if (searchText == value) return;
                searchText = value ?? string.Empty;
                OnPropertyChanged(nameof(SearchText));
                ApplyFilter();
            }
        }

        public EmployeeModel? SelectedEmployee
        {
            get => selectedEmployee;
            set
            {
                selectedEmployee = value;
                OnPropertyChanged(nameof(SelectedEmployee));
                OnPropertyChanged(nameof(HasSelection));
            }
        }

        public bool HasSelection => selectedEmployee != null;

        public string StatusMessage
        {
            get => statusMessage;
            set { statusMessage = value; OnPropertyChanged(nameof(StatusMessage)); }
        }

        public bool IsLoading
        {
            get => isLoading;
            set { isLoading = value; OnPropertyChanged(nameof(IsLoading)); }
        }

        public EmployeeViewModel()
        {
            AddEmployeeCommand = new RelayCommand(AddEmployee);
            EditEmployeeCommand = new RelayCommand(EditEmployee, () => SelectedEmployee != null);
            DeleteEmployeeCommand = new RelayCommand(DeleteEmployee, () => SelectedEmployee != null);
            ReloadCommand = new RelayCommand(async () => await ReloadAsync());
            SaveCommand = new RelayCommand(SaveAll);
            ExitCommand = new RelayCommand(() => Application.Current.Shutdown());
            OpenChatCommand = new RelayCommand(OpenChat);
            PublishScheduleCommand = new RelayCommand(PublishSchedule);
            ToggleAvailabilityCommand = new RelayCommand(p =>
            {
                if (p is EmployeeModel emp) emp.IsAvailable = !emp.IsAvailable;
            });

            var u = App.CurrentUser;
            if (u != null)
            {
                chatService = new ChatService(u.Login, u.Department);
                scheduleService = new ScheduleService();
                scheduleService.NoticeReceived += OnNoticeReceived;
            }

            _ = ReloadAsync();
            SetupWatcher();
        }

        private void OnNoticeReceived(string text)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                NotificationLog.Insert(0, "[Расписание] " + text);
                StatusMessage = "Получено уведомление";
            });
        }

        private async Task ReloadAsync()
        {
            try
            {
                IsLoading = true;
                var list = await employeeService.LoadEmployeesAsync();
                Employees.Clear();
                foreach (var e in list) Employees.Add(e);
                RefreshFilterOptions();
                ApplyFilter();
                StatusMessage = $"Загружено: {Employees.Count}";
            }
            catch (Exception ex)
            {
                App.Log("ReloadAsync failed: " + ex);
                StatusMessage = "Ошибка загрузки: " + ex.Message;
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void RefreshFilterOptions()
        {
            var current = FilterDepartment;
            FilterOptions.Clear();
            FilterOptions.Add("Все отделы");
            foreach (var d in Employees.Select(e => e.Department).Distinct().OrderBy(x => x))
                FilterOptions.Add(d);
            if (FilterOptions.Contains(current)) filterDepartment = current;
            else filterDepartment = "Все отделы";
            OnPropertyChanged(nameof(FilterDepartment));
        }

        public void ApplyFilter()
        {
            FilteredEmployees.Clear();
            IEnumerable<EmployeeModel> q = Employees;
            if (!string.IsNullOrWhiteSpace(filterDepartment) && filterDepartment != "Все отделы")
                q = q.Where(e => e.Department == filterDepartment);
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                var s = searchText.Trim().ToLowerInvariant();
                q = q.Where(e =>
                    e.FullName.ToLowerInvariant().Contains(s) ||
                    e.Position.ToLowerInvariant().Contains(s) ||
                    e.Department.ToLowerInvariant().Contains(s));
            }
            foreach (var e in q) FilteredEmployees.Add(e);
        }

        private void AddEmployee()
        {
            var emp = new EmployeeModel { FullName = "Новый сотрудник", Salary = 0, IsAvailable = true };
            var win = new EmployeeWindow(emp, employeeService) { Owner = Application.Current.MainWindow };
            if (win.ShowDialog() == true)
            {
                Employees.Add(emp);
                SelectedEmployee = emp;
                RefreshFilterOptions();
                ApplyFilter();
                SaveAll();
            }
        }

        private void EditEmployee()
        {
            if (SelectedEmployee == null) return;
            var win = new EmployeeWindow(SelectedEmployee, employeeService) { Owner = Application.Current.MainWindow };
            if (win.ShowDialog() == true)
            {
                RefreshFilterOptions();
                ApplyFilter();
                SaveAll();
            }
        }

        private void DeleteEmployee()
        {
            if (SelectedEmployee == null) return;
            var r = MessageBox.Show($"Удалить сотрудника \"{SelectedEmployee.FullName}\"?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (r != MessageBoxResult.Yes) return;
            Employees.Remove(SelectedEmployee);
            SelectedEmployee = null;
            RefreshFilterOptions();
            ApplyFilter();
            SaveAll();
        }

        private void SaveAll()
        {
            try
            {
                employeeService.SaveEmployees(Employees);
                StatusMessage = $"Сохранено: {DateTime.Now:HH:mm:ss}";
            }
            catch (Exception ex)
            {
                StatusMessage = "Ошибка сохранения: " + ex.Message;
                App.Log("SaveAll failed: " + ex);
            }
        }

        private void OpenChat()
        {
            if (chatService == null)
            {
                MessageBox.Show("Чат недоступен (не выполнен вход).");
                return;
            }
            var deps = new List<string> { "Управление", "Разработка", "Финансы", "Дизайн" };
            foreach (var d in Employees.Select(e => e.Department).Distinct())
                if (!deps.Contains(d)) deps.Add(d);
            var win = new ChatWindow(chatService, deps) { Owner = Application.Current.MainWindow };
            win.Show();
        }

        private void PublishSchedule()
        {
            if (scheduleService == null) return;
            var dlg = new InputDialog("Уведомление о расписании", "Введите текст уведомления:");
            if (dlg.ShowDialog() == true && !string.IsNullOrWhiteSpace(dlg.Result))
            {
                scheduleService.Publish(dlg.Result);
                NotificationLog.Insert(0, $"[Отправлено] {dlg.Result}");
                StatusMessage = "Уведомление отправлено";
            }
        }

        private void SetupWatcher()
        {
            try
            {
                var dir = Path.GetDirectoryName(EmployeeService.EmployeesPath);
                if (string.IsNullOrEmpty(dir) || !Directory.Exists(dir)) return;
                watcher = new FileSystemWatcher(dir, "employees.json")
                {
                    NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size
                };
                watcher.Changed += async (_, _) =>
                {
                    var now = DateTime.Now;
                    if ((now - lastWatchedWrite).TotalMilliseconds < 800) return;
                    lastWatchedWrite = now;
                    await Task.Delay(300);
                    await Application.Current.Dispatcher.InvokeAsync(async () => await ReloadAsync());
                };
                watcher.EnableRaisingEvents = true;
            }
            catch (Exception ex)
            {
                App.Log("SetupWatcher failed: " + ex);
            }
        }

        public void Dispose()
        {
            try { watcher?.Dispose(); } catch { }
            try { chatService?.Dispose(); } catch { }
            try { scheduleService?.Dispose(); } catch { }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
