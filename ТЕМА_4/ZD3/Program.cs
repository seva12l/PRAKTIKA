using System;

/*
 * Задание 3.
 * Простое число - это число, которое делится только на 1 и на себя.
 * Проверяем рекурсивно: пытаемся найти делитель числа n начиная с 2.
 * Если нашли делитель - число не простое (false).
 * Если дошли до sqrt(n) и не нашли - число простое (true).
*/

class Program
{
    static bool IsPrime(int n, int divisor = 2)
    {
        if (n < 2) return false;
        if (divisor * divisor > n) return true;
        if (n % divisor == 0) return false;
        return IsPrime(n, divisor + 1);
    }

    static void Main()
    {
        Console.Write("Введите число: ");
        int n = int.Parse(Console.ReadLine());

        if (IsPrime(n))
            Console.WriteLine(n + " - простое число");
        else
            Console.WriteLine(n + " - не простое число");

        Console.ReadKey();
    }
}