using System;
using System.IO;

/*
 * Задание 2.
 * ConfigLoader пытается загрузить файл и выбрасывает FileNotFoundException.
 * ConfigurationManager перехватывает это исключение и оборачивает его
 * в своё ConfigurationException (InnerException).
 * Логируем всю информацию об исключении включая стек вызовов.
*/

class ConfigurationException : Exception
{
    public ConfigurationException() : base("Ошибка конфигурации!") { }

    public ConfigurationException(string message) : base(message) { }

    public ConfigurationException(string message, Exception innerException) : base(message, innerException) { }
}

class ConfigLoader
{
    public void LoadConfig(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException("Файл конфигурации не найден: " + path);

        Console.WriteLine("Файл успешно загружен: " + path);
    }
}

class ConfigurationManager
{
    private ConfigLoader loader = new ConfigLoader();

    public void Load(string path)
    {
        try
        {
            loader.LoadConfig(path);
        }
        catch (FileNotFoundException ex)
        {
            throw new ConfigurationException("Не удалось загрузить конфигурацию", ex);
        }
    }
}

class Program
{
    static void Main()
    {
        ConfigurationManager manager = new ConfigurationManager();

        try
        {
            manager.Load("config.json");
        }
        catch (ConfigurationException ex)
        {
            Console.WriteLine("Ошибка: " + ex.Message);
            Console.WriteLine("Внутреннее исключение: " + ex.InnerException.Message);
            Console.WriteLine("Стек вызовов: " + ex.StackTrace);
        }

        Console.ReadKey();
    }
}