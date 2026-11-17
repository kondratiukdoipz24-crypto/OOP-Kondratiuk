
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public static class PerformanceTest
{
    public static void CompareSearchMethods(
        List<Document> documents,
        string searchWord)
    {
        InvertedIndex index = new InvertedIndex();

        foreach (Document doc in documents)
        {
            index.AddDocument(doc);
        }

        const int repetitions = 1000;

        List<Document> linearResults = new List<Document>();
        List<Document> indexResults = new List<Document>();

        Stopwatch sw = new Stopwatch();

        sw.Start();

        for (int i = 0; i < repetitions; i++)
        {
            linearResults = documents
                .Where(doc => TextProcessor.RemoveStopWords(
                    TextProcessor.Tokenize(doc.Content))
                    .Contains(searchWord.ToLowerInvariant()))
                .ToList();
        }

        sw.Stop();

        double linearTime = sw.ElapsedTicks / (double)repetitions;

        sw.Restart();

        for (int i = 0; i < repetitions; i++)
        {
            indexResults = index.SearchSingleWord(searchWord);
        }

        sw.Stop();

        double indexTime = sw.ElapsedTicks / (double)repetitions;

        Console.WriteLine($"Кількість документів: {documents.Count}");
        Console.WriteLine($"Linear search: {linearTime:F2} ticks");
        Console.WriteLine($"Inverted Index: {indexTime:F2} ticks");
        Console.WriteLine($"Linear found: {linearResults.Count}");
        Console.WriteLine($"Index found: {indexResults.Count}");
        Console.WriteLine(
            $"Результати однакові: {linearResults.Count == indexResults.Count}");
        Console.WriteLine();
    }
}
