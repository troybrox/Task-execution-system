using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskExecutionSystem.DAL.Interfaces
{
    public class IRegister
    {
        public int Id { get; set; }
         
        [Column(TypeName = "varchar(255)")]
        public string Email { get; set; }

        [Required]
        [Column(TypeName = "varchar(255)")]
        public string PasswordHash { get; set; }

        [Required]
        [Column(TypeName = "varchar(255)")]
        public string UserName { get; set; }
    }
}
