using System;

class Program
{
    static void Main()
    {
        double x = 2;

        // ln(e^(x+1)) упрощается до x+1
        double term1 = Math.Tan(Math.Sqrt(x + 1));
        double term2 = (3 + Math.Sin(x * x)) / (Math.Sin(x * x) - Math.Cos(x * x));

        double y = term1 - term2;

        Console.WriteLine("y = " + y);
        Console.ReadKey();
    }
}