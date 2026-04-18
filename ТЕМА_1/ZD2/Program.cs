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
        int sum = a + b + c;

        if (sum % 2 == 0)
            Console.WriteLine("Истина: сумма цифр " + sum + " является чётным числом");
        else
            Console.WriteLine("Ложь: сумма цифр " + sum + " не является чётным числом");

        Console.ReadKey();
    }
}