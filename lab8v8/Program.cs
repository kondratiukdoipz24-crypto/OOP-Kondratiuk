
using System;
using System.Collections.Generic;

public class Product
{
    public string Name { get; set; }
    public string Category { get; set; }
    public decimal Price { get; set; }

    public Product(string name, string category, decimal price)
    {
        Name = name;
        Category = category;
        Price = price;
    }
}

class Program
{
    static void Main()
    {
        List<int> numbers = new List<int>
        {
            5, 10, -3, 8, 12
        };

        bool hasNegative = CollectionHelper<int>.Any(
            numbers,
            number => number < 0
        );

        Console.WriteLine("Перевірка чисел:");
        Console.WriteLine($"Чи є від'ємні числа: {hasNegative}");

        List<Product> products = new List<Product>
        {
            new Product("Хліб", "Продукти", 30),
            new Product("Молоко", "Напої", 45),
            new Product("Яблука", "Фрукти", 50),
            new Product("Сік", "Напої", 65)
        };

        bool hasDrinks = CollectionHelper<Product>.Any(
            products,
            product => product.Category == "Напої"
        );

        Console.WriteLine();
        Console.WriteLine("Перевірка товарів:");
        Console.WriteLine($"Чи є товари категорії 'Напої': {hasDrinks}");

        bool hasExpensiveProduct = CollectionHelper<Product>.Any(
            products,
            product => product.Price > 100
        );

        Console.WriteLine(
            $"Чи є товари дорожчі за 100 грн: {hasExpensiveProduct}"
        );
    }
}
