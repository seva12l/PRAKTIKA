using System;
<<<<<<< HEAD

namespace Triangle
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Вычисление площади треугольника.");
            Console.WriteLine("Введите исходные данные:");

            Console.Write("Основание (см) --> ");
            double a = double.Parse(Console.ReadLine());

            Console.Write("Высота (см) --> ");
            double h = double.Parse(Console.ReadLine());

            double s = (a * h) / 2.0;

            Console.WriteLine($"Площадь треугольника {s:F2} кв.см.");

            Console.ReadKey();
        }
=======
class Program
{
    static void Main()
    {
        Console.Write("Основание (см) --> ");
        double a = double.Parse(Console.ReadLine());
        Console.Write("Высота (см) --> ");
        double h = double.Parse(Console.ReadLine());
        double s = (a * h) / 2;
        Console.WriteLine("Площадь треугольника " + s + " кв.см.");
        Console.ReadKey();
>>>>>>> 726c10b (Чистый старт проекта)
    }
}