﻿using System;
using System.Collections.Generic;
using System.Text;
using TaskExecutionSystem.DAL.Entities.Identity;

namespace TaskExecutionSystem.BLL.DTO
{
    public class SignInUserDetailDTO
    {
        public User User { get; set; }

        public IList<string> UserRoles { get; set; }
    }
}
