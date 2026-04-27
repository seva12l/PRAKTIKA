using System;
using System.Collections.Generic;

List<Animal> animals = new List<Animal>
{
    new Animal { Name = "Барсик", Species = "Лев", Age = 5 },
    new Animal { Name = "Тоша", Species = "Тигр", Age = 2 },
    new Animal { Name = "Маша", Species = "Слон", Age = 10 },
    new Animal { Name = "Рекс", Species = "Волк", Age = 3 },
    new Animal { Name = "Зара", Species = "Лев", Age = 7 }
};

SimpleSearch<Animal> search = new SimpleSearch<Animal>();
SearchManager<Animal> manager = new SearchManager<Animal>(search);

Console.WriteLine("Все животные:");
foreach (Animal a in animals)
    Console.WriteLine("  " + a);

Console.WriteLine();
Console.WriteLine("Поиск первого льва:");
manager.DisplaySearchResult(animals, a => a.Species == "Лев");

Console.WriteLine();
Console.WriteLine("Поиск первого животного старше 6 лет:");
manager.DisplaySearchResult(animals, a => a.Age > 6);

Console.WriteLine();
Console.WriteLine("Поиск по имени 'Рекс':");
manager.DisplaySearchResult(animals, a => a.Name == "Рекс");

Console.WriteLine();
Console.WriteLine("Поиск несуществующего (вид 'Жираф'):");
manager.DisplaySearchResult(animals, a => a.Species == "Жираф");

Console.WriteLine();
List<int> numbers = new List<int> { 15, 3, 42, 7, 88, 1, 56 };
SimpleSearch<int> numSearch = new SimpleSearch<int>();
SearchManager<int> numManager = new SearchManager<int>(numSearch);

Console.WriteLine("Числа: 15, 3, 42, 7, 88, 1, 56");
Console.WriteLine("Поиск первого числа больше 40:");
numManager.DisplaySearchResult(numbers, n => n > 40);

Console.WriteLine("Поиск первого чётного числа:");
numManager.DisplaySearchResult(numbers, n => n % 2 == 0);


class Animal
{
    public string Name;
    public string Species;
    public int Age;

    public override string ToString()
    {
        return Name + " (" + Species + "), " + Age + " лет";
    }
}

interface ISearchable<T>
{
    T Find(IEnumerable<T> items, Func<T, bool> predicate);
}

class SimpleSearch<T> : ISearchable<T>
{
    public T Find(IEnumerable<T> items, Func<T, bool> predicate)
    {
        foreach (T item in items)
        {
            if (predicate(item))
                return item;
        }
        return default(T);
    }
}

class SearchManager<T>
{
    private ISearchable<T> searcher;

    public SearchManager(ISearchable<T> searcher)
    {
        this.searcher = searcher;
    }

    public void DisplaySearchResult(IEnumerable<T> items, Func<T, bool> predicate)
    {
        T result = searcher.Find(items, predicate);

        if (result == null || result.Equals(default(T)))
            Console.WriteLine("  Не найдено");
        else
            Console.WriteLine("  Найдено: " + result);
    }
}
