using System;

/*
 * Задание 2.
 * Создаем процедуру (метод без возвращаемого значения), которая добавляет
 * цифру D справа к числу K. Это делается умножением K на 10 и прибавлением D.
 * Например: K=12, D=5 -> K=125
 * Параметр K передается по ссылке (ref), чтобы изменения сохранились снаружи метода.
*/

class Program
{
    static void AddRightDigit(int d, ref int k)
    {
        k = k * 10 + d;
    }

    static void Main()
    {
        Console.Write("Введите число K: ");
        int k = int.Parse(Console.ReadLine());

        Console.Write("Введите цифру D1 (0-9): ");
        int d1 = int.Parse(Console.ReadLine());

        Console.Write("Введите цифру D2 (0-9): ");
        int d2 = int.Parse(Console.ReadLine());

        AddRightDigit(d1, ref k);
        Console.WriteLine("После добавления D1: " + k);

        AddRightDigit(d2, ref k);
        Console.WriteLine("После добавления D2: " + k);

        Console.ReadKey();
    }
}