using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskExecutionSystem.DAL.Entities.Identity
{
    public class Teacher
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

        [Required]
        [Column(TypeName = "varchar(255)")]
        public string Position { get; set; }

        [Required]
        [Column(TypeName = "varchar(255)")]
        public string Department { get; set; }


        public string MainSubject { get; set; }


        [ForeignKey("UserId")]
        public User UserEntity { get; set; }


        public Teacher()
        {

        }
    }
}
