using System;

class Program
{
    static void Main()
    {
        Console.Write("Введите строку: ");
        StringBuilder sb = new StringBuilder(Console.ReadLine());

        Console.Write("Введите подстроку: ");
        string sub = Console.ReadLine();

        if (sb.ToString().StartsWith(sub))
            Console.WriteLine("StringBuilder начинается с \"" + sub + "\"");
        else
            Console.WriteLine("StringBuilder не начинается с \"" + sub + "\"");

        Console.ReadKey();
    }
}