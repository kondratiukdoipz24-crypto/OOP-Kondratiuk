
using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        List<int> numbers = new List<int> { 5, 12, 3, 20, 8 };

        Console.WriteLine("=== КОЛЕКЦІЯ ЧИСЕЛ ===");

        var doubledNumbers = CollectionUtils.Map(numbers, n => n * 2);
        Console.WriteLine("Подвоєні числа: " + string.Join(", ", doubledNumbers));

        var maxNumber = CollectionUtils.MaxBy(numbers, n => n);
        Console.WriteLine("Найбільше число: " + maxNumber);

        var evenNumbers = CollectionUtils.Filter(numbers, n => n % 2 == 0);
        Console.WriteLine("Парні числа: " + string.Join(", ", evenNumbers));

        Console.WriteLine("Усі числа:");
        CollectionUtils.ForEach(numbers, n => Console.WriteLine(n));


        List<string> names = new List<string>
        {
            "Олена", "Дарина", "Андрій", "Марія"
        };

        Console.WriteLine("\n=== КОЛЕКЦІЯ РЯДКІВ ===");

        var upperNames = CollectionUtils.Map(names, name => name.ToUpper());
        Console.WriteLine("Імена великими літерами: " + string.Join(", ", upperNames));

        var longestName = CollectionUtils.MaxBy(names, name => name.Length);
        Console.WriteLine("Найдовше ім'я: " + longestName);

        var shortNames = CollectionUtils.Filter(names, name => name.Length <= 5);
        Console.WriteLine("Імена до 5 символів: " + string.Join(", ", shortNames));

        Console.WriteLine("Усі імена:");
        CollectionUtils.ForEach(names, name => Console.WriteLine(name));


        List<Product> products = new List<Product>
        {
            new Product("Ноутбук", 25000),
            new Product("Мишка", 500),
            new Product("Клавіатура", 1500),
            new Product("Монітор", 8000)
        };

        Console.WriteLine("\n=== КОЛЕКЦІЯ ПРОДУКТІВ ===");

        var productNames = CollectionUtils.Map(products, product => product.Name);
        Console.WriteLine("Назви продуктів: " + string.Join(", ", productNames));

        var mostExpensive = CollectionUtils.MaxBy(products, product => product.Price);
        Console.WriteLine("Найдорожчий продукт: " +
            mostExpensive.Name + " - " + mostExpensive.Price + " грн");

        var expensiveProducts = CollectionUtils.Filter(
            products, product => product.Price > 1000);

        Console.WriteLine("Продукти дорожчі за 1000 грн:");
        CollectionUtils.ForEach(
            expensiveProducts,
            product => Console.WriteLine(product.Name + " - " + product.Price + " грн"));

        Console.WriteLine("Усі продукти:");
        CollectionUtils.ForEach(
            products,
            product => Console.WriteLine(product.Name + ": " + product.Price + " грн"));
    }
}
