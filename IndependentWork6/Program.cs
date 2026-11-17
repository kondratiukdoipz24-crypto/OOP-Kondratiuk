
using System;

namespace IndependentWork6
{
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Сценарій 1: Кеш метаданих файлів
            Console.WriteLine("=== КЕШ МЕТАДАНИХ ФАЙЛІВ ===");

            FileMetadataCache fileCache = new FileMetadataCache();

            fileCache.Add("file1", new FileMetadata(
                "document.txt", 1500, DateTime.Now, "ABC123"));

            fileCache.Add("file2", new FileMetadata(
                "photo.png", 5000, DateTime.Now, "DEF456"));

            Console.WriteLine(fileCache.Get("file1"));
            Console.WriteLine("Чи існує file2: " + fileCache.Contains("file2"));
            Console.WriteLine("Кількість файлів: " + fileCache.Count);

            fileCache.Remove("file1");

            Console.WriteLine("Після видалення file1: " + fileCache.Count);


            // Сценарій 2: Унікальні теги
            Console.WriteLine("\n=== УНІКАЛЬНІ ТЕГИ ===");

            TagManager tagManager = new TagManager();

            tagManager.AddTag("article1", "C#");
            tagManager.AddTag("article1", "Programming");
            tagManager.AddTag("article1", "DotNet");

            tagManager.AddTag("article2", "C#");
            tagManager.AddTag("article2", "Programming");
            tagManager.AddTag("article2", "Technology");

            tagManager.AddTag("article1", "c#");

            Console.WriteLine("Усі унікальні теги:");
            Console.WriteLine(string.Join(", ", tagManager.GetAllUniqueTags()));

            Console.WriteLine("\nСпільні теги двох статей:");
            Console.WriteLine(string.Join(", ",
                tagManager.FindCommonTags("article1", "article2")));

            Console.WriteLine("\nТеги тільки першої статті:");
            Console.WriteLine(string.Join(", ",
                tagManager.FindTagsOnlyInArticle("article1", "article2")));

            tagManager.RemoveTag("article1", "DotNet");

            Console.WriteLine("\nПісля видалення DotNet:");
            Console.WriteLine(string.Join(", ",
                tagManager.GetArticleTags("article1")));


            // Сценарій 3: Пошуковий індекс
            Console.WriteLine("\n=== ПОШУКОВИЙ ІНДЕКС ===");

            SearchIndex searchIndex = new SearchIndex();

            searchIndex.AddDocument(1, "C# is a programming language");
            searchIndex.AddDocument(2, "C# and .NET are popular technologies");
            searchIndex.AddDocument(3, "Programming with .NET");

            Console.WriteLine("Документи зі словом C#:");
            Console.WriteLine(string.Join(", ", searchIndex.Search("C#")));

            Console.WriteLine("\nДокументи зі словами C# та .NET:");
            Console.WriteLine(string.Join(", ",
                searchIndex.SearchMultiple("C#", ".NET")));

            Console.WriteLine("\nРоботу програми завершено.");
        }
    }
}
