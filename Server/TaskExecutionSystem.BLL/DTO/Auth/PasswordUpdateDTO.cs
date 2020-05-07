using System;
using System.Collections.Generic;
using System.Text;

namespace TaskExecutionSystem.BLL.DTO.Auth
{
    public class PasswordUpdateDTO
    {
        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }
    }
}
