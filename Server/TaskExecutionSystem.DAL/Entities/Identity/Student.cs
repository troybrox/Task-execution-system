using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskExecutionSystem.DAL.Entities.Relations;
using TaskExecutionSystem.DAL.Entities.Studies;
using TaskExecutionSystem.DAL.Entities.Task;

namespace TaskExecutionSystem.DAL.Entities.Identity
{
    public class Student
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(255)")]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "varchar(255)")]
        public string Surname { get; set; }

        [Required]
        [Column(TypeName = "varchar(255)")]
        public string Patronymic { get; set; }

        public int NotificationCounter { get; set; }


        [Required]
        public long UserId { get; set; }

        [Required]
        public int GroupId { get; set; }


        [ForeignKey("UserId")]
        public User User { get; set; }

        public Group Group { get; set; }


        public List<TaskStudentItem> TaskStudentItems { get; set; }

        public List<Solution> Solutions { get; set; }


        public Student()
        {
            TaskStudentItems = new List<TaskStudentItem>();

            Solutions = new List<Solution>();
        }
    }
}