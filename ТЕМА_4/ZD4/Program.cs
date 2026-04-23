using System;

/*
 * Задание 4.
 * Создаем расширяющий метод для типа string.
 * Расширяющий метод позволяет добавить новый метод к уже существующему типу.
 * Метод проверяет, состоит ли строка только из цифр.
 * Проходим по каждому символу строки и проверяем является ли он цифрой.
*/

static class StringExtensions
{
    public static bool IsAllDigits(this string str)
    {
        foreach (char c in str)
            if (!char.IsDigit(c)) return false;
        return true;
    }
}

class Program
{
    static void Main()
    {
        Console.Write("Введите строку: ");
        string str = Console.ReadLine();

        if (str.IsAllDigits())
            Console.WriteLine("Строка содержит только цифры");
        else
            Console.WriteLine("Строка содержит не только цифры");

        Console.ReadKey();
    }
}