using System;

/*
 * Вариант 7. Возврат минимального значения.
 * Создаем два метода GetMinValue (перегрузка): для int и для double.
 * in - параметр только для чтения, нельзя изменить внутри метода.
 * out - параметр только для записи, метод ОБЯЗАН присвоить ему значение.
 * Находим минимум из двух чисел и записываем его в out параметр.
*/

class Program
{
    static void GetMinValue(in int a, in int b, out int minValue)
    {
        if (a < b) minValue = a;
        else minValue = b;
    }

    static void GetMinValue(in double a, in double b, out double minValue)
    {
        if (a < b) minValue = a;
        else minValue = b;
    }

    static void Main()
    {
        Console.Write("Введите первое целое число: ");
        int a = int.Parse(Console.ReadLine());
        Console.Write("Введите второе целое число: ");
        int b = int.Parse(Console.ReadLine());

        GetMinValue(in a, in b, out int minInt);
        Console.WriteLine("Минимальное целое: " + minInt);

        Console.Write("Введите первое вещественное число: ");
        double x = double.Parse(Console.ReadLine());
        Console.Write("Введите второе вещественное число: ");
        double y = double.Parse(Console.ReadLine());

        GetMinValue(in x, in y, out double minDouble);
        Console.WriteLine("Минимальное вещественное: " + minDouble);

        Console.ReadKey();
    }
}