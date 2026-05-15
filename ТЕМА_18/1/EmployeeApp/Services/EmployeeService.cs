using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using EmployeeApp.Models;

namespace EmployeeApp.Services
{
    public class EmployeeService
    {
        private static readonly string DataDir =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
        public static readonly string EmployeesPath =
            Path.Combine(DataDir, "employees.json");
        private static readonly string DepartmentsPath =
            Path.Combine(DataDir, "departments.json");

        private static readonly JsonSerializerOptions JsonOpts = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        };

        public async Task<List<EmployeeModel>> LoadEmployeesAsync()
        {
            return await Task.Run(() =>
            {
                Directory.CreateDirectory(DataDir);
                if (!File.Exists(EmployeesPath))
                {
                    var seed = SeedEmployees();
                    SaveEmployees(seed);
                    return seed;
                }
                try
                {
                    var json = File.ReadAllText(EmployeesPath);
                    return JsonSerializer.Deserialize<List<EmployeeModel>>(json) ?? new List<EmployeeModel>();
                }
                catch (Exception ex)
                {
                    App.Log("EmployeeService.LoadEmployeesAsync failed: " + ex);
                    return new List<EmployeeModel>();
                }
            });
        }

        public void SaveEmployees(IEnumerable<EmployeeModel> employees)
        {
            try
            {
                Directory.CreateDirectory(DataDir);
                File.WriteAllText(EmployeesPath, JsonSerializer.Serialize(employees, JsonOpts));
            }
            catch (Exception ex)
            {
                App.Log("EmployeeService.SaveEmployees failed: " + ex);
            }
        }

        public async Task<List<DepartmentModel>> LoadDepartmentsAsync()
        {
            return await Task.Run(() =>
            {
                Directory.CreateDirectory(DataDir);
                if (!File.Exists(DepartmentsPath))
                {
                    var seed = new List<DepartmentModel>
                    {
                        new() { Name = "Управление" },
                        new() { Name = "Разработка" },
                        new() { Name = "Финансы" },
                        new() { Name = "Дизайн" }
                    };
                    File.WriteAllText(DepartmentsPath, JsonSerializer.Serialize(seed, JsonOpts));
                    return seed;
                }
                try
                {
                    var json = File.ReadAllText(DepartmentsPath);
                    return JsonSerializer.Deserialize<List<DepartmentModel>>(json) ?? new List<DepartmentModel>();
                }
                catch (Exception ex)
                {
                    App.Log("EmployeeService.LoadDepartmentsAsync failed: " + ex);
                    return new List<DepartmentModel>();
                }
            });
        }

        public bool Validate(EmployeeModel emp, out string error)
        {
            error = string.Empty;
            if (emp == null) { error = "Сотрудник не задан."; return false; }
            if (string.IsNullOrWhiteSpace(emp.FullName)) { error = "Введите ФИО."; return false; }
            if (string.IsNullOrWhiteSpace(emp.Position)) { error = "Выберите должность."; return false; }
            if (string.IsNullOrWhiteSpace(emp.Department)) { error = "Выберите отдел."; return false; }
            if (emp.Salary < 0) { error = "Оклад не может быть отрицательным."; return false; }
            return true;
        }

        private static List<EmployeeModel> SeedEmployees()
        {
            return new List<EmployeeModel>
            {
                new() { FullName = "Иванов Иван", Position = "Менеджер", Department = "Управление", Salary = 60000, IsAvailable = true },
                new() { FullName = "Петров Пётр", Position = "Разработчик", Department = "Разработка", Salary = 95000, IsAvailable = true },
                new() { FullName = "Сидорова Анна", Position = "Бухгалтер", Department = "Финансы", Salary = 70000, IsAvailable = false },
                new() { FullName = "Кузнецов Алексей", Position = "Разработчик", Department = "Разработка", Salary = 110000, IsAvailable = true },
                new() { FullName = "Смирнова Ольга", Position = "Дизайнер", Department = "Дизайн", Salary = 75000, IsAvailable = false }
            };
        }
    }
}
