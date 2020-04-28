using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskExecutionSystem.DAL.Entities.Studies;
using TaskExecutionSystem.DAL.Entities.Relations;
using TaskExecutionSystem.DAL.Entities.Task;

namespace TaskExecutionSystem.DAL.Entities.Identity
{
    public class Teacher
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

        [Required]
        [Column(TypeName = "varchar(255)")]
        public string Position { get; set; }

        public int NotificationCounter { get; set; }


        public List<GroupTeacherSubjectItem> GroupTeacherSubjectItems { get; set; }

        public List<TaskModel> Tasks { get; set; }


        [Required]
        public long UserId { get; set; }

        [Required]
        public int DepartmentId { get; set; }


        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("DepartmentId")]
        public Department Department { get; set; }

        
        public Teacher()
        {
            GroupTeacherSubjectItems = new List<GroupTeacherSubjectItem>();

            Tasks = new List<TaskModel>();
        }
    }
}
