using System;
using System.Collections.Generic;
using System.Text;

namespace TaskExecutionSystem.BLL.DTO.Studies
{
    public class FacultyDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<GroupDTO> Groups { get; set; }

        public List<DepartmentDTO> Departmens { get; set; }

        public FacultyDTO()
        {
            Groups = new List<GroupDTO>();
            Departmens = new List<DepartmentDTO>();
        }
    }
}
