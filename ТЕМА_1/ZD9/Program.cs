using System;

class Program
{
    static void Main()
    {
        double a = 0.5;
        double b = 1;
        int m = 10;

        double h = (b - a) / m;

        Console.WriteLine("x\t\tF(x)");
        Console.WriteLine("------------------------");

        double x = a;
        for (int i = 0; i <= m; i++)
        {
            double y = Math.Acos(x);
            Console.WriteLine(x + "\t\t" + y);
            x = x + h;
        }

        Console.ReadKey();
    }
}