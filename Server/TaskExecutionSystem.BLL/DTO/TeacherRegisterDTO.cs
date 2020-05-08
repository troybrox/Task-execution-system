using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace TaskExecutionSystem.BLL.DTO
{
    public class TeacherRegisterDTO : UserRegisterDTO
    {
        [Required]
        public string Position { get; set; }

        public int DepartmentId { get; set; }
    }
}
