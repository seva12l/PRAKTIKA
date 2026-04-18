using System;

class Program
{
    static void Main()
    {
        int[] a = new int[25];
        int[] b = new int[25];

        Console.WriteLine("Введите массив a (25 чисел):");
        for (int i = 0; i < 25; i++)
        {
            Console.Write("a[" + i + "] = ");
            a[i] = int.Parse(Console.ReadLine());
        }

        Console.WriteLine("Введите массив b (25 чисел):");
        for (int i = 0; i < 25; i++)
        {
            Console.Write("b[" + i + "] = ");
            b[i] = int.Parse(Console.ReadLine());
        }

        for (int i = 0; i < 25; i++)
        {
            if (b[i] > a[i])
                b[i] = b[i] * 10;
            else
                b[i] = 0;
        }

        Console.WriteLine("Преобразованный массив b:");
        for (int i = 0; i < 25; i++)
            Console.Write(b[i] + " ");

        Console.ReadKey();
    }
}