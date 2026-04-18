using System;

class Program
{
    static void Main()
    {
        Console.Write("Введите A: ");
        int a = int.Parse(Console.ReadLine());
        Console.Write("Введите B: ");
        int b = int.Parse(Console.ReadLine());

        int count = 0;
        for (int i = a; i <= b; i++)
        {
            Console.Write(i + " ");
            count++;
        }

        Console.WriteLine();
        Console.WriteLine("Количество чисел: " + count);
        Console.ReadKey();
    }
}