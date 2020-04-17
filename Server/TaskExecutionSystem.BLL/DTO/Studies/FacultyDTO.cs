using System;
using System.Collections.Generic;
using TaskExecutionSystem.DAL.Entities.Studies;

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

        public static FacultyDTO Map(Faculty entity) => new FacultyDTO
        {
            Id = entity.Id,
            Name = entity.Name,
            Departmens = DepartmentDTO.Map(entity.Departments),
            Groups = GroupDTO.Map(entity.Groups)
        };
    }
}
