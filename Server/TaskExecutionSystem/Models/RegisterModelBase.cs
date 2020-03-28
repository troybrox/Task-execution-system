using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskExecutionSystem.Models
{
    public class RegisterModelBase
    {
        public string Email { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Patronymic { get; set; }
    }
}
