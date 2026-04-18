using System;

class Program
{
    static void Main()
    {
        Console.Write("Введите трехзначное число: ");
        int n = int.Parse(Console.ReadLine());

        int a = n / 100;
        int b = (n / 10) % 10;
        int c = n % 10;

        int result = a * b * c;
        Console.WriteLine("Произведение цифр: " + result);
        Console.ReadKey();
    }
}