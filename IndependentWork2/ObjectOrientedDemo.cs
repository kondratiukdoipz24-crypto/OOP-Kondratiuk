using System;
using System.Collections.Generic;

public class Product
{
    public string Name { get; set; }
    public double Price { get; set; }

    public Product(string name, double price)
    {
        Name = name;
        Price = price;
    }
}

public class Cart
{
    private List<(Product Product, int Quantity)> items = new List<(Product Product, int Quantity)>();

    public void AddItem(Product product, int quantity)
    {
        items.Add((product, quantity));
    }

    public double ApplyDiscount(Product product, int quantity)
    {
        double sum = product.Price * quantity;

        if (product.Price > 500)
        {
            double discount = sum * 0.10;
            Console.WriteLine($"Знижка: {discount:F2} грн");
            return sum - discount;
        }

        return sum;
    }

    public double GetTotal()
    {
        double total = 0;

        foreach (var item in items)
        {
            double itemTotal = item.Product.Price * item.Quantity;

            Console.WriteLine(
                $"{item.Product.Name}: {item.Product.Price:F2} грн x {item.Quantity} = {itemTotal:F2} грн");

            total += ApplyDiscount(item.Product, item.Quantity);
        }

        return total;
    }
}

public static class ObjectOrientedDemo
{
    public static void Run()
    {
        Console.WriteLine("===== ОБ'ЄКТНО-ОРІЄНТОВАНИЙ ПІДХІД =====");

        Cart cart = new Cart();

        Product laptop = new Product("Ноутбук", 25000);
        Product mouse = new Product("Мишка", 800);
        Product headphones = new Product("Навушники", 1200);
        Product keyboard = new Product("Клавіатура", 450);

        cart.AddItem(laptop, 1);
        cart.AddItem(mouse, 2);
        cart.AddItem(headphones, 1);
        cart.AddItem(keyboard, 2);

        double total = cart.GetTotal();

        Console.WriteLine($"Підсумок кошика: {total:F2} грн");
    }
}