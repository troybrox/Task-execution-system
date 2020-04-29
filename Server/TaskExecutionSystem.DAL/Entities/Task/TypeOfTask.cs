using System;
using System.Collections.Generic;
using System.Text;

namespace TaskExecutionSystem.DAL.Entities.Task
{
    public class TypeOfTask
    {
        public int Id { get; set; }

        public string Name { get; set; }


        public List<TaskModel> Tasks { get; set; }
    }
}
