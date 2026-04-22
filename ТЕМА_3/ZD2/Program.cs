using System;

static class ArrayManager
{
    // 1. Генерация данных
    public static void Generate(double[] arr)
    {
        Random rnd = new Random();
        for (int i = 0; i < arr.Length; i++) arr[i] = rnd.Next(1, 10);
    }

    // 2. Сортировка
    public static void Sort(double[] arr) => Array.Sort(arr);

    // 3. Фильтрация (вывод чисел > 5)
    public static void Filter(double[] arr)
    {
        foreach (double x in arr) if (x > 5) Console.Write(x + " ");
        Console.WriteLine();
    }

    // 4. Статистика (среднее арифметическое)
    public static double GetAverage(double[] arr)
    {
        double sum = 0;
        foreach (double x in arr) sum += x;
        return sum / arr.Length;
    }

    // 5. Произведение элементов (обязательное требование)
    public static double Product(double[] arr)
    {
        double p = 1;
        foreach (double x in arr) p *= x;
        return p;
    }
}

class Program
{
    static void Main()
    {
        double[] arr = new double[5];

        ArrayManager.Generate(arr);
        Console.WriteLine("Сгенерированный массив: " + string.Join(", ", arr));

        ArrayManager.Sort(arr);
        Console.WriteLine("Отсортированный: " + string.Join(", ", arr));

        Console.Write("Числа больше 5: ");
        ArrayManager.Filter(arr);

        Console.WriteLine("Среднее: " + ArrayManager.GetAverage(arr));
        Console.WriteLine("Произведение (Product): " + ArrayManager.Product(arr));

        Console.ReadKey();
    }
}