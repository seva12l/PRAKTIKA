using System;

class Program
{
    static void Main()
    {
        Console.Write("Введите строку: ");
        string str = Console.ReadLine();

        double result;
        if (double.TryParse(str, out result))
            Console.WriteLine("\"" + str + "\" является числом с плавающей точкой");
        else
            Console.WriteLine("\"" + str + "\" не является числом с плавающей точкой");

        Console.ReadKey();
    }
}