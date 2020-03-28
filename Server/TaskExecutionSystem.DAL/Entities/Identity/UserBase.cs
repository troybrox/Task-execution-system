using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskExecutionSystem.DAL.Entities.Identity
{
    public class UserBase : IdentityUser<long>
    {
        [Column(TypeName = "varchar(255)")]
        public override string Email { get; set; }

        [Required]
        [Column(TypeName = "varchar(255)")]
        public override string PasswordHash { get; set; }

        [Required]
        [Column(TypeName = "varchar(255)")]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "varchar(255)")]
        public string Surname { get; set; }

        [Required]
        [Column(TypeName = "varchar(255)")]
        public string Patronymic { get; set; }
    }
}
