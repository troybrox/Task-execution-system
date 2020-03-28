using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskExecutionSystem.DAL.Entities.Identity
{
    public class StudentUser : UserBase
    {
        [Required]
        public int StudyGroupId { get; set; }

        // grop entity
    }
}