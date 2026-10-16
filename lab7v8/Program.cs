using System;
using lab12v8;

InMemoryRepository<TaskItem> repository =
    new InMemoryRepository<TaskItem>();

repository.Add(new TaskItem
{
    Id = 1,
    Title = "Вивчити Generics",
    Description = "Виконати лабораторну",
    IsCompleted = false
});

repository.Add(new TaskItem
{
    Id = 2,
    Title = "Написати тести",
    Description = "Перевірити CRUD",
    IsCompleted = true
});

foreach (TaskItem task in repository.GetAll())
{
    Console.WriteLine(
        $"{task.Id}. {task.Title} — {task.IsCompleted}");
}

TaskItem updatedTask = repository.GetById(1);
updatedTask.IsCompleted = true;
repository.Update(updatedTask);

repository.Delete(2);

Console.WriteLine("Після оновлення та видалення:");

foreach (TaskItem task in repository.GetAll())
{
    Console.WriteLine(
        $"{task.Id}. {task.Title} — {task.IsCompleted}");
}