using System;

public class TemporaryFile : IDisposable
{
    private string _tempFilePath;
    private bool _fileExists;
    private bool _disposed;

    public string TempFilePath
    {
        get { return _tempFilePath; }
    }

    public bool FileExists
    {
        get { return _fileExists; }
    }

    public TemporaryFile(string tempFilePath)
    {
        _tempFilePath = tempFilePath;
        _fileExists = true;
        _disposed = false;

        Console.WriteLine("Temporary file created: " + _tempFilePath);
    }

    public void Write(string content)
    {
        if (_disposed)
        {
            Console.WriteLine("Cannot write: resource is disposed.");
            return;
        }

        if (_fileExists)
        {
            Console.WriteLine("Writing to temporary file: " + content);
        }
        else
        {
            Console.WriteLine("File does not exist.");
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                Console.WriteLine("Disposing managed resources");
            }

            if (_fileExists)
            {
                Console.WriteLine("Deleting temporary file: " + _tempFilePath);
                _fileExists = false;
            }

            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~TemporaryFile()
    {
        Dispose(false);
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Scenario 1: using ===");

        using (TemporaryFile file = new TemporaryFile("temp1.txt"))
        {
            file.Write("Hello from using");
        }

        Console.WriteLine();

        Console.WriteLine("=== Scenario 2: explicit Dispose() ===");

        TemporaryFile file2 = new TemporaryFile("temp2.txt");
        file2.Write("Hello from explicit Dispose");
        file2.Dispose();

        Console.WriteLine();

        Console.WriteLine("=== Scenario 3: without Dispose(), using GC ===");

        CreateTemporaryFile();

        GC.Collect();
        GC.WaitForPendingFinalizers();

        Console.WriteLine("Garbage collection completed.");
    }

    static void CreateTemporaryFile()
    {
        TemporaryFile file3 = new TemporaryFile("temp3.txt");
        file3.Write("Hello from GC");
    }
}