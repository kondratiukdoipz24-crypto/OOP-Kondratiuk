
using System;
using IndependentWork7;

Cache<string, string> cache = new Cache<string, string>(3);

cache.Add("1", "Яблуко");
cache.Add("2", "Банан");
cache.Add("3", "Апельсин");

Console.WriteLine("Елементи кешу:");
Console.WriteLine(cache.Get("1"));
Console.WriteLine(cache.Get("2"));
Console.WriteLine(cache.Get("3"));

Console.WriteLine("\nДодаємо новий елемент:");

cache.Add("4", "Груша");

Console.WriteLine("Кількість елементів: " + cache.Count);

Console.WriteLine("Чи є ключ 1: " + cache.ContainsKey("1"));
Console.WriteLine("Чи є ключ 4: " + cache.ContainsKey("4"));

Console.WriteLine("\nПоточні елементи:");

Console.WriteLine(cache.Get("2"));
Console.WriteLine(cache.Get("3"));
Console.WriteLine(cache.Get("4"));
