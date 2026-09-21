using System;

class Animal
{
    private string _species;
    private string _nickname;
    private int _age;

    public string Species
    {
        get { return _species; }
        set { _species = value; }
    }

    public string Nickname
    {
        get { return _nickname; }
        set { _nickname = value; }
    }

    public int Age
    {
        get { return _age; }
        set
        {
            if (value < 0)
                throw new ArgumentException("Вік не може бути від'ємним.");

            _age = value;
        }
    }

    public Animal() : this("Unknown", "Unnamed", 0)
    {
    }

    public Animal(string species, string nickname, int age)
    {
        Species = species;
        Nickname = nickname;
        Age = age;

        Console.WriteLine($"Створено тварину: {Nickname}");
    }

    public void Speak()
    {
        if (Species.ToLower() == "собака")
            Console.WriteLine($"{Nickname}: Гав-гав!");
        else if (Species.ToLower() == "кіт")
            Console.WriteLine($"{Nickname}: Няв-няв!");
        else if (Species.ToLower() == "корова")
            Console.WriteLine($"{Nickname}: Му-у-у!");
        else if (Species.ToLower() == "папуга")
            Console.WriteLine($"{Nickname}: Привіт!");
        else
            Console.WriteLine($"{Nickname}: Тварина видає звук.");
    }

    ~Animal()
    {
        Console.WriteLine($"Знищується об'єкт тварини: {Nickname}");
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Створення об'єктів ===");

        Animal animal1 = new Animal();

        Animal animal2 = new Animal("Собака", "Бобік", 3);

        Animal animal3 = new Animal("Кіт", "Мурчик", 5);

        Console.WriteLine();

        Console.WriteLine("=== Інформація про тварин ===");

        Console.WriteLine(
            $"{animal1.Nickname} ({animal1.Species}), вік: {animal1.Age}"
        );

        Console.WriteLine(
            $"{animal2.Nickname} ({animal2.Species}), вік: {animal2.Age}"
        );

        Console.WriteLine(
            $"{animal3.Nickname} ({animal3.Species}), вік: {animal3.Age}"
        );

        Console.WriteLine();

        Console.WriteLine("=== Звуки тварин ===");

        animal1.Speak();
        animal2.Speak();
        animal3.Speak();

        Console.WriteLine();

        animal1 = null;
        animal2 = null;
        animal3 = null;

        Console.WriteLine("=== Запуск Garbage Collector ===");

        GC.Collect();
        GC.WaitForPendingFinalizers();

        Console.WriteLine("=== Кінець програми ===");
    }
}