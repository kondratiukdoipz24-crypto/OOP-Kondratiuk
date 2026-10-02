using System;

class Device
{
    private string brand;
    private string model;

    public string Brand
    {
        get { return brand; }
        set { brand = value; }
    }

    public string Model
    {
        get { return model; }
        set { model = value; }
    }

    public Device(string brand, string model)
    {
        this.brand = brand;
        this.model = model;
    }

    public virtual void PowerOn()
    {
        Console.WriteLine("Пристрій увімкнено.");
    }

    public string GetDeviceType()
    {
        return "Звичайний пристрій";
    }
}

class Smartphone : Device
{
    public double ScreenSize { get; set; }

    public Smartphone(string brand, string model, double screenSize)
        : base(brand, model)
    {
        ScreenSize = screenSize;
    }

    public override void PowerOn()
    {
        Console.WriteLine($"Смартфон {Brand} {Model} увімкнено.");
    }

    public void MakeCall()
    {
        Console.WriteLine($"Смартфон {Brand} здійснює дзвінок.");
    }

    public new string GetDeviceType()
    {
        return "Смартфон";
    }
}

class Laptop : Device
{
    public string Processor { get; set; }

    public Laptop(string brand, string model, string processor)
        : base(brand, model)
    {
        Processor = processor;
    }

    public override void PowerOn()
    {
        Console.WriteLine($"Ноутбук {Brand} {Model} увімкнено.");
    }

    public void RunProgram()
    {
        Console.WriteLine($"Ноутбук {Brand} запускає програму.");
    }
}

class Program
{
    static void Main()
    {
        Device device = new Device("Generic", "Device 1");

        Smartphone smartphone = new Smartphone("Samsung", "Galaxy S24", 6.2);

        Laptop laptop = new Laptop("Lenovo", "IdeaPad 5", "Intel Core i5");

        Console.WriteLine("=== ЗВИЧАЙНИЙ ПРИСТРІЙ ===");
        device.PowerOn();
        Console.WriteLine(device.GetDeviceType());

        Console.WriteLine();

        Console.WriteLine("=== SMARTPHONE ===");
        smartphone.PowerOn();
        smartphone.MakeCall();
        Console.WriteLine(smartphone.GetDeviceType());

        Console.WriteLine();

        Console.WriteLine("=== LAPTOP ===");
        laptop.PowerOn();
        laptop.RunProgram();

        Console.WriteLine();

        Console.WriteLine("=== ПОЛІМОРФІЗМ ===");

        Device smartphoneDevice = smartphone;
        Device laptopDevice = laptop;

        smartphoneDevice.PowerOn();
        laptopDevice.PowerOn();

        Console.WriteLine();

        Console.WriteLine("=== РІЗНИЦЯ МІЖ override ТА new ===");

        Console.WriteLine("Через посилання Smartphone:");
        Console.WriteLine(smartphone.GetDeviceType());

        Console.WriteLine("Через посилання Device:");
        Console.WriteLine(smartphoneDevice.GetDeviceType());
    }
}
