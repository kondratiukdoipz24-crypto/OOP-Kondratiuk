
using System;
using System.Collections.Generic;

// Власний виняток для помилки підключення до бази даних
class DatabaseConnectionException : Exception
{
    public string ConnectionString { get; }

    public DatabaseConnectionException()
        : base("Не вдалося підключитися до бази даних.")
    {
        ConnectionString = "";
    }

    public DatabaseConnectionException(string message)
        : base(message)
    {
        ConnectionString = "";
    }

    public DatabaseConnectionException(string message, Exception innerException)
        : base(message, innerException)
    {
        ConnectionString = "";
    }

    public DatabaseConnectionException(string connectionString, bool isConnectionError)
        : base("З'єднання з базою даних не встановлено.")
    {
        ConnectionString = connectionString;
    }
}

// Узагальнений клас Result<T>
class Result<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public string Error { get; }

    private Result(T? value, bool isSuccess, string error)
    {
        Value = value;
        IsSuccess = isSuccess;
        Error = error;
    }

    // Успішний результат
    public static Result<T> Success(T value)
    {
        return new Result<T>(value, true, "");
    }

    // Результат із повідомленням про помилку
    public static Result<T> Failure(string error)
    {
        return new Result<T>(default, false, error);
    }

    // Результат на основі винятку
    public static Result<T> Failure(Exception ex)
    {
        return new Result<T>(default, false, ex.Message);
    }
}

// Сервіс для роботи з базою даних
class DataService
{
    private readonly string connectionString;
    private readonly bool isConnected;
    private readonly List<string> database = new List<string>();

    public DataService(string connectionString, bool isConnected)
    {
        this.connectionString = connectionString;
        this.isConnected = isConnected;
    }

    // Отримання даних із бази даних
    public List<string> GetData(string query)
    {
        if (!isConnected)
        {
            throw new DatabaseConnectionException(connectionString, true);
        }

        if (string.IsNullOrWhiteSpace(query))
        {
            throw new ArgumentException("Запит не може бути порожнім.");
        }

        return new List<string>(database);
    }

    // Збереження даних із використанням Result<T>
    public Result<bool> SaveData(string data)
    {
        if (string.IsNullOrWhiteSpace(data))
        {
            return Result<bool>.Failure("Дані не можуть бути порожніми.");
        }

        database.Add(data);

        return Result<bool>.Success(true);
    }
}

class Program
{
    static void Main()
    {
        string connectionString = "Server=localhost;Database=TestDB;";

        DataService service = new DataService(
            connectionString,
            false
        );

        Console.WriteLine("=== ЛАБОРАТОРНА РОБОТА №13 ===");
        Console.WriteLine("Власні винятки та патерн Result\n");

        // Демонстрація власного винятку
        Console.WriteLine("1. Перевірка підключення до бази даних:");

        try
        {
            List<string> data = service.GetData("SELECT * FROM Users");

            foreach (string item in data)
            {
                Console.WriteLine(item);
            }
        }
        catch (DatabaseConnectionException ex)
        {
            Console.WriteLine($"Помилка: {ex.Message}");
            Console.WriteLine($"Рядок підключення: {ex.ConnectionString}");
        }

        // Демонстрація успішного Result
        Console.WriteLine("\n2. Збереження правильних даних:");

        Result<bool> successResult = service.SaveData("Користувач: Дарина");

        if (successResult.IsSuccess)
        {
            Console.WriteLine("Дані успішно збережено.");
        }
        else
        {
            Console.WriteLine($"Помилка: {successResult.Error}");
        }

        // Демонстрація невдалого Result
        Console.WriteLine("\n3. Збереження порожніх даних:");

        Result<bool> failureResult = service.SaveData("");

        if (failureResult.IsSuccess)
        {
            Console.WriteLine("Дані успішно збережено.");
        }
        else
        {
            Console.WriteLine($"Помилка: {failureResult.Error}");
        }

        // Демонстрація Failure(Exception)
        Console.WriteLine("\n4. Створення Result із винятку:");

        Exception exception = new Exception("Помилка обробки даних.");

        Result<bool> exceptionResult = Result<bool>.Failure(exception);

        if (!exceptionResult.IsSuccess)
        {
            Console.WriteLine($"Помилка: {exceptionResult.Error}");
        }

        Console.WriteLine("\nПрограму завершено.");
    }
}
