
using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        List<Document> documents = new List<Document>
        {
            new Document(
                1,
                "Introduction to C#",
                "C# is a modern object-oriented programming language developed by Microsoft."),

            new Document(
                2,
                "Java Basics",
                "Java is a popular programming language known for its portability and performance."),

            new Document(
                3,
                "Python Overview",
                "Python is a high-level programming language with simple syntax."),

            new Document(
                4,
                "C# Advanced Features",
                "C# supports LINQ, async/await, and many modern programming paradigms."),

            new Document(
                5,
                "Comparing Languages",
                "C#, Java, and Python are among the most popular programming languages today.")
        };

        InvertedIndex index = new InvertedIndex();

        foreach (Document doc in documents)
        {
            index.AddDocument(doc);
        }

        Console.WriteLine("=== INVERTED INDEX ===");
        Console.WriteLine($"Loaded {index.DocumentCount} documents");
        Console.WriteLine($"Index contains {index.UniqueWords} unique words");

        Console.WriteLine("\n=== SEARCH BY ONE WORD ===");

        List<Document> singleResults = index.SearchSingleWord("programming");

        foreach (Document doc in singleResults)
        {
            Console.WriteLine(doc);
        }

        Console.WriteLine("\n=== SEARCH AND ===");

        List<Document> andResults =
            index.SearchMultipleWords("programming", "language");

        foreach (Document doc in andResults)
        {
            Console.WriteLine(doc);
        }

        Console.WriteLine("\n=== SEARCH OR ===");

        List<Document> orResults =
            index.SearchAnyWord("java", "python");

        foreach (Document doc in orResults)
        {
            Console.WriteLine(doc);
        }

        Console.WriteLine("\n=== PERFORMANCE TEST ===");

        List<Document> tenDocuments = GenerateDocuments(10);
        List<Document> thousandDocuments = GenerateDocuments(1000);

        Console.WriteLine("\n--- 10 DOCUMENTS ---");
        PerformanceTest.CompareSearchMethods(tenDocuments, "programming");

        Console.WriteLine("--- 1000 DOCUMENTS ---");
        PerformanceTest.CompareSearchMethods(thousandDocuments, "programming");

        Console.WriteLine("=== INTERACTIVE SEARCH ===");

        while (true)
        {
            Console.Write("\nEnter search query (or 'exit'): ");
            string query = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(query))
                continue;

            if (query.Equals("exit", StringComparison.OrdinalIgnoreCase))
                break;

            string[] words = query.Split(
                ' ',
                StringSplitOptions.RemoveEmptyEntries);

            List<Document> results = index.SearchMultipleWords(words);

            Console.WriteLine("\n--- Search Results (AND) ---");

            foreach (Document doc in results)
            {
                Console.WriteLine(doc);

                string preview = doc.Content.Substring(
                    0, Math.Min(100, doc.Content.Length));

                Console.WriteLine(" " + preview + "...");
            }

            Console.WriteLine($"\nFound {results.Count} documents");
        }
    }

    static List<Document> GenerateDocuments(int count)
    {
        string[] titles =
        {
            "Introduction to C#",
            "Java Basics",
            "Python Overview",
            "C# Advanced Features",
            "Comparing Languages"
        };

        string[] contents =
        {
            "C# is a modern object-oriented programming language developed by Microsoft.",
            "Java is a popular programming language known for its portability and performance.",
            "Python is a high-level programming language with simple syntax.",
            "C# supports LINQ, async/await, and many modern programming paradigms.",
            "C#, Java, and Python are among the most popular programming languages today."
        };

        List<Document> result = new List<Document>();

        for (int i = 0; i < count; i++)
        {
            int templateIndex = i % contents.Length;

            result.Add(new Document(
                i + 1,
                titles[templateIndex] + " " + (i + 1),
                contents[templateIndex]));
        }

        return result;
    }
}
