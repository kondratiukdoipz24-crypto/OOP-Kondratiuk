
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Timeout;

class Program
{
    static async Task Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("=== САМОСТІЙНА РОБОТА №19 ===");
        Console.WriteLine("Дизайн винятків і політики retry");
        Console.WriteLine();

        DemonstrateCustomException();

        Console.WriteLine();
        Console.WriteLine("==================================");
        Console.WriteLine("СЦЕНАРІЙ 1. RETRY ДЛЯ ЗОВНІШНЬОГО API");
        Console.WriteLine("==================================");

        ScenarioRetry();

        Console.WriteLine();
        Console.WriteLine("==================================");
        Console.WriteLine("СЦЕНАРІЙ 2. CIRCUIT BREAKER");
        Console.WriteLine("==================================");

        ScenarioCircuitBreaker();

        Console.WriteLine();
        Console.WriteLine("==================================");
        Console.WriteLine("СЦЕНАРІЙ 3. RETRY + TIMEOUT");
        Console.WriteLine("==================================");

        await ScenarioRetryTimeout();

        Console.WriteLine();
        Console.WriteLine("=== ПРОГРАМУ ЗАВЕРШЕНО ===");
    }

    // Демонстрація власного типу винятку
    static void DemonstrateCustomException()
    {
        Console.WriteLine("=== ВЛАСНИЙ ТИП ВИНЯТКУ ===");

        BankAccount account = new BankAccount(500);

        try
        {
            Console.WriteLine(
                $"Поточний баланс: {account.Balance:F2} грн.");

            account.Withdraw(800);
        }
        catch (InsufficientFundsException ex)
        {
            Console.WriteLine("Помилка: " + ex.Message);

            Console.WriteLine(
                $"Потрібно коштів: {ex.RequestedAmount:F2} грн.");

            Console.WriteLine(
                $"Доступно коштів: {ex.CurrentBalance:F2} грн.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Інша помилка: " + ex.Message);
        }
        finally
        {
            Console.WriteLine("Операцію з рахунком завершено.");
        }
    }

    // Сценарій 1: повторні спроби підключення до API
    static void ScenarioRetry()
    {
        int attempts = 0;

        var pipeline = new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                ShouldHandle = new PredicateBuilder()
                    .Handle<HttpRequestException>(),

                MaxRetryAttempts = 3,

                Delay = TimeSpan.FromMilliseconds(300),

                BackoffType = DelayBackoffType.Exponential,

                UseJitter = false,

                OnRetry = args =>
                {
                    Console.WriteLine(
                        $"Повторна спроба №{args.AttemptNumber + 1}. " +
                        $"Помилка: {args.Outcome.Exception?.Message}");

                    return default;
                }
            })
            .Build();

        try
        {
            pipeline.Execute(() =>
            {
                attempts++;

                Console.WriteLine(
                    $"Виклик зовнішнього API. Спроба: {attempts}");

                if (attempts < 3)
                {
                    throw new HttpRequestException(
                        "Сервер тимчасово недоступний.");
                }

                Console.WriteLine("API успішно відповів.");
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                "Не вдалося виконати запит: " + ex.Message);
        }
    }

    // Сценарій 2: Circuit Breaker для недоступної бази даних
    static void ScenarioCircuitBreaker()
    {
        var pipeline = new ResiliencePipelineBuilder()
            .AddCircuitBreaker(new CircuitBreakerStrategyOptions
            {
                ShouldHandle = new PredicateBuilder()
                    .Handle<HttpRequestException>(),

                FailureRatio = 0.5,

                SamplingDuration = TimeSpan.FromSeconds(10),

                MinimumThroughput = 2,

                BreakDuration = TimeSpan.FromSeconds(5),

                OnOpened = args =>
                {
                    Console.WriteLine(
                        "Circuit Breaker відкрито. " +
                        "Запити тимчасово блокуються.");

                    return default;
                },

                OnClosed = args =>
                {
                    Console.WriteLine(
                        "Circuit Breaker закрито. " +
                        "З'єднання відновлено.");

                    return default;
                },

                OnHalfOpened = args =>
                {
                    Console.WriteLine(
                        "Circuit Breaker у стані Half-Open. " +
                        "Перевірка сервісу.");

                    return default;
                }
            })
            .Build();

        for (int i = 1; i <= 5; i++)
        {
            try
            {
                Console.WriteLine(
                    $"\nСпроба підключення до бази даних №{i}");

                pipeline.Execute(() =>
                {
                    Console.WriteLine(
                        "Виконується запит до бази даних...");

                    throw new HttpRequestException(
                        "База даних тимчасово недоступна.");
                });
            }
            catch (BrokenCircuitException)
            {
                Console.WriteLine(
                    "Запит відхилено: Circuit Breaker відкритий.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    "Помилка підключення: " + ex.Message);
            }
        }
    }

    // Сценарій 3: повторні спроби з обмеженням часу
    static async Task ScenarioRetryTimeout()
    {
        int attempts = 0;

        var pipeline = new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                ShouldHandle = new PredicateBuilder()
                    .Handle<TimeoutRejectedException>(),

                MaxRetryAttempts = 2,

                Delay = TimeSpan.FromMilliseconds(200),

                OnRetry = args =>
                {
                    Console.WriteLine(
                        $"Повторна спроба №{args.AttemptNumber + 1} " +
                        "після перевищення часу очікування.");

                    return default;
                }
            })
            .AddTimeout(TimeSpan.FromMilliseconds(300))
            .Build();

        try
        {
            await pipeline.ExecuteAsync(async cancellationToken =>
            {
                attempts++;

                Console.WriteLine(
                    $"Надсилання повідомлення. Спроба: {attempts}");

                if (attempts == 1)
                {
                    await Task.Delay(1000, cancellationToken);
                }
                else
                {
                    await Task.Delay(100, cancellationToken);
                }

                Console.WriteLine("Повідомлення успішно надіслано.");

            }, CancellationToken.None);
        }
        catch (TimeoutRejectedException)
        {
            Console.WriteLine(
                "Операцію скасовано: час очікування перевищено.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                "Помилка надсилання: " + ex.Message);
        }
    }
}
