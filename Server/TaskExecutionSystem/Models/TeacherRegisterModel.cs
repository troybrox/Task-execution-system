using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskExecutionSystem.Models
{
    public class TeacherRegisterModel : RegisterModelBase
    {
        public string Position { get; set; }

        public string Department { get; set; }
    }
}
