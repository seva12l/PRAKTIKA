using System;

class Program
{
    static void Main()
    {
        Console.Write("Введите строку: ");
        string str = Console.ReadLine();

        string result = "";
        for (int i = 0; i < str.Length; i++)
        {
            if (char.IsUpper(str[i]))
                result += char.ToLower(str[i]);
            else if (char.IsLower(str[i]))
                result += char.ToUpper(str[i]);
            else
                result += str[i];
        }

        Console.WriteLine("Результат: " + result);
        Console.ReadKey();
    }
}