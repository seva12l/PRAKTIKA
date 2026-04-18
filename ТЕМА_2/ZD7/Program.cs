using System;

class Program
{
    static void Main()
    {
        Console.Write("Введите строку: ");
        string str = Console.ReadLine();

        Console.Write("Введите суффикс: ");
        string suffix = Console.ReadLine();

        if (str.EndsWith(suffix))
            Console.WriteLine("Строка заканчивается на \"" + suffix + "\"");
        else
            Console.WriteLine("Строка не заканчивается на \"" + suffix + "\"");

        Console.ReadKey();
    }
}