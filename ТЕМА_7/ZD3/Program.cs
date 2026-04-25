using System;

/*
 * Задание 3.
 * Создаем исключение TemperatureOutOfRangeException.
 * Класс TemperatureSensor имеет метод SetTemperature.
 * Если температура выходит за пределы [-50, 50] - выбрасываем исключение.
 * В Main обрабатываем исключение через try-catch.
*/

class TemperatureOutOfRangeException : Exception
{
    public TemperatureOutOfRangeException() : base("Температура вышла за допустимый диапазон!") { }

    public TemperatureOutOfRangeException(string message) : base(message) { }

    public TemperatureOutOfRangeException(string message, Exception innerException) : base(message, innerException) { }
}

class TemperatureSensor
{
    public void SetTemperature(int temp)
    {
        if (temp < -50 || temp > 50)
            throw new TemperatureOutOfRangeException("Температура " + temp + " вне диапазона [-50, 50]!");

        Console.WriteLine("Температура установлена: " + temp + " градусов");
    }
}

class Program
{
    static void Main()
    {
        TemperatureSensor sensor = new TemperatureSensor();

        try
        {
            sensor.SetTemperature(25);
            sensor.SetTemperature(75);
        }
        catch (TemperatureOutOfRangeException ex)
        {
            Console.WriteLine("Ошибка: " + ex.Message);
        }

        Console.ReadKey();
    }
}