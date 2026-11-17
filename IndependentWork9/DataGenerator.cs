
using System;
using System.Collections.Generic;

public static class DataGenerator
{
    public static void GenerateData(
        out List<Product> products,
        out List<Customer> customers,
        out List<Sale> sales)
    {
        Random random = new Random(42);

        string[] categories =
        {
            "Електроніка",
            "Одяг",
            "Продукти",
            "Побутова техніка",
            "Канцелярія"
        };

        string[] productNames =
        {
            "Ноутбук", "Телефон", "Навушники",
            "Футболка", "Джинси", "Кава",
            "Чай", "Пилосос", "Праска",
            "Зошит", "Ручка", "Монітор",
            "Клавіатура", "Мишка", "Планшет"
        };

        string[] cities =
        {
            "Рівне", "Львів", "Київ", "Одеса",
            "Харків", "Дніпро", "Тернопіль",
            "Луцьк", "Житомир", "Полтава"
        };

        products = new List<Product>();

        for (int i = 1; i <= 100; i++)
        {
            products.Add(new Product
            {
                Id = i,
                Name = productNames[random.Next(productNames.Length)]
                    + " " + i,
                Category = categories[random.Next(categories.Length)],
                UnitPrice = random.Next(50, 30001)
            });
        }

        customers = new List<Customer>();

        for (int i = 1; i <= 100; i++)
        {
            customers.Add(new Customer
            {
                Id = i,
                Name = "Клієнт " + i,
                City = cities[random.Next(cities.Length)]
            });
        }

        sales = new List<Sale>();

        DateTime startDate = new DateTime(2025, 1, 1);

        for (int i = 1; i <= 5000; i++)
        {
            sales.Add(new Sale
            {
                Id = i,
                ProductId = random.Next(1, 101),
                Quantity = random.Next(1, 11),
                SaleDate = startDate.AddDays(random.Next(0, 365)),
                CustomerId = random.Next(1, 101)
            });
        }
    }
}
