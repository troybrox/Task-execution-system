using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskExecutionSystem.DAL.Interfaces;
using TaskExecutionSystem.DAL.Entities.Identity;

namespace TaskExecutionSystem.DAL.Entities.Registration
{
    public class TeacherRegisterRequest : RegisterRequestBase
    {
        [Required]
        public int DepartmentId { get; set; }

        [Required]
        [Column(TypeName = "varchar(255)")]
        public string Position { get; set; }
    }
}
