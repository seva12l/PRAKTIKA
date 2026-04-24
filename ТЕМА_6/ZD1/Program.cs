using System;

/*
 * Задание 1.
 * Создаем пользовательский делегат TimeHandler.
 * Два класса TimeFormatter и DateFormatter реализуют разные форматы вывода времени.
 * Демонстрируем передачу методов в делегат и их вызов.
*/

delegate string TimeHandler(DateTime dateTime);

class TimeFormatter
{
    public string FormatTime(DateTime dateTime)
    {
        return "Время: " + dateTime.ToString("HH:mm:ss");
    }
}

class DateFormatter
{
    public string FormatDate(DateTime dateTime)
    {
        return "Дата: " + dateTime.ToString("dd.MM.yyyy");
    }
}

class Program
{
    static void Main()
    {
        TimeFormatter timeFormatter = new TimeFormatter();
        DateFormatter dateFormatter = new DateFormatter();

        TimeHandler handler = timeFormatter.FormatTime;
        Console.WriteLine(handler(DateTime.Now));

        handler = dateFormatter.FormatDate;
        Console.WriteLine(handler(DateTime.Now));

        Console.ReadKey();
    }
}