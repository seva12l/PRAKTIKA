using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using EmployeeApp.Models;
using EmployeeApp.Services;

namespace EmployeeApp.ViewModels
{
    public class EmployeeViewModel : INotifyPropertyChanged, IDisposable
    {
        public EmployeeService Service { get; } = new();
        public ChatService Chat { get; }
        public NotificationService? Notifications { get; }

        private FileSystemWatcher? watcher;

        private EmployeeModel? selectedEmployee;
        private string filterDepartment = "Все";
        private bool isLoading;
        private string statusMessage = string.Empty;
        private string currentUserInfo = string.Empty;

        public ObservableCollection<EmployeeModel> Employees { get; } = new();
        public ObservableCollection<EmployeeModel> FilteredEmployees { get; } = new();
        public ObservableCollection<DepartmentModel> Departments { get; } = new();
        public ObservableCollection<string> FilterOptions { get; } = new() { "Все" };
        public ObservableCollection<string> NotificationLog { get; } = new();

        public EmployeeModel? SelectedEmployee
        {
            get => selectedEmployee;
            set { selectedEmployee = value; OnPropertyChanged(nameof(SelectedEmployee)); }
        }

        public string FilterDepartment
        {
            get => filterDepartment;
            set { filterDepartment = value; OnPropertyChanged(nameof(FilterDepartment)); ApplyFilter(); }
        }

        public bool IsLoading
        {
            get => isLoading;
            set { isLoading = value; OnPropertyChanged(nameof(IsLoading)); }
        }

        public string StatusMessage
        {
            get => statusMessage;
            set { statusMessage = value; OnPropertyChanged(nameof(StatusMessage)); }
        }

        public string CurrentUserInfo
        {
            get => currentUserInfo;
            set { currentUserInfo = value; OnPropertyChanged(nameof(CurrentUserInfo)); }
        }

        public ICommand AddEmployeeCommand { get; }
        public ICommand EditEmployeeCommand { get; }
        public ICommand DeleteEmployeeCommand { get; }
        public ICommand ReloadCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand OpenChatCommand { get; }
        public ICommand PublishScheduleCommand { get; }
        public ICommand ExitCommand { get; }

        public EmployeeViewModel()
        {
            var user = App.CurrentUser;
            CurrentUserInfo = user == null
                ? "Гость"
                : $"{user.Login} ({user.Department})";

            Chat = new ChatService(user?.Login ?? "user", user?.Department ?? "Управление");
            try { Chat.Start(); }
            catch (Exception ex) { StatusMessage = "Чат не запущен: " + ex.Message; }

            try
            {
                Notifications = new NotificationService();
                Notifications.NotificationReceived += OnNotificationReceived;
                Notifications.StartListening();
            }
            catch (Exception ex)
            {
                StatusMessage = "Уведомления недоступны: " + ex.Message;
            }

            AddEmployeeCommand = new RelayCommand(_ => AddEmployee());
            EditEmployeeCommand = new RelayCommand(_ => EditEmployee(), _ => SelectedEmployee != null);
            DeleteEmployeeCommand = new RelayCommand(_ => DeleteEmployee(), _ => SelectedEmployee != null);
            ReloadCommand = new RelayCommand(async _ => await LoadAsync());
            SaveCommand = new RelayCommand(async _ => await SaveAsync());
            OpenChatCommand = new RelayCommand(_ => OpenChat());
            PublishScheduleCommand = new RelayCommand(_ => PublishSchedule());
            ExitCommand = new RelayCommand(_ => Application.Current.Shutdown());

            _ = LoadAsync();
        }

        public async Task LoadAsync()
        {
            IsLoading = true;
            try
            {
                var deps = await Service.LoadDepartmentsAsync();
                Departments.Clear();
                FilterOptions.Clear();
                FilterOptions.Add("Все");
                foreach (var d in deps)
                {
                    Departments.Add(d);
                    FilterOptions.Add(d.Name);
                }

                var loaded = await Service.LoadEmployeesAsync();
                Employees.Clear();
                foreach (var e in loaded) Employees.Add(e);

                ApplyFilter();
                StatusMessage = $"Загружено из {EmployeeService.FileName}: {Employees.Count}";

                StartWatcher();
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task SaveAsync()
        {
            // Не реагируем на собственную запись через FileSystemWatcher.
            if (watcher != null) watcher.EnableRaisingEvents = false;
            try
            {
                await Service.SaveEmployeesAsync(Employees);
                StatusMessage = $"Сохранено в {EmployeeService.FileName}: {Employees.Count}";
            }
            finally
            {
                await Task.Delay(200);
                if (watcher != null) watcher.EnableRaisingEvents = true;
            }
        }

        private void StartWatcher()
        {
            if (watcher != null) return;
            watcher = new FileSystemWatcher(JsonStore.DataDir, EmployeeService.FileName)
            {
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size,
                EnableRaisingEvents = true
            };
            watcher.Changed += async (_, _) =>
            {
                // Debounce: дать ОС закончить запись.
                await Task.Delay(300);
                await Application.Current.Dispatcher.InvokeAsync(async () =>
                {
                    StatusMessage = "Изменение employees.json — перечитываем...";
                    await LoadAsync();
                });
            };
        }

        private void ApplyFilter()
        {
            FilteredEmployees.Clear();
            foreach (var e in Service.Filter(Employees, filterDepartment))
                FilteredEmployees.Add(e);
        }

        private async void AddEmployee()
        {
            var emp = new EmployeeModel();
            var window = new EmployeeWindow(emp, Service) { Owner = Application.Current.MainWindow };
            if (window.ShowDialog() == true)
            {
                Employees.Add(emp);
                ApplyFilter();
                await SaveAsync();
            }
        }

        private async void EditEmployee()
        {
            if (SelectedEmployee == null) return;
            var window = new EmployeeWindow(SelectedEmployee, Service) { Owner = Application.Current.MainWindow };
            if (window.ShowDialog() == true)
            {
                ApplyFilter();
                await SaveAsync();
            }
        }

        private async void DeleteEmployee()
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
                await SaveAsync();
            }
        }

        private void OpenChat()
        {
            var deps = new List<string>();
            foreach (var d in Departments) deps.Add(d.Name);
            var win = new ChatWindow(Chat, deps) { Owner = Application.Current.MainWindow };
            win.Show();
        }

        private void PublishSchedule()
        {
            var win = new ScheduleWindow { Owner = Application.Current.MainWindow };
            if (win.ShowDialog() == true && !string.IsNullOrWhiteSpace(win.NotificationText))
            {
                var user = App.CurrentUser;
                var text = $"[{DateTime.Now:HH:mm:ss}] {user?.Login}: {win.NotificationText}";
                if (Notifications != null)
                {
                    Notifications.Publish(text);
                    NotificationLog.Add(text + "  (отправлено)");
                }
                else
                {
                    NotificationLog.Add(text + "  (локально, MMF недоступен)");
                }
            }
        }

        private void OnNotificationReceived(string text)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                NotificationLog.Add(text);
                StatusMessage = "Получено уведомление о расписании";
            });
        }

        public void Dispose()
        {
            try { Chat.Stop(); } catch { }
            try { Notifications?.Dispose(); } catch { }
            try { watcher?.Dispose(); } catch { }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
