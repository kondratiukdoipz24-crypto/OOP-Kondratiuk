using System;
using System.Collections.Generic;

class Transaction
{
    public decimal Amount { get; }

    public Transaction(decimal amount)
    {
        Amount = amount;
    }

    public virtual void Execute()
    {
        Console.WriteLine($"Виконано транзакцію на суму {Amount:F2} грн.");
    }
}

class Deposit : Transaction
{
    public string DestinationAccount { get; }

    public Deposit(decimal amount, string destinationAccount)
        : base(amount)
    {
        DestinationAccount = destinationAccount;
    }

    public override void Execute()
    {
        Console.WriteLine(
            $"Поповнення рахунку {DestinationAccount}: +{Amount:F2} грн.");
    }
}

class Withdrawal : Transaction
{
    public string SourceAccount { get; }

    public Withdrawal(decimal amount, string sourceAccount)
        : base(amount)
    {
        SourceAccount = sourceAccount;
    }

    public override void Execute()
    {
        Console.WriteLine(
            $"Зняття з рахунку {SourceAccount}: -{Amount:F2} грн.");
    }
}

class Transfer : Transaction
{
    public string FromAccount { get; }
    public string ToAccount { get; }

    public Transfer(decimal amount, string fromAccount, string toAccount)
        : base(amount)
    {
        FromAccount = fromAccount;
        ToAccount = toAccount;
    }

    public override void Execute()
    {
        Console.WriteLine(
            $"Переказ {Amount:F2} грн: {FromAccount} -> {ToAccount}");
    }
}

class Program
{
    static void Main()
    {
        List<Transaction> transactions = new List<Transaction>
        {
            new Deposit(1500m, "UA123456"),
            new Withdrawal(300m, "UA123456"),
            new Transfer(700m, "UA123456", "UA987654")
        };

        decimal totalAmount = 0;

        Console.WriteLine("Поліморфні банківські операції");
        Console.WriteLine("--------------------------------");

        foreach (Transaction transaction in transactions)
        {
            transaction.Execute();
            totalAmount += transaction.Amount;
        }

        Console.WriteLine("--------------------------------");
        Console.WriteLine(
            $"Загальна сума всіх транзакцій: {totalAmount:F2} грн.");
    }
}
