using System.ComponentModel.DataAnnotations.Schema;
using TaskExecutionSystem.DAL.Entities.Task;

namespace TaskExecutionSystem.DAL.Entities.File
{
    public class SolutionFile : FileModelBase
    {
        public int SolutionId { get; set; }

        [ForeignKey("SolutionId")]
        public Solution Solution { get; set; }
    }
}
