using System;

class Program
{
    static void Main(string[] args)
    {
        Console.Write("Введите x (x > 3): ");
        double x = double.Parse(Console.ReadLine());
        double sq = Math.Sqrt(x * x - 9);
        double z1 = (x * x + 2 * x - 3 + (x + 1) * sq) / (x * x - 2 * x - 3 + (x - 1) * sq);
        double z2 = Math.Sqrt((x + 3) / (x - 3));
        Console.WriteLine("z1 = " + z1);
        Console.WriteLine("z2 = " + z2);
        Console.ReadKey();
    }
}