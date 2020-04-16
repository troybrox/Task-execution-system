using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskExecutionSystem.DAL.Entities.Studies;

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


        [Required]
        public long UserId { get; set; }

        [Required]
        public int GroupId { get; set; }


        [ForeignKey("UserId")]
        public User User { get; set; }

        public Group Group { get; set; }
    }
}