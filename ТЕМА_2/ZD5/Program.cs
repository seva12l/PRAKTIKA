using System;

class Program
{
    static void Main()
    {
        int[][] arr = new int[4][];
        arr[0] = new int[] { 1, 2 };
        arr[1] = new int[] { 3, 4, 5 };
        arr[2] = new int[] { 6 };
        arr[3] = new int[] { 7, 8, 9, 10 };

        Console.WriteLine("Исходный массив:");
        for (int i = 0; i < arr.Length; i++)
        {
            for (int j = 0; j < arr[i].Length; j++)
                Console.Write(arr[i][j] + " ");
            Console.WriteLine();
        }

        for (int i = 0; i < arr.Length; i++)
            Array.Reverse(arr[i]);

        Console.WriteLine("Зеркальный массив:");
        for (int i = 0; i < arr.Length; i++)
        {
            for (int j = 0; j < arr[i].Length; j++)
                Console.Write(arr[i][j] + " ");
            Console.WriteLine();
        }

        Console.ReadKey();
    }
}