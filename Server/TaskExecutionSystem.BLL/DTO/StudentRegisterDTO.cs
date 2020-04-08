﻿using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace TaskExecutionSystem.BLL.DTO
{
    public class StudentRegisterDTO : UserRegisterDTO
    {
        [Required]
        public string Group { get; set; }

        public string Faculty { get; set; }
    }
}
