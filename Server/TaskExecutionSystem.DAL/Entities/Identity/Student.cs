using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskExecutionSystem.DAL.Entities.Identity
{
    public class Student
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public long UserId { get; set; }

        [Required]
        [Column(TypeName = "varchar(255)")]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "varchar(255)")]
        public string Surname { get; set; }

        [Required]
        [Column(TypeName = "varchar(255)")]
        public string Patronymic { get; set; }

        public string StudyGroupNumber { get; set; }

        public int FacultyNumber { get; set; }


        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}