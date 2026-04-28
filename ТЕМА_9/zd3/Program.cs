using System;
using System.IO;
using System.Collections.Generic;

string filePath = "file.data";

if (!File.Exists(filePath))
{
    File.WriteAllText(filePath,
        "1|1500.50" + Environment.NewLine +
        "2|320.00" + Environment.NewLine +
        "3|9999.99" + Environment.NewLine +
        "4|75.25" + Environment.NewLine +
        "5|4200.00" + Environment.NewLine);
    Console.WriteLine("Создан file.data");
    Console.WriteLine();
}

TransactionFileReader reader = new TransactionFileReader(filePath);
List<Transaction> all = reader.ReadTransactions();

Console.WriteLine("Все транзакции (" + all.Count + "):");
foreach (Transaction t in all)
    Console.WriteLine("Id=" + t.Id + ", Amount=" + t.Amount.ToString("F2"));

Console.WriteLine();
Console.Write("Введите минимальную сумму: ");
decimal min = decimal.Parse(Console.ReadLine());

TransactionProcessor processor = new TransactionProcessor(all);
List<Transaction> filtered = processor.FilterByAmount(min);

Console.WriteLine();
Console.WriteLine("Транзакции с суммой >= " + min.ToString("F2") + " (" + filtered.Count + "):");
foreach (Transaction t in filtered)
    Console.WriteLine("Id=" + t.Id + ", Amount=" + t.Amount.ToString("F2"));


class Transaction
{
    public int Id;
    public decimal Amount;
}

class TransactionFileReader
{
    private string path;

    public TransactionFileReader(string path)
    {
        this.path = path;
    }

    public List<Transaction> ReadTransactions()
    {
        List<Transaction> result = new List<Transaction>();
        string[] lines = File.ReadAllLines(path);

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] parts = line.Split('|');
            if (parts.Length != 2) continue;

            Transaction t = new Transaction();
            t.Id = int.Parse(parts[0]);
            t.Amount = decimal.Parse(parts[1]);
            result.Add(t);
        }

        return result;
    }
}

class TransactionProcessor
{
    private List<Transaction> transactions;

    public TransactionProcessor(List<Transaction> transactions)
    {
        this.transactions = transactions;
    }

    public List<Transaction> FilterByAmount(decimal minAmount)
    {
        List<Transaction> result = new List<Transaction>();
        foreach (Transaction t in transactions)
        {
            if (t.Amount >= minAmount)
                result.Add(t);
        }
        return result;
    }
}
