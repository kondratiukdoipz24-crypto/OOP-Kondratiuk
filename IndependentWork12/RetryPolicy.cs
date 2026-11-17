
using System;
using System.Threading;

public class RetryPolicy
{
    private readonly int _maxRetries;
    private readonly TimeSpan _initialDelay;
    private readonly double _jitterFactor;
    private readonly Random _random = new Random();

    public RetryPolicy(
        int maxRetries,
        TimeSpan initialDelay,
        double jitterFactor = 0.2)
    {
        if (maxRetries < 0)
            throw new ArgumentOutOfRangeException(nameof(maxRetries));

        if (initialDelay < TimeSpan.Zero)
            throw new ArgumentOutOfRangeException(nameof(initialDelay));

        if (jitterFactor < 0 || jitterFactor > 1)
            throw new ArgumentOutOfRangeException(nameof(jitterFactor));

        _maxRetries = maxRetries;
        _initialDelay = initialDelay;
        _jitterFactor = jitterFactor;
    }

    public T Execute<T>(
        Func<T> operation,
        Func<Exception, bool>? shouldRetry = null)
    {
        if (operation == null)
            throw new ArgumentNullException(nameof(operation));

        int attempt = 0;

        while (true)
        {
            try
            {
                Console.WriteLine($"Спроба №{attempt + 1}");

                T result = operation();

                Console.WriteLine("Операція виконана успішно.");

                return result;
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Помилка: {exception.Message}");

                bool canRetry =
                    shouldRetry == null || shouldRetry(exception);

                if (!canRetry)
                {
                    Console.WriteLine(
                        "Повторення заборонено умовою shouldRetry.");

                    throw;
                }

                if (attempt >= _maxRetries)
                {
                    Console.WriteLine(
                        "Усі повторні спроби вичерпано.");

                    throw;
                }

                double exponentialDelay =
                    _initialDelay.TotalMilliseconds *
                    Math.Pow(2, attempt);

                double jitter =
                    1 + _random.NextDouble() *
                    _jitterFactor * 2 - _jitterFactor;

                double delayMilliseconds =
                    exponentialDelay * jitter;

                TimeSpan delay =
                    TimeSpan.FromMilliseconds(delayMilliseconds);

                Console.WriteLine(
                    $"Очікування перед повтором: {delay.TotalMilliseconds:F0} мс");

                attempt++;

                Thread.Sleep(delay);
            }
        }
    }
}
