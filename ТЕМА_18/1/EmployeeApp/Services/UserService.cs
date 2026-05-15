using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using EmployeeApp.Models;

namespace EmployeeApp.Services
{
    public class UserService
    {
        private static readonly string DataDir =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
        private static readonly string FilePath =
            Path.Combine(DataDir, "users.json");

        private List<UserModel> users = new();

        public UserService()
        {
            Load();
        }

        private void Load()
        {
            try
            {
                Directory.CreateDirectory(DataDir);
                if (File.Exists(FilePath))
                {
                    var json = File.ReadAllText(FilePath);
                    users = JsonSerializer.Deserialize<List<UserModel>>(json) ?? new List<UserModel>();
                }
            }
            catch (Exception ex)
            {
                App.Log("UserService.Load failed: " + ex);
                users = new List<UserModel>();
            }

            if (users.Count == 0)
            {
                users.Add(new UserModel
                {
                    Login = "admin",
                    PasswordHash = Hash("admin"),
                    Department = "Управление"
                });
                Save();
            }
        }

        private void Save()
        {
            try
            {
                Directory.CreateDirectory(DataDir);
                var opts = new JsonSerializerOptions { WriteIndented = true };
                File.WriteAllText(FilePath, JsonSerializer.Serialize(users, opts));
            }
            catch (Exception ex)
            {
                App.Log("UserService.Save failed: " + ex);
            }
        }

        public UserModel? Authenticate(string login, string password)
        {
            if (string.IsNullOrWhiteSpace(login) || password == null) return null;
            var hash = Hash(password);
            return users.FirstOrDefault(u =>
                string.Equals(u.Login, login, StringComparison.OrdinalIgnoreCase) &&
                u.PasswordHash == hash);
        }

        public bool Register(string login, string password, string department, out string error)
        {
            error = string.Empty;
            if (string.IsNullOrWhiteSpace(login)) { error = "Введите логин."; return false; }
            if (string.IsNullOrWhiteSpace(password) || password.Length < 3)
            { error = "Пароль должен быть не короче 3 символов."; return false; }
            if (users.Any(u => string.Equals(u.Login, login, StringComparison.OrdinalIgnoreCase)))
            { error = "Пользователь с таким логином уже существует."; return false; }
            users.Add(new UserModel
            {
                Login = login,
                PasswordHash = Hash(password),
                Department = string.IsNullOrWhiteSpace(department) ? "Управление" : department
            });
            Save();
            return true;
        }

        private static string Hash(string text)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(text));
            var sb = new StringBuilder(bytes.Length * 2);
            foreach (var b in bytes) sb.Append(b.ToString("x2"));
            return sb.ToString();
        }
    }
}
