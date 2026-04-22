using System;

class A
{
    public int a;
    public int b;

    public A(int a, int b)
    {
        this.a = a;
        this.b = b;
    }

    public double CalcExpression()
    {
        return 1.0 / (1 + (double)(a + b) / 2);
    }

    public int SquareDiff()
    {
        return (a - b) * (a - b);
    }
}

class Program
{
    static void Main()
    {
        Console.Write("Введите a: ");
        int a = int.Parse(Console.ReadLine());
        Console.Write("Введите b: ");
        int b = int.Parse(Console.ReadLine());

        A obj = new A(a, b);

        Console.WriteLine("Значение выражения 1/(1+(a+b)/2) = " + obj.CalcExpression());
        Console.WriteLine("Квадрат разности (a-b)^2 = " + obj.SquareDiff());

        Console.ReadKey();
    }
}