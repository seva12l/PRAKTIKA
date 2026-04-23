using System;

/*
 * Задание 1.
 * Создаем абстрактный класс LearningMode с абстрактным методом GetLearningType().
 * Создаем три наследника: Online, Offline, Hybrid.
 * Каждый наследник реализует метод GetLearningType() по-своему.
 * Заполняем массив видами обучения и выводим их особенности.
*/

abstract class LearningMode
{
    public string Name;
    public abstract string GetLearningType();
}

class Online : LearningMode
{
    public Online() { Name = "Онлайн"; }
    public override string GetLearningType()
    {
        return "Онлайн обучение: занятия проводятся через интернет";
    }
}

class Offline : LearningMode
{
    public Offline() { Name = "Оффлайн"; }
    public override string GetLearningType()
    {
        return "Оффлайн обучение: занятия проводятся в аудитории";
    }
}

class Hybrid : LearningMode
{
    public Hybrid() { Name = "Гибридное"; }
    public override string GetLearningType()
    {
        return "Гибридное обучение: сочетание онлайн и оффлайн занятий";
    }
}

class Program
{
    static void Main()
    {
        LearningMode[] modes = new LearningMode[]
        {
            new Online(),
            new Offline(),
            new Hybrid()
        };

        foreach (LearningMode mode in modes)
            Console.WriteLine(mode.GetLearningType());

        Console.ReadKey();
    }
}