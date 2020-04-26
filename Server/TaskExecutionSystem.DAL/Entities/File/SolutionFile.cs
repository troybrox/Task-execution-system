using TaskExecutionSystem.DAL.Entities.Task;

namespace TaskExecutionSystem.DAL.Entities.File
{
    public class SolutionFile : FileModelBase
    {
        public int SolutionId { get; set; }

        public Solution Solution { get; set; }
    }
}
