using System;

class Animal
{
    private string species;
    private string nickname;

    public int Age { get; set; }

    public Animal(string species, string nickname, int age)
    {
        this.species = species;
        this.nickname = nickname;
        Age = age;
    }

    public void Speak()
    {
        Console.WriteLine($"{nickname} ({species}), вік: {Age} років");
        Console.WriteLine("Тварина видає звук.");
    }
}

class Program
{
    static void Main()
    {
        Animal animal1 = new Animal("Собака", "Бобік", 3);
        Animal animal2 = new Animal("Кіт", "Мурчик", 5);
        Animal animal3 = new Animal("Папуга", "Кеша", 2);

        animal1.Speak();
        animal2.Speak();
        animal3.Speak();
    }
}
