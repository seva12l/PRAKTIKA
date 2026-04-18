using System;

class Program
{
    static void Main()
    {
        Console.Write("Введите a: ");
        double a = double.Parse(Console.ReadLine());
        Console.Write("Введите b: ");
        double b = double.Parse(Console.ReadLine());
        Console.Write("Введите c: ");
        double c = double.Parse(Console.ReadLine());

        double x = -b / (2 * a);
        double y = a * x * x + b * x + c;

        Console.WriteLine("Координаты вершины: (" + x + "; " + y + ")");
        Console.ReadKey();
    }
}