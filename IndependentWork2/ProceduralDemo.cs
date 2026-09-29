using System;

public static class ProceduralDemo
{
    public static void Run()
    {
        Console.WriteLine("===== ПРОЦЕДУРНИЙ ПІДХІД =====");

        string[] names = { "Ноутбук", "Мишка", "Навушники", "Клавіатура" };
        double[] prices = { 25000, 800, 1200, 450 };
        int[] quantities = { 1, 2, 1, 2 };

        double total = CalculateTotal(names, prices, quantities);

        Console.WriteLine($"Підсумок кошика: {total:F2} грн");
    }

    static double CalculateItemTotal(double price, int quantity)
    {
        return price * quantity;
    }

    static double ApplyDiscount(double price, int quantity)
    {
        double sum = CalculateItemTotal(price, quantity);

        if (price > 500)
        {
            double discount = sum * 0.10;
            Console.WriteLine($"Знижка: {discount:F2} грн");
            return sum - discount;
        }

        return sum;
    }

    static double CalculateTotal(string[] names, double[] prices, int[] quantities)
    {
        double total = 0;

        for (int i = 0; i < names.Length; i++)
        {
            double itemTotal = CalculateItemTotal(prices[i], quantities[i]);

            Console.WriteLine(
                $"{names[i]}: {prices[i]:F2} грн x {quantities[i]} = {itemTotal:F2} грн");

            double finalPrice = ApplyDiscount(prices[i], quantities[i]);

            total += finalPrice;
        }

        return total;
    }
}