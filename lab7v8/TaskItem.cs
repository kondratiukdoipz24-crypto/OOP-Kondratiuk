namespace lab12v8
{
    public class TaskItem : IEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }

        public TaskItem()
        {
            Title = "";
            Description = "";
        }
    }
}