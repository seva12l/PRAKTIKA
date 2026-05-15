using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using EmployeeApp.Models;

namespace EmployeeApp.Services
{
    /// <summary>
    /// Локальная регистрация и аутентификация. Учётные записи хранятся в users.json,
    /// пароли — в виде SHA-256-хэша.
    /// </summary>
    public class UserService
    {
        public const string FileName = "users.json";

        private readonly List<UserModel> users;

        public UserService()
        {
            users = JsonStore.Load<UserModel>(FileName);
            if (users.Count == 0)
            {
                // Стандартная учётка администратора при первом запуске.
                users.Add(new UserModel
                {
                    Login = "admin",
                    PasswordHash = Hash("admin"),
                    Department = "Управление"
                });
                JsonStore.Save(FileName, users);
            }
        }

        public bool Register(string login, string password, string department, out string error)
        {
            if (string.IsNullOrWhiteSpace(login)) { error = "Введите логин."; return false; }
            if (string.IsNullOrWhiteSpace(password) || password.Length < 3)
            {
                error = "Пароль должен быть не короче 3 символов."; return false;
            }
            if (users.Any(u => u.Login.Equals(login, System.StringComparison.OrdinalIgnoreCase)))
            {
                error = "Пользователь с таким логином уже существует."; return false;
            }
            users.Add(new UserModel
            {
                Login = login,
                PasswordHash = Hash(password),
                Department = department
            });
            JsonStore.Save(FileName, users);
            error = string.Empty;
            return true;
        }

        public UserModel? Authenticate(string login, string password)
        {
            var hash = Hash(password);
            return users.FirstOrDefault(u =>
                u.Login.Equals(login, System.StringComparison.OrdinalIgnoreCase) &&
                u.PasswordHash == hash);
        }

        private static string Hash(string text)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(text));
            var sb = new StringBuilder();
            foreach (var b in bytes) sb.Append(b.ToString("x2"));
            return sb.ToString();
        }
    }
}
