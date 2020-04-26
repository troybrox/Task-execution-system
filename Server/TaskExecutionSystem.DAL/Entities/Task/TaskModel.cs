using System;
using System.Collections.Generic;
using System.Text;
using TaskExecutionSystem.DAL.Entities.Studies;
using TaskExecutionSystem.DAL.Entities.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using TaskExecutionSystem.DAL.Entities.Relations;
using TaskExecutionSystem.DAL.Entities.File;


namespace TaskExecutionSystem.DAL.Entities.Task
{
    public class TaskModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ContentText { get; set; }

        public DateTime BeginDate { get; set; }

        public DateTime FinishDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public bool IsOpen { get; set; }

        public TaskFile File { get; set; }


        public int TeacherId { get; set; }

        public int TypeId { get; set; }

        public int SubjectId { get; set; }

        public int GroupId { get; set; }


        [ForeignKey("TeacherId")]
        public Teacher Creator { get; set; }

        [ForeignKey("TypeId")]
        public TypeOfTask Type { get; set; }

        public Subject Subject { get; set; }

        public Group Group { get; set; }


        public List<TaskStudentItem> TaskStudentItems { get; set; }


        public TaskModel()
        {
            TaskStudentItems = new List<TaskStudentItem>();
        }
    }
}
