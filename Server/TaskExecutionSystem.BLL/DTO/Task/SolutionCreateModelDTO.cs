using System;
using System.Collections.Generic;
using System.Text;
using TaskExecutionSystem.DAL.Entities.Task;

namespace TaskExecutionSystem.BLL.DTO.Task
{
    public class SolutionCreateModelDTO
    {
        public int Id { get; set; }

        public string ContentText { get; set; }

        public int TaskId { get; set; }
    }
}
