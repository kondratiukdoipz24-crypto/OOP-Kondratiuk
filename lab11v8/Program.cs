
using System;
using System.Collections.Generic;
using System.Linq;

class Dish
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}

class DishCategory
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Dish> Dishes { get; set; } = new List<Dish>();
}

class RestaurantMenu
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<DishCategory> Categories { get; set; } = new List<DishCategory>();
}

static class DishExtensions
{
    public static decimal AverageDishPrice(this IEnumerable<DishCategory> categories)
    {
        return categories
            .SelectMany(category => category.Dishes)
            .Select(dish => dish.Price)
            .DefaultIfEmpty(0m)
            .Average();
    }
}

class Program
{
    static void Main()
    {
        // Створення меню ресторану та тестових даних

        List<RestaurantMenu> menus = new List<RestaurantMenu>
        {
            new RestaurantMenu
            {
                Id = 1,
                Name = "Основне меню",
                Categories = new List<DishCategory>
                {
                    new DishCategory
                    {
                        Id = 1,
                        Name = "Салати",
                        Dishes = new List<Dish>
                        {
                            new Dish { Id = 1, Name = "Цезар", Price = 180 },
                            new Dish { Id = 2, Name = "Грецький салат", Price = 150 }
                        }
                    },
                    new DishCategory
                    {
                        Id = 2,
                        Name = "Перші страви",
                        Dishes = new List<Dish>
                        {
                            new Dish { Id = 3, Name = "Борщ", Price = 120 },
                            new Dish { Id = 4, Name = "Суп курячий", Price = 100 }
                        }
                    },
                    new DishCategory
                    {
                        Id = 3,
                        Name = "Основні страви",
                        Dishes = new List<Dish>
                        {
                            new Dish { Id = 5, Name = "Карбонара", Price = 200 },
                            new Dish { Id = 6, Name = "Стейк", Price = 350 }
                        }
                    }
                }
            },

            new RestaurantMenu
            {
                Id = 2,
                Name = "Додаткове меню",
                Categories = new List<DishCategory>
                {
                    new DishCategory
                    {
                        Id = 4,
                        Name = "Напої",
                        Dishes = new List<Dish>
                        {
                            new Dish { Id = 7, Name = "Кола", Price = 60 },
                            new Dish { Id = 8, Name = "Сік", Price = 70 }
                        }
                    },
                    new DishCategory
                    {
                        Id = 5,
                        Name = "Десерти",
                        Dishes = new List<Dish>
                        {
                            new Dish { Id = 9, Name = "Чізкейк", Price = 140 },
                            new Dish { Id = 10, Name = "Тірамісу", Price = 160 }
                        }
                    },
                    new DishCategory
                    {
                        Id = 6,
                        Name = "Гарніри",
                        Dishes = new List<Dish>
                        {
                            new Dish { Id = 11, Name = "Картопля фрі", Price = 90 },
                            new Dish { Id = 12, Name = "Рис", Price = 80 }
                        }
                    }
                }
            },

            new RestaurantMenu
            {
                Id = 3,
                Name = "Сезонне меню",
                Categories = new List<DishCategory>
                {
                    new DishCategory
                    {
                        Id = 7,
                        Name = "Піца",
                        Dishes = new List<Dish>
                        {
                            new Dish { Id = 13, Name = "Маргарита", Price = 190 },
                            new Dish { Id = 14, Name = "Пепероні", Price = 230 }
                        }
                    },
                    new DishCategory
                    {
                        Id = 8,
                        Name = "Закуски",
                        Dishes = new List<Dish>
                        {
                            new Dish { Id = 15, Name = "Брускети", Price = 110 },
                            new Dish { Id = 16, Name = "Сирна тарілка", Price = 210 }
                        }
                    },
                    new DishCategory
                    {
                        Id = 9,
                        Name = "Морозиво",
                        Dishes = new List<Dish>
                        {
                            new Dish { Id = 17, Name = "Пломбір", Price = 65 },
                            new Dish { Id = 18, Name = "Шоколадне морозиво", Price = 75 }
                        }
                    }
                }
            }
        };

        // 1. SelectMany: отримання всіх страв з усіх меню

        Console.WriteLine("=== ВСІ СТРАВИ РЕСТОРАНУ ===");

        var allDishes = menus
            .SelectMany(menu => menu.Categories)
            .SelectMany(category => category.Dishes)
            .ToList();

        foreach (var dish in allDishes)
        {
            Console.WriteLine($"{dish.Name} - {dish.Price} грн");
        }

        // 2. Пагінація: виведення категорій по 2 на сторінку

        Console.WriteLine("\n=== ПАГІНАЦІЯ КАТЕГОРІЙ ===");

        var allCategories = menus
            .SelectMany(menu => menu.Categories)
            .ToList();

        int pageSize = 2;

        int totalPages = (int)Math.Ceiling(
            (double)allCategories.Count / pageSize);

        for (int page = 1; page <= totalPages; page++)
        {
            Console.WriteLine($"\nСторінка {page}:");

            var pageCategories = allCategories
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            foreach (var category in pageCategories)
            {
                Console.WriteLine(category.Name);
            }
        }

        // 3. Custom Extension Method: середня ціна страв

        Console.WriteLine("\n=== СЕРЕДНЯ ЦІНА СТРАВ ===");

        decimal averagePrice = allCategories.AverageDishPrice();

        Console.WriteLine($"Середня ціна всіх страв: {averagePrice:F2} грн");
    }
}
