using System;

class Program
{
    static void Main()
    {
        int[] arr = new int[10];
        Console.WriteLine("Введите 10 чисел:");
        for (int i = 0; i < arr.Length; i++)
        {
            Console.Write("arr[" + i + "] = ");
            arr[i] = int.Parse(Console.ReadLine());
        }

        int sum = 0;
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] % 3 == 0)
                sum += arr[i];
        }

        Console.WriteLine("Сумма чисел кратных 3: " + sum);
        Console.ReadKey();
    }
}