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

        Console.Write("Введите номер столбца (0-3): ");
        int col = int.Parse(Console.ReadLine());

        Console.Write("Введите число: ");
        int num = int.Parse(Console.ReadLine());

        int sum = 0;
        for (int i = 0; i < 4; i++)
            sum += arr[i, col];

        if (sum % num == 0)
            Console.WriteLine("Сумма столбца " + sum + " кратна " + num);
        else
            Console.WriteLine("Сумма столбца " + sum + " не кратна " + num);

        Console.ReadKey();
    }
}