using TaskExecutionSystem.DAL.Entities.Task;

namespace TaskExecutionSystem.DAL.Entities.File
{
    public class TaskFile : FileModelBase
    {
        public int TaskItemId { get; set; }

        public TaskModel TaskItem { get; set; }
    }
}
