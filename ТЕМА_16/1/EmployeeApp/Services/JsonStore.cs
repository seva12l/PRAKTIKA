using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace EmployeeApp.Services
{
    /// <summary>
    /// Универсальное JSON-хранилище для коллекций моделей.
    /// Файлы хранятся рядом с .exe, в подкаталоге Data/.
    /// </summary>
    public static class JsonStore
    {
        private static readonly JsonSerializerOptions Options = new()
        {
            WriteIndented = true,
            PropertyNamingPolicy = null
        };

        public static string DataDir { get; } =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");

        public static string PathFor(string fileName)
        {
            Directory.CreateDirectory(DataDir);
            return Path.Combine(DataDir, fileName);
        }

        public static List<T> Load<T>(string fileName)
        {
            var path = PathFor(fileName);
            if (!File.Exists(path)) return new List<T>();
            try
            {
                var json = File.ReadAllText(path);
                return JsonSerializer.Deserialize<List<T>>(json, Options) ?? new List<T>();
            }
            catch
            {
                return new List<T>();
            }
        }

        public static void Save<T>(string fileName, List<T> data)
        {
            var path = PathFor(fileName);
            var json = JsonSerializer.Serialize(data, Options);
            File.WriteAllText(path, json);
        }
    }
}
