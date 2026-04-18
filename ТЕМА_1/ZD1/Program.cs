using System;

class Program
{
    static void Main()
    {
        Console.Write("Введите сторону квадрата: ");
        double a = double.Parse(Console.ReadLine());
        double p = 4 * a;
        Console.WriteLine("Периметр квадрата: " + p);
        Console.ReadKey();
    }
}