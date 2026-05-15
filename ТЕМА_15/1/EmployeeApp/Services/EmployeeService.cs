using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeApp.Models;

namespace EmployeeApp.Services
{
    /// <summary>
    /// Бизнес-логика учёта сотрудников. Вынесена из UI согласно принципам MVVM.
    /// В реальном приложении здесь была бы работа с БД или сетевым API.
    /// </summary>
    public class EmployeeService
    {
        public async Task<List<EmployeeModel>> LoadEmployeesAsync()
        {
            return await Task.Run(() =>
            {
                // Имитация долгой операции (загрузка из БД / API)
                System.Threading.Thread.Sleep(1500);

                return new List<EmployeeModel>
                {
                    new() { FullName = "Иванов Иван", Position = "Менеджер", Department = "Управление", Salary = 60000 },
                    new() { FullName = "Петров Пётр", Position = "Разработчик", Department = "Разработка", Salary = 95000 },
                    new() { FullName = "Сидорова Анна", Position = "Бухгалтер", Department = "Финансы", Salary = 70000 },
                    new() { FullName = "Кузнецов Алексей", Position = "Разработчик", Department = "Разработка", Salary = 110000 },
                    new() { FullName = "Смирнова Ольга", Position = "Дизайнер", Department = "Дизайн", Salary = 75000 }
                };
            });
        }

        public async Task<List<DepartmentModel>> LoadDepartmentsAsync()
        {
            return await Task.Run(() =>
            {
                System.Threading.Thread.Sleep(300);
                return new List<DepartmentModel>
                {
                    new() { Name = "Управление" },
                    new() { Name = "Разработка" },
                    new() { Name = "Финансы" },
                    new() { Name = "Дизайн" }
                };
            });
        }

        public IEnumerable<EmployeeModel> Filter(IEnumerable<EmployeeModel> source, string department)
        {
            if (string.IsNullOrEmpty(department) || department == "Все")
                return source;
            return source.Where(e => e.Department == department);
        }

        public bool Validate(EmployeeModel emp, out string error)
        {
            if (string.IsNullOrWhiteSpace(emp.FullName))
            {
                error = "Введите ФИО.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(emp.Position))
            {
                error = "Выберите должность.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(emp.Department))
            {
                error = "Выберите отдел.";
                return false;
            }
            if (emp.Salary < 0)
            {
                error = "Оклад не может быть отрицательным.";
                return false;
            }
            error = string.Empty;
            return true;
        }
    }
}
