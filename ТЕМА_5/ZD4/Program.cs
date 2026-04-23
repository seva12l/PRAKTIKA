using System;

/*
 * Задание 4.
 * Создаем два интерфейса IPowerOn и IPowerOff с одинаковым методом TogglePower().
 * Класс Device реализует оба интерфейса явно (explicit).
 * Явная реализация означает что метод доступен только через ссылку на интерфейс,
 * а не через ссылку на сам класс Device.
*/

interface IPowerOn
{
    void TogglePower();
}

interface IPowerOff
{
    void TogglePower();
}

class Device : IPowerOn, IPowerOff
{
    public string Name;

    public Device(string name) { Name = name; }

    void IPowerOn.TogglePower()
    {
        Console.WriteLine(Name + ": устройство включается");
    }

    void IPowerOff.TogglePower()
    {
        Console.WriteLine(Name + ": устройство выключается");
    }
}

class Program
{
    static void Main()
    {
        Device device = new Device("Ноутбук");

        IPowerOn powerOn = device;
        IPowerOff powerOff = device;

        powerOn.TogglePower();
        powerOff.TogglePower();

        Console.ReadKey();
    }
}