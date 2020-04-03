using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskExecutionSystem.DAL.Entities.Identity
{
    public class User : IdentityUser<long>
    {
        [Column(TypeName = "varchar(255)")]
        public override string Email { get; set; }

        [Required]
        [Column(TypeName = "varchar(255)")]
        public override string PasswordHash { get; set; }

        [Required]
        [Column(TypeName = "varchar(255)")]
        public override string UserName { get; set; }


        [Required]
        public int EntityId { get; set; }

        public Student Student { get; set; }

        public Teacher Teacher { get; set; }
    }
}
