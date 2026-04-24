using System;

/*
 * Задание 2.
 * Создаем делегат ArrayProcessor для обработки массива.
 * Метод ProcessArray принимает массив и делегат, который его изменяет.
 * Два метода SortAscending и SortDescending передаются в ProcessArray.
*/

delegate int[] ArrayProcessor(int[] arr);

class Program
{
    static int[] SortAscending(int[] arr)
    {
        Array.Sort(arr);
        return arr;
    }

    static int[] SortDescending(int[] arr)
    {
        Array.Sort(arr);
        Array.Reverse(arr);
        return arr;
    }

    static void ProcessArray(int[] arr, ArrayProcessor processor)
    {
        int[] result = processor(arr);
        Console.Write("Результат: ");
        foreach (int x in result)
            Console.Write(x + " ");
        Console.WriteLine();
    }

    static void Main()
    {
        int[] arr1 = { 5, 3, 8, 1, 9, 2 };
        int[] arr2 = { 5, 3, 8, 1, 9, 2 };

        Console.Write("Исходный массив: ");
        foreach (int x in arr1) Console.Write(x + " ");
        Console.WriteLine();

        Console.Write("Сортировка по возрастанию: ");
        ProcessArray(arr1, SortAscending);

        Console.Write("Сортировка по убыванию: ");
        ProcessArray(arr2, SortDescending);

        Console.ReadKey();
    }
}