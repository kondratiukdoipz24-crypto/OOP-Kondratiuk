
using System;

public class Student
{
    private string _name;
    private int _age;

    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public int Age
    {
        get { return _age; }
    }

    public Student(string name, int age)
    {
        _name = name;
        _age = age;
    }

    public void PrintInfo()
    {
        Console.WriteLine("Студент: " + Name + ", вік: " + Age);
    }
}

public class Book
{
    private string _title;
    private string _author;

    public string Title
    {
        get { return _title; }
        set { _title = value; }
    }

    public string Author
    {
        get { return _author; }
    }

    public Book(string title, string author)
    {
        _title = title;
        _author = author;
    }

    public void PrintInfo()
    {
        Console.WriteLine("Книга: " + Title + ", автор: " + Author);
    }
}

public class BankAccount
{
    private string _owner;
    private double _balance;

    public string Owner
    {
        get { return _owner; }
        set { _owner = value; }
    }

    public double Balance
    {
        get { return _balance; }
    }

    public BankAccount(string owner, double balance)
    {
        _owner = owner;
        _balance = balance;
    }

    public bool HasEnoughMoney(double amount)
    {
        return _balance >= amount;
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        Student student = new Student("Дарина", 17);
        Book book = new Book("Кобзар", "Тарас Шевченко");
        BankAccount account = new BankAccount("Дарина", 5000);

        student.PrintInfo();
        book.PrintInfo();

        Console.WriteLine("Власник рахунку: " + account.Owner);
        Console.WriteLine("Баланс: " + account.Balance + " грн");

        double purchase = 1200;
        bool enoughMoney = account.HasEnoughMoney(purchase);

        Console.WriteLine("Чи вистачає грошей на покупку " + purchase + " грн: " + enoughMoney);
    }
}