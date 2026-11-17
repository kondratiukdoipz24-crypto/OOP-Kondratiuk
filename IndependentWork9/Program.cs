
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

class Program
{
    static void Main()
    {
        DataGenerator.GenerateData(
            out List<Product> products,
            out List<Customer> customers,
            out List<Sale> sales);

        Console.WriteLine("=== LINQ ETL АНАЛІТИКА ===");
        Console.WriteLine();

        Console.WriteLine("Кількість продуктів: " + products.Count);
        Console.WriteLine("Кількість клієнтів: " + customers.Count);
        Console.WriteLine("Кількість продажів: " + sales.Count);

        Console.WriteLine();

        // Пайплайн 1
        Console.WriteLine("=== 1. НАЙПОПУЛЯРНІШІ ПРОДУКТИ ЗА КАТЕГОРІЯМИ ===");

        List<CategorySalesResult> pipeline1 = Measure(
            "Пайплайн 1",
            () => TopProductsByCategory(sales, products));

        foreach (CategorySalesResult category in pipeline1)
        {
            Console.WriteLine("\nКатегорія: " + category.Category);

            foreach (ProductSalesResult product in category.Products)
            {
                Console.WriteLine(
                    product.Name + " - продано: " +
                    product.Quantity + " шт.");
            }
        }

        Console.WriteLine();

        // Пайплайн 2
        Console.WriteLine("=== 2. ЗАГАЛЬНИЙ ДОХІД ЗА МІСТАМИ ===");

        List<CityRevenueResult> pipeline2 = Measure(
            "Пайплайн 2",
            () => RevenueByCity(sales, products, customers));

        foreach (CityRevenueResult city in pipeline2)
        {
            Console.WriteLine(
                city.City + " - " +
                city.Revenue.ToString("N2") + " грн");
        }

        Console.WriteLine();

        // Пайплайн 3
        Console.WriteLine("=== 3. ПРОДАЖІ ЗА МІСЯЦЯМИ ТА РОКАМИ ===");

        List<MonthlySalesResult> pipeline3 = Measure(
            "Пайплайн 3",
            () => SalesByMonth(sales, products));

        foreach (MonthlySalesResult month in pipeline3)
        {
            Console.WriteLine(
                month.Year + "/" +
                month.Month.ToString("D2") +
                " - кількість: " + month.Quantity +
                ", дохід: " + month.Revenue.ToString("N2") +
                " грн");
        }

        Console.WriteLine();

        // Пайплайн 4
        Console.WriteLine("=== 4. ТОП-10 КЛІЄНТІВ ЗА ВИТРАТАМИ ===");

        List<CustomerRevenueResult> pipeline4 = Measure(
            "Пайплайн 4",
            () => TopCustomers(sales, products, customers));

        foreach (CustomerRevenueResult customer in pipeline4)
        {
            Console.WriteLine(
                customer.Name + " (" + customer.City + ") - " +
                customer.Revenue.ToString("N2") + " грн");
        }

        Console.WriteLine();
        Console.WriteLine("=== АНАЛІЗ ЗАВЕРШЕНО ===");
    }

    // Вимірювання часу виконання LINQ-пайплайна
    static T Measure<T>(string name, Func<T> action)
    {
        Stopwatch stopwatch = new Stopwatch();

        stopwatch.Start();

        T result = action();

        stopwatch.Stop();

        Console.WriteLine(
            name + " - час виконання: " +
            stopwatch.Elapsed.TotalMilliseconds.ToString("F4") +
            " мс");

        return result;
    }

    // Пайплайн 1: три найпопулярніші продукти в кожній категорії
    static List<CategorySalesResult> TopProductsByCategory(
        List<Sale> sales,
        List<Product> products)
    {
        var productSales = sales
            .GroupBy(sale => sale.ProductId)
            .Select(group => new
            {
                ProductId = group.Key,
                Quantity = group.Sum(sale => sale.Quantity)
            });

        var joinedProducts = productSales
            .Join(
                products,
                sale => sale.ProductId,
                product => product.Id,
                (sale, product) => new
                {
                    product.Category,
                    product.Name,
                    sale.Quantity
                });

        return joinedProducts
            .GroupBy(item => item.Category)
            .Select(group => new CategorySalesResult
            {
                Category = group.Key,
                Products = group
                    .OrderByDescending(item => item.Quantity)
                    .Take(3)
                    .Select(item => new ProductSalesResult
                    {
                        Name = item.Name,
                        Quantity = item.Quantity
                    })
                    .ToList()
            })
            .OrderBy(item => item.Category)
            .ToList();
    }

