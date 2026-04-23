using System;

/*
 * Задание 3.
 * Создаем базовый класс BankAccount.
 * Два интерфейса: IDebitAccount и ICreditAccount.
 * Два наследника: SavingsAccount (дебетовый) и LoanAccount (кредитный).
 * Создаем массив счетов и фильтруем только кредитные через проверку типа интерфейса.
*/

interface IDebitAccount
{
    void Deposit(double amount);
}

interface ICreditAccount
{
    void TakeLoan(double amount);
}

class BankAccount
{
    public string Owner;
    public double Balance;

    public BankAccount(string owner, double balance)
    {
        Owner = owner;
        Balance = balance;
    }

    public void ShowInfo()
    {
        Console.WriteLine("Владелец: " + Owner + ", Баланс: " + Balance);
    }
}

class SavingsAccount : BankAccount, IDebitAccount
{
    public SavingsAccount(string owner, double balance) : base(owner, balance) { }

    public void Deposit(double amount)
    {
        Balance += amount;
        Console.WriteLine(Owner + " пополнил счет на " + amount + ". Баланс: " + Balance);
    }
}

class LoanAccount : BankAccount, ICreditAccount
{
    public LoanAccount(string owner, double balance) : base(owner, balance) { }

    public void TakeLoan(double amount)
    {
        Balance += amount;
        Console.WriteLine(Owner + " взял кредит на " + amount + ". Баланс: " + Balance);
    }
}

class Program
{
    static void Main()
    {
        BankAccount[] accounts = new BankAccount[]
        {
            new SavingsAccount("Иванов", 10000),
            new LoanAccount("Петров", 0),
            new SavingsAccount("Сидоров", 25000),
            new LoanAccount("Козлов", 0)
        };

        Console.WriteLine("Все счета:");
        foreach (BankAccount acc in accounts)
            acc.ShowInfo();

        Console.WriteLine();
        Console.WriteLine("Только кредитные счета:");
        foreach (BankAccount acc in accounts)
        {
            if (acc is ICreditAccount creditAcc)
            {
                acc.ShowInfo();
                creditAcc.TakeLoan(5000);
            }
        }

        Console.ReadKey();
    }
}