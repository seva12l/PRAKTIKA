using System;
<<<<<<< HEAD

namespace ThreeDigitNumber
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Определение первой и последней цифр трёхзначного числа.");
            Console.Write("Введите трёхзначное число: ");
            int n = int.Parse(Console.ReadLine());
            if (n < 100 || n > 999)
            {
                Console.WriteLine("Ошибка: введённое число не является трёхзначным!");
            }
            else
            {
                int firstDigit = n / 100;
                int lastDigit = n % 10;

                Console.WriteLine($"Число: {n}");
                Console.WriteLine($"Первая цифра:    {firstDigit}");
                Console.WriteLine($"Последняя цифра: {lastDigit}");
            }

            Console.ReadKey();
        }
=======
class Program
{
    static void Main()
    {
        Console.Write("Введите трёхзначное число: ");
        int n = int.Parse(Console.ReadLine());
        int firstDigit = n / 100;
        int lastDigit = n % 10;
        Console.WriteLine("Первая цифра: " + firstDigit);
        Console.WriteLine("Последняя цифра: " + lastDigit);
        Console.ReadKey();
>>>>>>> 726c10b (Чистый старт проекта)
    }
}