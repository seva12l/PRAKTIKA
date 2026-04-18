using System;
class Program
{
    static void Main()
    {
        Console.Write("Введите строку: ");
        string str = Console.ReadLine();

        string result = Regex.Replace(str, @" +", " ");

        Console.WriteLine("Результат: " + result);
        Console.ReadKey();
    }
}