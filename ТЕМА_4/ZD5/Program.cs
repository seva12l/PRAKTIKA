using System;

/*
 * Задание 5.
 * Создаем абстрактный класс Shape3D с абстрактным методом CalculateVolume()
 * и виртуальным методом DisplayInfo().
 * Абстрактный метод - метод без реализации, который ОБЯЗАН быть реализован в наследнике.
 * Виртуальный метод - метод с реализацией по умолчанию, который МОЖНО переопределить.
 * Создаем два наследника: Sphere (сфера) и Cube (куб).
*/

abstract class Shape3D
{
    public abstract double CalculateVolume();

    public virtual void DisplayInfo()
    {
        Console.WriteLine("Это трёхмерная фигура. Объём: " + CalculateVolume());
    }
}

class Sphere : Shape3D
{
    public double Radius;

    public Sphere(double radius)
    {
        Radius = radius;
    }

    public override double CalculateVolume()
    {
        return (4.0 / 3.0) * Math.PI * Radius * Radius * Radius;
    }

    public override void DisplayInfo()
    {
        Console.WriteLine("Сфера. Радиус: " + Radius + ". Объём: " + CalculateVolume());
    }
}

class Cube : Shape3D
{
    public double Side;

    public Cube(double side)
    {
        Side = side;
    }

    public override double CalculateVolume()
    {
        return Side * Side * Side;
    }

    public override void DisplayInfo()
    {
        Console.WriteLine("Куб. Сторона: " + Side + ". Объём: " + CalculateVolume());
    }
}

class Program
{
    static void Main()
    {
        Shape3D sphere = new Sphere(5);
        Shape3D cube = new Cube(3);

        sphere.DisplayInfo();
        cube.DisplayInfo();

        Console.ReadKey();
    }
}