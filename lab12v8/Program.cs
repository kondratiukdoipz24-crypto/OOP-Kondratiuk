
using System;
using System.IO;
using System.Net.Http;
using System.Threading;

class FileProcessor
{
    private int attempts = 0;

    public void SaveUserData(string path, string userData)
    {
        attempts++;

        if (attempts <= 4)
        {
            throw new IOException("Помилка під час збереження файлу.");
        }

        File.WriteAllText(path, userData);
        Console.WriteLine("Дані користувача успішно збережено у файл.");
    }
}

class NetworkClient
{
    private int attempts = 0;

    public bool PostUserData(string url, string userData)
    {
        attempts++;

        if (attempts <= 2)
        {
            throw new HttpRequestException("Помилка відправлення даних на сервер.");
        }

        Console.WriteLine("Дані користувача успішно відправлено на сервер.");
        return true;
    }
}

static class RetryHelper
{
    public static T ExecuteWithRetry<T>(
        Func<T> operation,
        int retryCount = 3,
        TimeSpan initialDelay = default,
        Func<Exception, bool>? shouldRetry = null)
    {
        if (retryCount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(retryCount));
        }

        if (initialDelay == default)
        {
            initialDelay = TimeSpan.FromMilliseconds(200);
        }

        if (initialDelay < TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(initialDelay));
        }

        if (shouldRetry == null)
        {
            shouldRetry = exception => true;
        }

        for (int attempt = 1; ; attempt++)
        {
            try
            {
                Console.WriteLine($"Спроба №{attempt}");
                return operation();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");

                if (attempt > retryCount || !shouldRetry(ex))
                {
                    Console.WriteLine("Повторення неможливе.");
                    throw;
                }

                double delay = initialDelay.TotalMilliseconds *
                               Math.Pow(2, attempt - 1);

                Console.WriteLine($"Повторна спроба через {delay} мс.");

                Thread.Sleep(TimeSpan.FromMilliseconds(delay));
            }
        }
    }
}

class Program
{
    static void Main()
    {
        FileProcessor fileProcessor = new FileProcessor();
        NetworkClient networkClient = new NetworkClient();

        Func<Exception, bool> shouldRetry = exception =>
            exception is IOException ||
            exception is HttpRequestException;

        Console.WriteLine("=== ЗБЕРЕЖЕННЯ ДАНИХ КОРИСТУВАЧА ===");

        try
        {
            RetryHelper.ExecuteWithRetry(
                () =>
                {
                    fileProcessor.SaveUserData(
                        "UserData.txt",
                        "Ім'я: Дарина\nВік: 20");

                    return true;
                },
                retryCount: 4,
                initialDelay: TimeSpan.FromMilliseconds(200),
                shouldRetry: shouldRetry
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Не вдалося зберегти файл: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Обробку файлу завершено.\n");
        }

        Console.WriteLine("=== ВІДПРАВЛЕННЯ ДАНИХ НА СЕРВЕР ===");

        try
        {
            RetryHelper.ExecuteWithRetry(
                () => networkClient.PostUserData(
                    "https://example.com/users",
                    "Ім'я: Дарина"),
                retryCount: 2,
                initialDelay: TimeSpan.FromMilliseconds(200),
                shouldRetry: shouldRetry
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Не вдалося відправити дані: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Обробку мережевого запиту завершено.\n");
        }

        Console.WriteLine("=== ПЕРЕВІРКА shouldRetry ===");

        try
        {
            RetryHelper.ExecuteWithRetry<bool>(
                () =>
                {
                    throw new InvalidOperationException(
                        "Ця помилка не підтримує повторення.");
                },
                retryCount: 3,
                initialDelay: TimeSpan.FromMilliseconds(200),
                shouldRetry: shouldRetry
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Оброблено виняток: {ex.Message}");
        }

        Console.WriteLine("\nПрограму завершено.");
    }
}
