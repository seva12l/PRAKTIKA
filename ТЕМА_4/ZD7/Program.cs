using System;

/*
 * Вариант 7. Расчет скидок.
 * Создаем два метода с одинаковым именем GetDiscount (перегрузка).
 * Первый метод: стандартная скидка 10%.
 * Второй метод: если покупатель студент - скидка 20%, иначе 10%.
 * Перегрузка - это когда методы называются одинаково, но принимают разные параметры.
*/

class Program
{
    static double GetDiscount(double price)
    {
        return price * 0.9;
    }

    static double GetDiscount(double price, bool isStudent)
    {
        if (isStudent)
            return price * 0.8;
        return price * 0.9;
    }

    static void Main()
    {
        Console.Write("Введите цену: ");
        double price = double.Parse(Console.ReadLine());

        Console.WriteLine("Цена со стандартной скидкой (10%): " + GetDiscount(price));
        Console.WriteLine("Цена со студенческой скидкой (20%): " + GetDiscount(price, true));

        Console.ReadKey();
    }
}