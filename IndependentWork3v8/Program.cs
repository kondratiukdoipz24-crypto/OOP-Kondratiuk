using System;

namespace IndependentWork3v8
{
    // Варіант 8: Клас TemporaryFile
    public class TemporaryFile : IDisposable
    {
        private bool _disposed = false;
        private string _tempFilePath;
        private bool _fileExists;

        public string TempFilePath => _tempFilePath;
        public bool FileExists => _fileExists;

        // Конструктор: "виділяє ресурс"
        public TemporaryFile(string fileName)
        {
            _tempFilePath = fileName;
            _fileExists = true;
            Console.WriteLine($"[Створено] Тимчасовий файл '{_tempFilePath}' виділено.");
        }

        // Метод класу
        public void Write(string content)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(TemporaryFile), "Неможливо записати: об'єкт уже знищено.");
            }

            if (_fileExists)
            {
                Console.WriteLine($"[Запис] У файл '{_tempFilePath}' записано: \"{content}\"");
            }
        }

        // Захищений віртуальний метод для реалізації патерну Dispose
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Звільнення керованих ресурсів (якщо є)
                    Console.WriteLine("[Dispose] Звільнення керованих ресурсів...");
                }

                // Звільнення некерованих ресурсів / очищення файлу
                if (_fileExists)
                {
                    Console.WriteLine($"[Dispose] Видалення тимчасового файлу '{_tempFilePath}'...");
                    _fileExists = false;
                }

                _disposed = true;
            }
        }

        // Публічний метод Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); // Скасовує виклик деструктора
        }

        // Деструктор (фіналізатор)
        ~TemporaryFile()
        {
            Console.WriteLine("[Finalizer] Виклик деструктора (Dispose(false))...");
            Dispose(false);
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // ==========================================
            // Сценарій 1: Використання блоку using
            // ==========================================
            Console.WriteLine("=== Сценарій 1: Автоматичне звільнення через using ===");
            using (var tempFile1 = new TemporaryFile("temp1.txt"))
            {
                tempFile1.Write("Данi для першого файлу");
            } // Dispose() викликається автоматично
            Console.WriteLine();

            // ==========================================
            // Сценарій 2: Явний виклик Dispose() та перевірка повторного виклику
            // ==========================================
            Console.WriteLine("=== Сценарій 2: Явний виклик Dispose() ===");
            var tempFile2 = new TemporaryFile("temp2.txt");
            tempFile2.Write("Данi для другого файлу");
            
            // Явний виклик
            tempFile2.Dispose();

            // Повторний виклик (безпечний завдяки _disposed)
            Console.WriteLine("Спроба повторного виклику Dispose():");
            tempFile2.Dispose(); 
            Console.WriteLine();

            // ==========================================
            // Сценарій 3: Об'єкт без Dispose() — робота деструктора (GC)
            // ==========================================
            Console.WriteLine("=== Сценарій 3: Робота деструктора через GC ===");
            CreateUnmanagedObject();

            // Примусовий виклик збирача сміття
            Console.WriteLine("Запуск GC.Collect() та GC.WaitForPendingFinalizers()...");
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Console.WriteLine("\nПрограму завершено успішно.");
        }

        static void CreateUnmanagedObject()
        {
            var tempFile3 = new TemporaryFile("temp3.txt");
            tempFile3.Write("Данi для третього файлу (забули закрити)");
            // Dispose() свідомо не викликається
        }
    }
}