    // Пайплайн 2: дохід за містами клієнтів
    static List<CityRevenueResult> RevenueByCity(
        List<Sale> sales,
        List<Product> products,
        List<Customer> customers)
    {
        return sales
            .Join(
                products,
                sale => sale.ProductId,
                product => product.Id,
                (sale, product) => new
                {
                    sale.CustomerId,
                    Revenue = sale.Quantity * product.UnitPrice
                })
            .Join(
                customers,
                sale => sale.CustomerId,
                customer => customer.Id,
                (sale, customer) => new
                {
                    customer.City,
                    sale.Revenue
                })
            .GroupBy(item => item.City)
            .Select(group => new CityRevenueResult
            {
                City = group.Key,
                Revenue = group.Sum(item => item.Revenue)
            })
            .OrderByDescending(item => item.Revenue)
            .ToList();
    }

    // Пайплайн 3: кількість продажів і дохід за місяцями
    static List<MonthlySalesResult> SalesByMonth(
        List<Sale> sales,
        List<Product> products)
    {
        return sales
            .Join(
                products,
                sale => sale.ProductId,
                product => product.Id,
                (sale, product) => new
                {
                    sale.SaleDate,
                    sale.Quantity,
                    Revenue = sale.Quantity * product.UnitPrice
                })
            .GroupBy(item => new
            {
                Year = item.SaleDate.Year,
                Month = item.SaleDate.Month
            })
            .Select(group => new MonthlySalesResult
            {
                Year = group.Key.Year,
                Month = group.Key.Month,
                Quantity = group.Sum(item => item.Quantity),
                Revenue = group.Sum(item => item.Revenue)
            })
            .OrderBy(item => item.Year)
            .ThenBy(item => item.Month)
            .ToList();
    }

    // Пайплайн 4: десять клієнтів із найбільшими витратами
    static List<CustomerRevenueResult> TopCustomers(
        List<Sale> sales,
        List<Product> products,
        List<Customer> customers)
    {
        return sales
            .Join(
                products,
                sale => sale.ProductId,
                product => product.Id,
                (sale, product) => new
                {
                    sale.CustomerId,
                    Revenue = sale.Quantity * product.UnitPrice
                })
            .Join(
                customers,
                sale => sale.CustomerId,
                customer => customer.Id,
                (sale, customer) => new
                {
                    customer.Id,
                    customer.Name,
                    customer.City,
                    sale.Revenue
                })
            .GroupBy(item => new
            {
                item.Id,
                item.Name,
                item.City
            })
            .Select(group => new CustomerRevenueResult
            {
                Name = group.Key.Name,
                City = group.Key.City,
                Revenue = group.Sum(item => item.Revenue)
            })
            .OrderByDescending(item => item.Revenue)
            .Take(10)
            .ToList();
    }
}

// Результат аналізу продуктів
public class ProductSalesResult
{
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
}

// Результат групування продуктів за категоріями
public class CategorySalesResult
{
    public string Category { get; set; } = string.Empty;
    public List<ProductSalesResult> Products { get; set; } =
        new List<ProductSalesResult>();
}

// Результат доходу за містами
public class CityRevenueResult
{
    public string City { get; set; } = string.Empty;
    public decimal Revenue { get; set; }
}

// Результат продажів за місяцями
public class MonthlySalesResult
{
    public int Year { get; set; }
    public int Month { get; set; }
    public int Quantity { get; set; }
    public decimal Revenue { get; set; }
}

// Результат аналізу клієнтів
public class CustomerRevenueResult
{
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public decimal Revenue { get; set; }
}
