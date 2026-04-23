using System;

/*
 * Задание 2.
 * Демонстрируем три вида отношений между классами:
 * Композиция - Equipment создается внутри Athlete (экипировка — часть спортсмена).
 * Агрегация - массив Coach[] передается в Athlete снаружи (тренеры существуют отдельно).
 * Ассоциация - Athlete связан с Team, но не владеет им (команда существует отдельно).
*/

class Equipment
{
    public string Type;
    public Equipment(string type) { Type = type; }
}

class Coach
{
    public string Name;
    public Coach(string name) { Name = name; }
}

class Team
{
    public string TeamName;
    public Team(string name) { TeamName = name; }
}

class Athlete
{
    public string Name;
    public Equipment Equipment;  // Композиция
    public Coach[] Coaches;      // Агрегация
    public Team Team;            // Ассоциация

    public Athlete(string name, string equipmentType, Coach[] coaches, Team team)
    {
        Name = name;
        Equipment = new Equipment(equipmentType);  // Создается внутри
        Coaches = coaches;
        Team = team;
    }

    public void Train()
    {
        Console.WriteLine("Спортсмен: " + Name);
        Console.WriteLine("Экипировка: " + Equipment.Type);
        Console.WriteLine("Команда: " + Team.TeamName);
        Console.Write("Тренеры: ");
        foreach (Coach c in Coaches)
            Console.Write(c.Name + " ");
        Console.WriteLine();
        Console.WriteLine("Тренировка начата!");
        Console.WriteLine();
    }
}

class Program
{
    static void Main()
    {
        Team team1 = new Team("Динамо");
        Team team2 = new Team("Спартак");

        Coach[] coaches1 = { new Coach("Иванов"), new Coach("Петров") };
        Coach[] coaches2 = { new Coach("Сидоров") };

        Athlete[] athletes = new Athlete[]
        {
            new Athlete("Алексей", "Кроссовки Nike", coaches1, team1),
            new Athlete("Мария", "Кроссовки Adidas", coaches2, team2)
        };

        foreach (Athlete a in athletes)
            a.Train();

        Console.ReadKey();
    }
}