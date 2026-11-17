
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== LINQ ТА PLINQ: ПРОДУКТИВНІСТЬ І БЕЗПЕКА ===");
        Console.WriteLine();

        // Перевірка на невеликій кількості даних для прогріву JIT
        List<int> warmupData = GenerateNumbers(10000);

        AnalyzeWithLinq(warmupData);
        AnalyzeWithPlinq(warmupData);

        // Експерименти на двох обов'язкових розмірах
        int[] sizes = { 1_000_000, 10_000_000 };

        foreach (int size in sizes)
        {
            Console.WriteLine();
            Console.WriteLine("========================================");
            Console.WriteLine($"КІЛЬКІСТЬ ЕЛЕМЕНТІВ: {size:N0}");
            Console.WriteLine("========================================");

            List<int> numbers = GenerateNumbers(size);

            Console.WriteLine("Дані згенеровано.");

            // Звичайний LINQ
            AnalysisResult linqResult = Measure(
                "Звичайний LINQ",
                () => AnalyzeWithLinq(numbers));

            // Паралельний PLINQ
            AnalysisResult plinqResult = Measure(
                "PLINQ",
                () => AnalyzeWithPlinq(numbers));

            Console.WriteLine();
            Console.WriteLine("Результати:");

            Console.WriteLine(
                $"LINQ: знайдено {linqResult.Count:N0} простих чисел");

            Console.WriteLine(
                $"PLINQ: знайдено {plinqResult.Count:N0} простих чисел");

            Console.WriteLine(
                $"Сума результатів LINQ: {linqResult.Sum:N0}");

            Console.WriteLine(
                $"Сума результатів PLINQ: {plinqResult.Sum:N0}");

            Console.WriteLine(
                "Результати однакові: " +
                (linqResult.Count == plinqResult.Count &&
                 linqResult.Sum == plinqResult.Sum));
        }

        // Демонстрація проблеми потокобезпечності
        Console.WriteLine();
        Console.WriteLine("========================================");
        Console.WriteLine("ПЕРЕВІРКА ПОТОКОБЕЗПЕЧНОСТІ");
        Console.WriteLine("========================================");

        DemonstrateUnsafeCounter();

        DemonstrateSafeCounter();

        Console.WriteLine();
        Console.WriteLine("=== ДОСЛІДЖЕННЯ ЗАВЕРШЕНО ===");
    }

    // Генерація випадкових чисел
    static List<int> GenerateNumbers(int count)
    {
        Random random = new Random(42);

        List<int> numbers = new List<int>(count);

        for (int i = 0; i < count; i++)
        {
            numbers.Add(random.Next(2, 10001));
        }

        return numbers;
    }

    // Перевірка, чи є число простим
    static bool IsPrime(int number)
    {
        if (number < 2)
            return false;

        if (number == 2)
            return true;

        if (number % 2 == 0)
            return false;

        for (int i = 3; i * i <= number; i += 2)
        {
            if (number % i == 0)
                return false;
        }

        return true;
    }

    // Результат обчислень
    static AnalysisResult AnalyzeWithLinq(List<int> numbers)
    {
        var results = numbers
            .Where(number => IsPrime(number))
            .Select(number => (long)number * number)
            .ToList();

        return new AnalysisResult
        {
            Count = results.Count,
            Sum = results.Sum()
        };
    }

    // Та сама операція за допомогою PLINQ
    static AnalysisResult AnalyzeWithPlinq(List<int> numbers)
    {
        var results = numbers
            .AsParallel()
            .Where(number => IsPrime(number))
            .Select(number => (long)number * number)
            .ToList();

        return new AnalysisResult
        {
            Count = results.Count,
            Sum = results.Sum()
        };
    }

    // Вимірювання часу виконання
    static T Measure<T>(string name, Func<T> operation)
    {
        Stopwatch stopwatch = new Stopwatch();

        stopwatch.Start();

        T result = operation();

        stopwatch.Stop();

        Console.WriteLine(
            $"{name}: {stopwatch.Elapsed.TotalMilliseconds:F2} мс");

        return result;
    }

    // Демонстрація небезпечного доступу до спільної змінної
    static void DemonstrateUnsafeCounter()
    {
        List<int> numbers = GenerateNumbers(500000);

        int counter = 0;

        numbers
            .AsParallel()
            .ForAll(number =>
            {
                counter++;
            });

        Console.WriteLine();
        Console.WriteLine("Небезпечний лічильник:");

        Console.WriteLine(
            $"Очікувана кількість: {numbers.Count:N0}");

        Console.WriteLine(
            $"Отримана кількість: {counter:N0}");

        Console.WriteLine(
            "Результат може бути неправильним через одночасне " +
            "змінювання спільної змінної.");
    }

    // Виправлення за допомогою Interlocked
    static void DemonstrateSafeCounter()
    {
        List<int> numbers = GenerateNumbers(500000);

        int counter = 0;

        numbers
            .AsParallel()
            .ForAll(number =>
            {
                Interlocked.Increment(ref counter);
            });

        Console.WriteLine();
        Console.WriteLine("Безпечний лічильник:");

        Console.WriteLine(
            $"Очікувана кількість: {numbers.Count:N0}");

        Console.WriteLine(
            $"Отримана кількість: {counter:N0}");

        Console.WriteLine(
            "Результат правильний завдяки Interlocked.");
    }
}

// Клас для збереження результатів аналізу
public class AnalysisResult
{
    public int Count { get; set; }
    public long Sum { get; set; }
}
