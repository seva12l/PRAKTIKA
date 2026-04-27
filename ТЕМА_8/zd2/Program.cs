using System;
using System.Collections.Generic;

Console.WriteLine("=== MyFixedQueue<int> (размер 4) ===");
FixedQueueProcessor<int> intProcessor = new FixedQueueProcessor<int>(4);

intProcessor.Add(10);
intProcessor.Add(20);
intProcessor.Add(30);
intProcessor.Add(40);

Console.WriteLine("Очередь заполнена:");
intProcessor.PrintAll();

Console.WriteLine();
Console.WriteLine("Добавляем 50 — очередь переполнена, удаляем старейший:");
intProcessor.Add(50);
intProcessor.PrintAll();

Console.WriteLine();
Console.WriteLine("Добавляем 60:");
intProcessor.Add(60);
intProcessor.PrintAll();

Console.WriteLine();
Console.WriteLine("Peek (первый элемент без удаления): " + intProcessor.Peek());
Console.WriteLine("Dequeue (извлекаем первый): " + intProcessor.Dequeue());
Console.WriteLine("После Dequeue:");
intProcessor.PrintAll();

Console.WriteLine();
Console.WriteLine("Содержит 30? " + intProcessor.Contains(30));
Console.WriteLine("Содержит 10? " + intProcessor.Contains(10));

Console.WriteLine();
Console.WriteLine("=== MyFixedQueue<string> (размер 3) ===");
FixedQueueProcessor<string> strProcessor = new FixedQueueProcessor<string>(3);

strProcessor.Add("первый");
strProcessor.Add("второй");
strProcessor.Add("третий");
strProcessor.PrintAll();

Console.WriteLine("Добавляем 'четвертый' — вытесняет 'первый':");
strProcessor.Add("четвертый");
strProcessor.PrintAll();

Console.WriteLine("Размер очереди: " + strProcessor.Size);
Console.WriteLine("Заполнена: " + strProcessor.IsFull);


class MyFixedQueue<T>
{
    private T[] items;
    private int head = 0;
    private int tail = 0;
    private int count = 0;
    private int capacity;

    public int Count
    {
        get { return count; }
    }

    public int Capacity
    {
        get { return capacity; }
    }

    public bool IsFull
    {
        get { return count == capacity; }
    }

    public bool IsEmpty
    {
        get { return count == 0; }
    }

    public MyFixedQueue(int capacity)
    {
        if (capacity <= 0)
            throw new ArgumentException("Размер должен быть больше нуля");
        this.capacity = capacity;
        items = new T[capacity];
    }

    public void Enqueue(T item)
    {
        if (IsFull)
        {
            Console.WriteLine("  Очередь переполнена, удаляем: " + items[head]);
            head = (head + 1) % capacity;
            count--;
        }
        items[tail] = item;
        tail = (tail + 1) % capacity;
        count++;
    }

    public T Dequeue()
    {
        if (IsEmpty)
            throw new InvalidOperationException("Очередь пуста");
        T item = items[head];
        head = (head + 1) % capacity;
        count--;
        return item;
    }

    public T Peek()
    {
        if (IsEmpty)
            throw new InvalidOperationException("Очередь пуста");
        return items[head];
    }

    public bool Contains(T item)
    {
        for (int i = 0; i < count; i++)
        {
            int index = (head + i) % capacity;
            if (items[index].Equals(item))
                return true;
        }
        return false;
    }

    public List<T> ToList()
    {
        List<T> result = new List<T>();
        for (int i = 0; i < count; i++)
        {
            result.Add(items[(head + i) % capacity]);
        }
        return result;
    }
}

class FixedQueueProcessor<T>
{
    private MyFixedQueue<T> queue;

    public int Size
    {
        get { return queue.Count; }
    }

    public bool IsFull
    {
        get { return queue.IsFull; }
    }

    public FixedQueueProcessor(int capacity)
    {
        queue = new MyFixedQueue<T>(capacity);
    }

    public void Add(T item)
    {
        Console.WriteLine("Добавляем: " + item);
        queue.Enqueue(item);
    }

    public T Dequeue()
    {
        return queue.Dequeue();
    }

    public T Peek()
    {
        return queue.Peek();
    }

    public bool Contains(T item)
    {
        return queue.Contains(item);
    }

    public void PrintAll()
    {
        List<T> items = queue.ToList();
        Console.Write("  Очередь [");
        for (int i = 0; i < items.Count; i++)
        {
            Console.Write(items[i]);
            if (i < items.Count - 1)
                Console.Write(", ");
        }
        Console.WriteLine("] (голова слева)");
    }
}
