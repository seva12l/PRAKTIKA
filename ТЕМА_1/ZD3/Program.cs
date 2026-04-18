using System;

class Program
{
    static void Main()
    {
        Console.Write("Введите A: ");
        int a = int.Parse(Console.ReadLine());
        Console.Write("Введите B: ");
        int b = int.Parse(Console.ReadLine());

        int result = 1;
        for (int i = a; i <= b; i++)
        {
            result *= i;
        }

        Console.WriteLine("Произведение чисел от " + a + " до " + b + " = " + result);
        Console.ReadKey();
    }
}