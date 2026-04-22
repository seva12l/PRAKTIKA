using System;

abstract class Person
{
    public string FullName;
    public int Age;

    public Person(string fullName, int age)
    {
        FullName = fullName;
        Age = age;
    }
}

sealed class Student : Person
{
    public double AverageGrade;

    public Student(string fullName, int age, double averageGrade) : base(fullName, age)
    {
        AverageGrade = averageGrade;
    }
}

sealed class Teacher : Person
{
    public string Subject;

    public Teacher(string fullName, int age, string subject) : base(fullName, age)
    {
        Subject = subject;
    }
}

class University
{
    public Person[] Persons;

    public University(Person[] persons)
    {
        Persons = persons;
    }

    public Student GetBestStudent()
    {
        Student best = null;
        for (int i = 0; i < Persons.Length; i++)
        {
            if (Persons[i] is Student s)
            {
                if (best == null || s.AverageGrade > best.AverageGrade)
                    best = s;
            }
        }
        return best;
    }

    public void GetTeachersByAge(int age)
    {
        Console.WriteLine("Преподаватели старше " + age + " лет:");
        for (int i = 0; i < Persons.Length; i++)
        {
            if (Persons[i] is Teacher t && t.Age > age)
                Console.WriteLine(t.FullName + ", возраст: " + t.Age + ", предмет: " + t.Subject);
        }
    }
}

class Program
{
    static void Main()
    {
        Person[] persons = new Person[]
        {
            new Student("Иванов Иван", 20, 4.5),
            new Student("Петров Петр", 21, 3.8),
            new Student("Сидоров Сидор", 19, 4.9),
            new Teacher("Козлов Андрей", 45, "Математика"),
            new Teacher("Смирнова Анна", 38, "Физика"),
            new Teacher("Попов Игорь", 52, "Информатика")
        };

        University university = new University(persons);

        Student best = university.GetBestStudent();
        Console.WriteLine("Лучший студент: " + best.FullName + ", средний балл: " + best.AverageGrade);

        Console.WriteLine();

        Console.Write("Введите возраст для фильтрации преподавателей: ");
        int age = int.Parse(Console.ReadLine());
        university.GetTeachersByAge(age);

        Console.ReadKey();
    }
}