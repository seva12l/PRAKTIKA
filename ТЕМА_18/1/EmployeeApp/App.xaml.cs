using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using EmployeeApp.Models;

namespace EmployeeApp;

public partial class App : Application
{
    public static UserModel? CurrentUser { get; private set; }

    private static readonly string LogPath = Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory, "error.log");

    public static void Log(string text)
    {
        try
        {
            File.AppendAllText(LogPath,
                $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {text}\n");
        }
        catch { }
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        ShutdownMode = ShutdownMode.OnExplicitShutdown;

        string lastErrorKey = string.Empty;
        DateTime lastErrorTime = DateTime.MinValue;
        DispatcherUnhandledException += (s, ev) =>
        {
            Log("UI thread exception: " + ev.Exception);
            var key = ev.Exception.GetType().FullName + ":" + ev.Exception.Message;
            var now = DateTime.UtcNow;
            if (key != lastErrorKey || (now - lastErrorTime).TotalSeconds > 3)
            {
                lastErrorKey = key;
                lastErrorTime = now;
                MessageBox.Show("Произошла ошибка:\n" + ev.Exception.Message,
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            ev.Handled = true;
        };
        AppDomain.CurrentDomain.UnhandledException += (s, ev) =>
            Log("AppDomain unhandled: " + ev.ExceptionObject);
        System.Threading.Tasks.TaskScheduler.UnobservedTaskException += (s, ev) =>
        {
            Log("Unobserved task: " + ev.Exception);
            ev.SetObserved();
        };

        if (!Resources.Contains("BoolToVis"))
            Resources.Add("BoolToVis", new BooleanToVisibilityConverter());

        Log("App starting. Data dir = " + AppDomain.CurrentDomain.BaseDirectory);

        try
        {
            var login = new LoginWindow();
            var dlg = login.ShowDialog();
            Log($"LoginWindow closed. DialogResult={dlg}, AuthenticatedUser={login.AuthenticatedUser?.Login}");

            if (dlg != true || login.AuthenticatedUser == null)
            {
                Shutdown();
                return;
            }

            CurrentUser = login.AuthenticatedUser;
            Log("Creating MainWindow for user: " + CurrentUser.Login);

            MainWindow main;
            try
            {
                main = new MainWindow();
            }
            catch (Exception ex)
            {
                Log("MainWindow ctor failed: " + ex);
                MessageBox.Show("Не удалось создать главное окно:\n\n" + ex,
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
                return;
            }

            MainWindow = main;
            ShutdownMode = ShutdownMode.OnMainWindowClose;
            main.Show();
            Log("MainWindow shown");
        }
        catch (Exception ex)
        {
            Log("OnStartup fatal: " + ex);
            MessageBox.Show("Не удалось запустить приложение:\n\n" + ex,
                "Ошибка запуска", MessageBoxButton.OK, MessageBoxImage.Error);
            Shutdown();
        }
    }
}
