using System;
using System.Collections.Generic;
using System.Text;

namespace TaskExecutionSystem.BLL.DTO.Task
{
    public class SolutionCreateModel
    {
        public string ContentText { get; set; }

        public int TaskId { get; set; }

        public int StudentId { get; set; }
    }
}
