using System;
using System.Collections.Generic;
using System.Linq;

namespace lab12v8
{
    public class InMemoryRepository<T> : IRepository<T>
        where T : IEntity
    {
        private readonly List<T> _items = new List<T>();

        public T GetById(int id)
        {
            T item = _items.FirstOrDefault(x => x.Id == id);

            if (item is null)
                throw new KeyNotFoundException("Елемент не знайдено.");

            return item;
        }

        public IEnumerable<T> GetAll()
        {
            return _items.ToList();
        }

        public void Add(T item)
        {
            if (_items.Any(x => x.Id == item.Id))
                throw new InvalidOperationException("Такий ID вже існує.");

            _items.Add(item);
        }

        public void Update(T item)
        {
            int index = _items.FindIndex(x => x.Id == item.Id);

            if (index == -1)
                throw new KeyNotFoundException("Елемент не знайдено.");

            _items[index] = item;
        }

        public void Delete(int id)
        {
            T item = GetById(id);
            _items.Remove(item);
        }
    }
}