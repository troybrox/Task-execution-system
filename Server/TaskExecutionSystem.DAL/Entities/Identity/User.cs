using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskExecutionSystem.DAL.Interfaces;

namespace TaskExecutionSystem.DAL.Entities.Identity
{
    public class User : IdentityUser<long>, IEntity
    {
        [Column(TypeName = "varchar(255)")]
        public override string Email { get; set; }

        [Required]
        [Column(TypeName = "varchar(255)")]
        public override string PasswordHash { get; set; }

        [Required]
        [Column(TypeName = "varchar(255)")]
        public override string UserName { get; set; }

        public int EntityId { get; set; }


        public Student Student { get; set; }

        public Teacher Teacher { get; set; }
    }
}
