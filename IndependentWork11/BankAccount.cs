
using System;

public class BankAccount
{
    public decimal Balance { get; private set; }

    public BankAccount(decimal balance)
    {
        Balance = balance;
    }

    public void Withdraw(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException(
                "Сума зняття повинна бути більшою за нуль.");
        }

        if (amount > Balance)
        {
            throw new InsufficientFundsException(
                "Недостатньо коштів на рахунку.",
                amount,
                Balance);
        }

        Balance -= amount;

        Console.WriteLine(
            $"Знято {amount:F2} грн. Залишок: {Balance:F2} грн.");
    }
}
