using System;
using System.Collections.Generic;
using System.Text;
using TaskExecutionSystem.DAL.Entities.Identity;
using TaskExecutionSystem.DAL.Entities.File;

namespace TaskExecutionSystem.DAL.Entities.Task
{
    public class Solution
    {
        public int Id { get; set; }

        public string ContentText { get; set; }

        public DateTime CreationDate { get; set; }

        public SolutionFile File { get; set; }

        // проверка на вовремя

        public int TaskId { get; set; }

        public int StudentId { get; set; }

    }
}
