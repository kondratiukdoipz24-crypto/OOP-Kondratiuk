
using System;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("САМОСТІЙНА РОБОТА №20");
        Console.WriteLine("Реалізація exponential backoff");
        Console.WriteLine();

        RetryPolicy retryPolicy = new RetryPolicy(
            maxRetries: 3,
            initialDelay: TimeSpan.FromMilliseconds(500),
            jitterFactor: 0.2);

        // Сценарій 1: спочатку помилки, потім успіх

        Console.WriteLine("===== СЦЕНАРІЙ 1: УСПІХ ПІСЛЯ ПОВТОРІВ =====");

        int attempts = 0;

        try
        {
            string result = retryPolicy.Execute(
                () =>
                {
                    attempts++;

                    Console.WriteLine(
                        $"Виконується операція. Спроба: {attempts}");

                    if (attempts < 3)
                    {
                        throw new InvalidOperationException(
                            "Сервер тимчасово недоступний.");
                    }

                    return "Дані успішно отримано!";
                },
                exception => exception is InvalidOperationException);

            Console.WriteLine($"Результат: {result}");
        }
        catch (Exception exception)
        {
            Console.WriteLine(
                $"Остаточна помилка: {exception.Message}");
        }

        Console.WriteLine();

        // Сценарій 2: операція завжди завершується помилкою

        Console.WriteLine("===== СЦЕНАРІЙ 2: ОСТАТОЧНА НЕВДАЧА =====");

        int failedAttempts = 0;

        try
        {
            string result = retryPolicy.Execute<string>(
                () =>
                {
                    failedAttempts++;

                    Console.WriteLine(
                        $"Виконується операція. Спроба: {failedAttempts}");

                    throw new InvalidOperationException(
                        "Не вдалося підключитися до сервера.");
                },
                exception => exception is InvalidOperationException);

            Console.WriteLine($"Результат: {result}");
        }
        catch (Exception exception)
        {
            Console.WriteLine(
                $"Операція остаточно завершилася помилкою: {exception.Message}");
        }

        Console.WriteLine();

        // Сценарій 3: повторення заборонене

        Console.WriteLine("===== СЦЕНАРІЙ 3: SHOULDRETRY =====");

        try
        {
            retryPolicy.Execute<string>(
                () =>
                {
                    Console.WriteLine("Перевірка даних...");

                    throw new ArgumentException(
                        "Передано неправильні дані.");
                },
                exception => exception is InvalidOperationException);
        }
        catch (Exception exception)
        {
            Console.WriteLine(
                $"Повторення не виконувалося: {exception.Message}");
        }

        Console.WriteLine();
        Console.WriteLine("Демонстрацію завершено.");
    }
}
