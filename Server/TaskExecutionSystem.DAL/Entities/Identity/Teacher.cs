﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskExecutionSystem.DAL.Entities.Studies;
using TaskExecutionSystem.DAL.Entities.Relations;

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


        public List<GroupTeacherSubjectItem> GroupTeacherSubjectItems { get; set; }


        [Required]
        public long UserId { get; set; }

        public int DepartmentId { get; set; }


        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("DepartmentId")]
        public Department Department { get; set; }

        
        public Teacher()
        {
            GroupTeacherSubjectItems = new List<GroupTeacherSubjectItem>();
        }
    }
}
