
using System;
using System.Collections.Generic;

class Transaction
{
    public int Id { get; set; }
    public double Amount { get; set; }
    public string Description { get; set; }

    public Transaction(int id, double amount, string description)
    {
        Id = id;
        Amount = amount;
        Description = description;
    }

    public override string ToString()
    {
        return $"ID: {Id}, Сума: {Amount} грн, Опис: {Description}";
    }
}

class TransactionManager
{
    private HashSet<int> transactionIds = new HashSet<int>();
    private Queue<Transaction> transactionQueue = new Queue<Transaction>();
    private Stack<Transaction> transactionHistory = new Stack<Transaction>();

    public void AddTransaction(Transaction transaction)
    {
        if (transactionIds.Add(transaction.Id))
        {
            transactionQueue.Enqueue(transaction);
            Console.WriteLine($"Транзакцію додано: {transaction}");
        }
        else
        {
            Console.WriteLine($"Транзакція з ID {transaction.Id} вже існує!");
        }
    }

    public void ProcessTransaction()
    {
        if (transactionQueue.Count > 0)
        {
            Transaction transaction = transactionQueue.Dequeue();
            transactionHistory.Push(transaction);

            Console.WriteLine($"Оброблено: {transaction}");
        }
        else
        {
            Console.WriteLine("Черга транзакцій порожня.");
        }
    }

    public void CancelLastTransaction()
    {
        if (transactionHistory.Count > 0)
        {
            Transaction transaction = transactionHistory.Pop();
            Console.WriteLine($"Скасовано транзакцію: {transaction}");
        }
        else
        {
            Console.WriteLine("Немає транзакцій для скасування.");
        }
    }

    public void ShowQueue()
    {
        Console.WriteLine("\nЧерга транзакцій:");

        foreach (var transaction in transactionQueue)
        {
            Console.WriteLine(transaction);
        }
    }

    public void ShowHistory()
    {
        Console.WriteLine("\nІсторія оброблених транзакцій:");

        foreach (var transaction in transactionHistory)
        {
            Console.WriteLine(transaction);
        }
    }

    public void CheckTransaction(int id)
    {
        if (transactionIds.Contains(id))
            Console.WriteLine($"Транзакція з ID {id} існує.");
        else
            Console.WriteLine($"Транзакції з ID {id} немає.");
    }
}

class Program
{
    static void Main()
    {
        TransactionManager manager = new TransactionManager();

        Console.WriteLine("=== СИСТЕМА ОБРОБКИ ТРАНЗАКЦІЙ ===");

        Console.WriteLine("\n1. HASHSET - унікальні ID транзакцій");

        manager.AddTransaction(new Transaction(1, 500, "Оплата товару"));
        manager.AddTransaction(new Transaction(2, 250, "Переказ коштів"));
        manager.AddTransaction(new Transaction(3, 1000, "Поповнення рахунку"));

        manager.AddTransaction(new Transaction(1, 300, "Повторна транзакція"));

        manager.CheckTransaction(2);
        manager.CheckTransaction(5);

        Console.WriteLine("\n2. QUEUE - черга транзакцій");

        manager.ShowQueue();

        manager.ProcessTransaction();
        manager.ProcessTransaction();

        manager.ShowQueue();

        Console.WriteLine("\n3. STACK - історія транзакцій");

        manager.ShowHistory();

        manager.CancelLastTransaction();

        manager.ShowHistory();

        Console.WriteLine("\n=== ЗАВЕРШЕННЯ РОБОТИ ===");
    }
}
