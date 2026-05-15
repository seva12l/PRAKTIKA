using System;
using System.IO;
using EmployeeApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApp.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<EmployeeModel> Employees => Set<EmployeeModel>();

        public static string DbPath { get; } =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "employees.db");

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(DbPath)!);
            options.UseSqlite($"Data Source={DbPath}");
        }

        protected override void OnModelCreating(ModelBuilder b)
        {
            b.Entity<EmployeeModel>().HasKey(e => e.Id);
            b.Entity<EmployeeModel>().Ignore(e => e.StatusText);
            b.Entity<EmployeeModel>().Ignore(e => e.Initials);
            b.Entity<EmployeeModel>().Property(e => e.FullName).HasMaxLength(200);
            b.Entity<EmployeeModel>().Property(e => e.Position).HasMaxLength(100);
            b.Entity<EmployeeModel>().Property(e => e.Department).HasMaxLength(100);
            b.Entity<EmployeeModel>().Property(e => e.Salary).HasColumnType("DECIMAL(18,2)");
        }
    }
}
