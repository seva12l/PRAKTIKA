using System;

/*
 * Задание 1.
 * Создаем статический класс с методом, который принимает массив чисел
 * и возвращает среднее арифметическое всех элементов.
 * Складываем все элементы и делим на количество элементов.
*/

static class ArrayHelper
{
    public static double GetAverage(double[] arr)
    {
        double sum = 0;
        for (int i = 0; i < arr.Length; i++)
            sum += arr[i];
        return sum / arr.Length;
    }
}

class Program
{
    static void Main()
    {
        double[] arr = { 2, 4, 6, 8, 10 };
        Console.WriteLine("Среднее значение: " + ArrayHelper.GetAverage(arr));
        Console.ReadKey();
    }
}