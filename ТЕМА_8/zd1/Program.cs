using System;
using System.Collections.Generic;

ElevatorSystem elevator = new ElevatorSystem(10);

elevator.AddRequest(new ElevatorRequest(3, Direction.Up));
elevator.AddRequest(new ElevatorRequest(7, Direction.Down));
elevator.AddRequest(new ElevatorRequest(1, Direction.Up));
elevator.AddRequest(new ElevatorRequest(5, Direction.Down));
elevator.AddRequest(new ElevatorRequest(9, Direction.Up));

Console.WriteLine("Очередь вызовов лифта:");
elevator.PrintQueue();

Console.WriteLine();
Console.WriteLine("Обработка вызовов:");
elevator.ProcessAll();

Console.WriteLine();
Console.WriteLine("Добавляем новые вызовы:");
elevator.AddRequest(new ElevatorRequest(2, Direction.Up));
elevator.AddRequest(new ElevatorRequest(6, Direction.Down));

Console.WriteLine("Поиск вызовов на этаж 6:");
ElevatorRequest found = elevator.FindByFloor(6);
if (found != null)
    Console.WriteLine("Найден: " + found);
else
    Console.WriteLine("Не найдено");

Console.WriteLine();
Console.WriteLine("Фильтрация — только вызовы Up:");
elevator.PrintFiltered(Direction.Up);

Console.WriteLine();
Console.WriteLine("Обрабатываем следующий вызов:");
elevator.ProcessNext();

Console.WriteLine("Осталось вызовов: " + elevator.Count);


enum Direction
{
    Up,
    Down
}

class ElevatorRequest
{
    public int FloorNumber;
    public Direction Direction;

    public ElevatorRequest(int floor, Direction direction)
    {
        FloorNumber = floor;
        Direction = direction;
    }

    public override string ToString()
    {
        return "Этаж " + FloorNumber + " (" + Direction + ")";
    }
}

class ElevatorSystem
{
    private Queue<ElevatorRequest> requests = new Queue<ElevatorRequest>();
    private int totalFloors;

    public int Count
    {
        get { return requests.Count; }
    }

    public ElevatorSystem(int totalFloors)
    {
        this.totalFloors = totalFloors;
    }

    public void AddRequest(ElevatorRequest request)
    {
        if (request.FloorNumber < 1 || request.FloorNumber > totalFloors)
        {
            Console.WriteLine("Ошибка: этаж " + request.FloorNumber + " не существует");
            return;
        }
        requests.Enqueue(request);
        Console.WriteLine("Добавлен вызов: " + request);
    }

    public void ProcessNext()
    {
        if (requests.Count == 0)
        {
            Console.WriteLine("Очередь пуста");
            return;
        }
        ElevatorRequest req = requests.Dequeue();
        Console.WriteLine("Обработан вызов: " + req);
    }

    public void ProcessAll()
    {
        while (requests.Count > 0)
        {
            ElevatorRequest req = requests.Dequeue();
            Console.WriteLine("  Обработан: " + req);
        }
    }

    public ElevatorRequest FindByFloor(int floor)
    {
        foreach (ElevatorRequest req in requests)
        {
            if (req.FloorNumber == floor)
                return req;
        }
        return null;
    }

    public void PrintQueue()
    {
        if (requests.Count == 0)
        {
            Console.WriteLine("Очередь пуста");
            return;
        }
        foreach (ElevatorRequest req in requests)
        {
            Console.WriteLine("  " + req);
        }
    }

    public void PrintFiltered(Direction direction)
    {
        bool found = false;
        foreach (ElevatorRequest req in requests)
        {
            if (req.Direction == direction)
            {
                Console.WriteLine("  " + req);
                found = true;
            }
        }
        if (!found)
            Console.WriteLine("  Нет вызовов в направлении " + direction);
    }
}
