using System;

/*
 * Вариант 7.
 * Создаем функцию Leng, которая считает длину отрезка AB на плоскости.
 * Формула: |AB| = sqrt((xA-xB)^2 + (yA-yB)^2)
 * Используем эту функцию для нахождения длин отрезков AB, AC, AD.
*/

class Program
{
    static double Leng(double xA, double yA, double xB, double yB)
    {
        return Math.Sqrt((xA - xB) * (xA - xB) + (yA - yB) * (yA - yB));
    }

    static void Main()
    {
        Console.Write("Координаты A (xA yA): ");
        string[] a = Console.ReadLine().Split();
        double xA = double.Parse(a[0]), yA = double.Parse(a[1]);

        Console.Write("Координаты B (xB yB): ");
        string[] b = Console.ReadLine().Split();
        double xB = double.Parse(b[0]), yB = double.Parse(b[1]);

        Console.Write("Координаты C (xC yC): ");
        string[] c = Console.ReadLine().Split();
        double xC = double.Parse(c[0]), yC = double.Parse(c[1]);

        Console.Write("Координаты D (xD yD): ");
        string[] d = Console.ReadLine().Split();
        double xD = double.Parse(d[0]), yD = double.Parse(d[1]);

        Console.WriteLine("Длина AB: " + Leng(xA, yA, xB, yB));
        Console.WriteLine("Длина AC: " + Leng(xA, yA, xC, yC));
        Console.WriteLine("Длина AD: " + Leng(xA, yA, xD, yD));

        Console.ReadKey();
    }
}