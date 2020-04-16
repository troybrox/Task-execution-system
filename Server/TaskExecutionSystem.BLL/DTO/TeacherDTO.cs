using System;
using System.Collections.Generic;
using System.Text;

namespace TaskExecutionSystem.BLL.DTO
{
    public class TeacherDTO : UserDTO
    {
        public string Position { get; set; }

        public int DepartmentId { get; set; }
    }
}
