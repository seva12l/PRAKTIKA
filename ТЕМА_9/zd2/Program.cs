using System;
using System.IO;

string filePath = "file.data";

TransactionFileWriter writer = new TransactionFileWriter(filePath);

writer.AppendTransaction(new Transaction { Id = 1, Amount = 1500.50m });
writer.AppendTransaction(new Transaction { Id = 2, Amount = 320.00m });
writer.AppendTransaction(new Transaction { Id = 3, Amount = 9999.99m });
writer.AppendTransaction(new Transaction { Id = 4, Amount = 75.25m });
writer.AppendTransaction(new Transaction { Id = 5, Amount = 4200.00m });

Console.WriteLine();
Console.WriteLine("Содержимое file.data:");
Console.WriteLine(File.ReadAllText(filePath));


class Transaction
{
    public int Id;
    public decimal Amount;
}

class TransactionFileWriter
{
    private string path;

    public TransactionFileWriter(string path)
    {
        this.path = path;
    }

    public void AppendTransaction(Transaction t)
    {
        string line = t.Id + "|" + t.Amount.ToString("F2");
        File.AppendAllText(path, line + Environment.NewLine);
        Console.WriteLine("Добавлено: Id=" + t.Id + ", Amount=" + t.Amount.ToString("F2"));
    }
}
