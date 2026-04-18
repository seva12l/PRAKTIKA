using System;

class Program
{
    static void Main()
    {
        Console.Write("Введите x: ");
        double x = double.Parse(Console.ReadLine());

        double y;
        if (x >= 4 && x <= 6)
            y = x;
        else
            y = 3 * x + 4 * x * x;

        Console.WriteLine("y = " + y);
        Console.ReadKey();
    }
}