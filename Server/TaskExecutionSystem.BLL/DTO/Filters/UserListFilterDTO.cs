using System;
using System.Collections.Generic;
using System.Text;

namespace TaskExecutionSystem.BLL.DTO.Filters
{
    public class UserListFilterDTO
    {
        public int FacultyId { get; set; }
    }

    public class StudentFilterDTO : UserListFilterDTO
    {
        public int GroupId { get; set; }
    }

    public class TeacherFilterDTO : UserListFilterDTO
    {
        public int DepartmentId { get; set; }
    }
}
