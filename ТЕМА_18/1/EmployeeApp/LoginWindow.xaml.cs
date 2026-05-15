using System;
using System.Windows;
using System.Windows.Controls;
using EmployeeApp.Models;
using EmployeeApp.Services;

namespace EmployeeApp
{
    public partial class LoginWindow : Window
    {
        private readonly UserService userService;

        public UserModel? AuthenticatedUser { get; private set; }

        public LoginWindow()
        {
            InitializeComponent();
            userService = new UserService();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var user = userService.Authenticate(LoginBox.Text.Trim(), PasswordBox.Password);
                if (user == null)
                {
                    SetError("Неверный логин или пароль.");
                    return;
                }
                AuthenticatedUser = user;
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                SetError("Ошибка входа: " + ex.Message);
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dep = (DepartmentBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Управление";
                var login = LoginBox.Text.Trim();
                var pwd = PasswordBox.Password;
                App.Log($"Register click: login='{login}', dep='{dep}', pwdLen={pwd.Length}");

                if (!userService.Register(login, pwd, dep, out string error))
                {
                    App.Log("Register validation failed: " + error);
                    SetError(error);
                    return;
                }
                AuthenticatedUser = userService.Authenticate(login, pwd);
                if (AuthenticatedUser == null)
                {
                    App.Log("Register OK but auto-login failed");
                    SetError("Регистрация прошла, но войти не удалось. Введите данные вручную.");
                    return;
                }
                App.Log("Register OK, auto-login success: " + login);
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                App.Log("RegisterButton_Click exception: " + ex);
                SetError("Ошибка регистрации: " + ex.Message);
            }
        }

        private void SetError(string text)
        {
            ErrorText.Foreground = System.Windows.Media.Brushes.Crimson;
            ErrorText.Text = text;
        }
    }
}
