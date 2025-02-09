﻿using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskExecutionSystem.DAL.Interfaces;
using TaskExecutionSystem.DAL.Entities.Identity;
using TaskExecutionSystem.DAL.Entities.Studies;

namespace TaskExecutionSystem.DAL.Entities.Registration
{
    public class StudentRegisterRequest : RegisterRequestBase
    {
        [Required]
        public int GroupId { get; set; }

        [ForeignKey("GroupId")]
        public Group Group { get;set; }
    }
}
