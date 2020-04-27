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

        public bool InTime { get; set; }


        public int TaskId { get; set; }

        public int StudentId { get; set; }


        public TaskModel TaskModel { get; set; }

        public Student Student { get; set; }
    }
}
