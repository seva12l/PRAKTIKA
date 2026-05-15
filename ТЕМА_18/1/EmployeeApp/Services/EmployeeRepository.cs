using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeApp.Data;
using EmployeeApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApp.Services
{
    public class EmployeeRepository
    {
        private readonly AppDbContext context;

        public EmployeeRepository(AppDbContext context)
        {
            this.context = context;
            this.context.Database.EnsureCreated();
            SeedIfEmpty();
        }

        private void SeedIfEmpty()
        {
            if (context.Employees.Any()) return;
            var seed = new[]
            {
                new EmployeeModel { FullName = "Иванов Иван", Position = "Менеджер", Department = "Управление", Salary = 60000, IsAvailable = true },
                new EmployeeModel { FullName = "Петров Пётр", Position = "Разработчик", Department = "Разработка", Salary = 95000, IsAvailable = true },
                new EmployeeModel { FullName = "Сидорова Анна", Position = "Бухгалтер", Department = "Финансы", Salary = 70000, IsAvailable = false },
                new EmployeeModel { FullName = "Кузнецов Алексей", Position = "Разработчик", Department = "Разработка", Salary = 110000, IsAvailable = true },
                new EmployeeModel { FullName = "Смирнова Ольга", Position = "Дизайнер", Department = "Дизайн", Salary = 75000, IsAvailable = false }
            };
            context.Employees.AddRange(seed);
            context.SaveChanges();
        }

        public async Task<List<EmployeeModel>> GetEmployeesAsync()
        {
            return await context.Employees
                .AsNoTracking()
                .OrderBy(e => e.FullName)
                .ToListAsync();
        }

        public async Task AddEmployeeAsync(EmployeeModel employee)
        {
            await context.Employees.AddAsync(employee);
            await context.SaveChangesAsync();
        }

        public async Task UpdateEmployeeAsync(EmployeeModel employee)
        {
            var existing = await context.Employees.FindAsync(employee.Id);
            if (existing == null)
            {
                context.Employees.Attach(employee);
                context.Entry(employee).State = EntityState.Modified;
            }
            else
            {
                context.Entry(existing).CurrentValues.SetValues(employee);
            }
            await context.SaveChangesAsync();
        }

        public async Task DeleteEmployeeAsync(EmployeeModel employee)
        {
            var existing = await context.Employees.FindAsync(employee.Id);
            if (existing != null)
            {
                context.Employees.Remove(existing);
                await context.SaveChangesAsync();
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }
    }
}
