using System;
using System.Collections.Generic;
using System.Text;

namespace TaskExecutionSystem.BLL.DTO.Task
{
    public class SolutionDTO
    {
        public int Id { get; set; }

        public string ContentText { get; set; }

        public DateTime CreationDate { get; set; }

        public bool IsInTime { get; set; } 
    }
}
