using System;
using System.Collections.Generic;
using System.Text;

using TaskExecutionSystem.DAL.Entities.Identity;
using TaskExecutionSystem.DAL.Entities.Task;

namespace TaskExecutionSystem.DAL.Entities.Relations
{
    public class TaskStudentItem
    {
        public int Id { get; set; }

        public int TaskId { get; set; }

        public int StudentId { get; set; }


        public TaskModel Task { get; set; }

        public Student Student { get; set; }
    }
}
