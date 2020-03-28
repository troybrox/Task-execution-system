using System;
using System.Collections.Generic;
using System.Text;

namespace TaskExecutionSystem.BLL.DTO
{
    public class UserRegisterDTO
    {
        public string Email { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Patronymic { get; set; }
    }
}
