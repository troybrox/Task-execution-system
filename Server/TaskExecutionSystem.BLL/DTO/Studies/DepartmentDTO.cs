using System;
using System.Collections.Generic;
using TaskExecutionSystem.DAL.Entities.Studies;

namespace TaskExecutionSystem.BLL.DTO.Studies
{
    public class DepartmentDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int FacultyId { get; set; }


        public static DepartmentDTO Map(Department entity) => new DepartmentDTO
        {
            Id = entity.Id,
            Name = entity.Name,
            FacultyId = entity.FacultyId
        };

        public static List<DepartmentDTO> Map(List<Department> entities)
        {
            var res = new List<DepartmentDTO>();
            foreach(var entity in entities)
            {
                res.Add(DepartmentDTO.Map(entity));
            }
            return res;
        }
    }
}
