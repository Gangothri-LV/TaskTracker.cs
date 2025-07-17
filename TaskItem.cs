namespace TaskTracker
{
    public class TaskItem
    {
        public string Username { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }

        public override string ToString()
        {
            return $"Title: {Title}, Description: {Description}, Completed: {IsCompleted}";
        }
    }
}