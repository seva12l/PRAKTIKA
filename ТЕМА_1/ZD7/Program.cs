using System;

class Program
{
    static void Main()
    {
        Console.Write("Введите A: ");
        int a = int.Parse(Console.ReadLine());
        Console.Write("Введите B: ");
        int b = int.Parse(Console.ReadLine());

        // Способ 1 - while
        Console.WriteLine("While:");
        int i = b;
        while (i >= a)
        {
            Console.WriteLine(i + "^3 = " + i * i * i);
            i--;
        }

        // Способ 2 - do while
        Console.WriteLine("Do While:");
        i = b;
        do
        {
            Console.WriteLine(i + "^3 = " + i * i * i);
            i--;
        }
        while (i >= a);

        // Способ 3 - for
        Console.WriteLine("For:");
        for (int j = b; j >= a; j--)
        {
            Console.WriteLine(j + "^3 = " + j * j * j);
        }

        Console.ReadKey();
    }
}