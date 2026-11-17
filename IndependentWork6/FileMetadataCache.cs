
using System;
using System.Collections.Generic;

namespace IndependentWork6
{
    public class FileMetadata
    {
        public string FileName { get; set; }
        public long Size { get; set; }
        public DateTime LastModified { get; set; }
        public string Hash { get; set; }

        public FileMetadata(string fileName, long size, DateTime lastModified, string hash)
        {
            FileName = fileName;
            Size = size;
            LastModified = lastModified;
            Hash = hash;
        }

        public override string ToString()
        {
            return $"Назва: {FileName}, Розмір: {Size} байт, " +
                   $"Дата зміни: {LastModified}, Хеш: {Hash}";
        }
    }

    public class FileMetadataCache
    {
        private readonly Dictionary<string, FileMetadata> _cache;

        public FileMetadataCache()
        {
            _cache = new Dictionary<string, FileMetadata>(
                StringComparer.OrdinalIgnoreCase);
        }

        public void Add(string key, FileMetadata metadata)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Ключ не може бути порожнім.");

            _cache[key] = metadata;
        }

        public FileMetadata Get(string key)
        {
            if (_cache.TryGetValue(key, out FileMetadata metadata))
                return metadata;

            throw new KeyNotFoundException("Файл не знайдено.");
        }

        public bool Remove(string key)
        {
            return _cache.Remove(key);
        }

        public bool Contains(string key)
        {
            return _cache.ContainsKey(key);
        }

        public int Count
        {
            get { return _cache.Count; }
        }
    }
}
