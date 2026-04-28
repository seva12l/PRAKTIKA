using System;
using System.Collections.Generic;

OrderSystem system = new OrderSystem();

Customer customer = new Customer("Иван");
Chef chef = new Chef();
Waiter waiter = new Waiter();

system.Subscribe(customer);
system.Subscribe(chef);
system.Subscribe(waiter);

system.NewOrder("Пицца Маргарита");
Console.WriteLine();

system.UpdateStatus("Пицца Маргарита", "Готовится");
Console.WriteLine();

system.UpdateStatus("Пицца Маргарита", "Готов");
Console.WriteLine();

system.Unsubscribe(chef);

system.NewOrder("Салат Цезарь");


interface IOrderObserver
{
    void OnOrderUpdate(string orderName, string status);
}

class OrderSystem
{
    private List<IOrderObserver> observers = new List<IOrderObserver>();

    public void Subscribe(IOrderObserver observer)
    {
        observers.Add(observer);
    }

    public void Unsubscribe(IOrderObserver observer)
    {
        observers.Remove(observer);
    }

    public void NewOrder(string orderName)
    {
        Console.WriteLine("Новый заказ: " + orderName);
        Notify(orderName, "Принят");
    }

    public void UpdateStatus(string orderName, string status)
    {
        Console.WriteLine("Статус заказа \"" + orderName + "\" изменён на: " + status);
        Notify(orderName, status);
    }

    private void Notify(string orderName, string status)
    {
        foreach (IOrderObserver o in observers)
            o.OnOrderUpdate(orderName, status);
    }
}

class Customer : IOrderObserver
{
    private string name;

    public Customer(string name)
    {
        this.name = name;
    }

    public void OnOrderUpdate(string orderName, string status)
    {
        Console.WriteLine("  Клиент " + name + ": заказ \"" + orderName + "\" - " + status);
    }
}

class Chef : IOrderObserver
{
    public void OnOrderUpdate(string orderName, string status)
    {
        if (status == "Принят")
            Console.WriteLine("  Повар: начинаю готовить \"" + orderName + "\"");
        else if (status == "Готовится")
            Console.WriteLine("  Повар: продолжаю готовить \"" + orderName + "\"");
        else if (status == "Готов")
            Console.WriteLine("  Повар: \"" + orderName + "\" готов, передаю официанту");
    }
}

class Waiter : IOrderObserver
{
    public void OnOrderUpdate(string orderName, string status)
    {
        if (status == "Принят")
            Console.WriteLine("  Официант: принял заказ \"" + orderName + "\"");
        else if (status == "Готов")
            Console.WriteLine("  Официант: несу заказ \"" + orderName + "\" клиенту");
    }
}
