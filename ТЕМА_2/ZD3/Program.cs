using System;

class Program
{
    static void Main()
    {
        int[,] arr = new int[4, 4];

        Console.WriteLine("Введите элементы массива 4x4:");
        for (int i = 0; i < 4; i++)
            for (int j = 0; j < 4; j++)
            {
                Console.Write("arr[" + i + "][" + j + "] = ");
                arr[i, j] = int.Parse(Console.ReadLine());
            }

        int product = 1;
        for (int i = 0; i < 4; i++)
            for (int j = 0; j < 4; j++)
                if (arr[i, j] % 2 != 0)
                    product *= arr[i, j];

        Console.WriteLine("Произведение нечётных элементов: " + product);

        Console.Write("Введите номер строки k (0-3): ");
        int k = int.Parse(Console.ReadLine());

        int sum = 0;
        for (int j = 0; j < 4; j++)
            sum += arr[k, j];

        Console.WriteLine("Сумма элементов " + k + "-й строки: " + sum);
        Console.ReadKey();
    }
}