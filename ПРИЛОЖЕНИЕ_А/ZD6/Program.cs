using System;

class Program
{
    static void Main()
    {
        double x = 2;

        // ln(e^(x+1)) это просто x+1
        double part1 = Math.Tan(Math.Sqrt(x + 1));
        double part2 = (3 + Math.Sin(x * x)) / (Math.Sin(x * x) - Math.Cos(x * x));

        double y = part1 - part2;

        Console.WriteLine("y = " + y);
        Console.ReadKey();
    }
}