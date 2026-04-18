using System;

class Program
{
    static void Main()
    {
        Console.Write("Введите трёхзначное число: ");
        int n = int.Parse(Console.ReadLine());

        int a = n / 100;
        int b = (n / 10) % 10;
        int c = n % 10;

        if (a == b && b == c)
            Console.WriteLine("Все цифры одинаковы");
        else
            Console.WriteLine("Цифры не одинаковы");

        Console.ReadKey();
    }
}