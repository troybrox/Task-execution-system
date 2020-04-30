using System.ComponentModel.DataAnnotations.Schema;
using TaskExecutionSystem.DAL.Entities.Task;

namespace TaskExecutionSystem.DAL.Entities.File
{
    public class TaskFile : FileModelBase
    {
        public int TaskModelId { get; set; }

        [ForeignKey("TaskModelId")]
        public TaskModel TaskModel { get; set; }
    }
}
