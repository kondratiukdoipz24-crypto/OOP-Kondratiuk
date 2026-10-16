using System;
using System.Collections.Generic;
using System.Linq;
using lab12v8;
using Xunit;

namespace lab12v8.Tests
{
    public class RepositoryTests
    {
        private readonly InMemoryRepository<TaskItem> _repository;

        public RepositoryTests()
        {
            _repository = new InMemoryRepository<TaskItem>();
        }

        private TaskItem CreateTask(int id, string title)
        {
            return new TaskItem
            {
                Id = id,
                Title = title,
                Description = "Опис завдання",
                IsCompleted = false
            };
        }

        [Fact]
        public void Add_AddsNewItem()
        {
            _repository.Add(CreateTask(1, "Завдання"));

            Assert.Single(_repository.GetAll());
            Assert.Equal("Завдання", _repository.GetById(1).Title);
        }

        [Fact]
        public void GetById_ReturnsCorrectItem()
        {
            _repository.Add(CreateTask(1, "Перше"));
            _repository.Add(CreateTask(2, "Друге"));

            TaskItem result = _repository.GetById(2);

            Assert.Equal("Друге", result.Title);
        }

        [Fact]
        public void GetById_ThrowsWhenItemDoesNotExist()
        {
            Assert.Throws<KeyNotFoundException>(
                () => _repository.GetById(99));
        }

        [Fact]
        public void GetAll_ReturnsAllItems()
        {
            _repository.Add(CreateTask(1, "Перше"));
            _repository.Add(CreateTask(2, "Друге"));

            Assert.Equal(2, _repository.GetAll().Count());
        }

        [Fact]
        public void Update_ChangesExistingItem()
        {
            _repository.Add(CreateTask(1, "Старе"));

            _repository.Update(CreateTask(1, "Нове"));

            Assert.Equal("Нове", _repository.GetById(1).Title);
        }

        [Fact]
        public void Update_ThrowsWhenItemDoesNotExist()
        {
            Assert.Throws<KeyNotFoundException>(
                () => _repository.Update(CreateTask(99, "Нове")));
        }

        [Fact]
        public void Delete_RemovesExistingItem()
        {
            _repository.Add(CreateTask(1, "Завдання"));

            _repository.Delete(1);

            Assert.Empty(_repository.GetAll());
        }

        [Fact]
        public void Delete_ThrowsWhenItemDoesNotExist()
        {
            Assert.Throws<KeyNotFoundException>(
                () => _repository.Delete(99));
        }
    }
}