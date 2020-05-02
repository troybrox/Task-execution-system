using System;
using System.Collections.Generic;
using TaskExecutionSystem.DAL.Entities.Studies;

namespace TaskExecutionSystem.BLL.DTO.Studies
{
    public class GroupDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int FacultyId { get; set; }

        public List<StudentDTO> Students { get; set; }

        
        public GroupDTO()
        {
            Students = new List<StudentDTO>();
        }


        public static GroupDTO Map(Group entity) => new GroupDTO
        {
            Id = entity.Id,
            Name = entity.NumberName,
            FacultyId = entity.FacultyId
        };

        public static List<GroupDTO> Map(List<Group> entities)
        {
            var res = new List<GroupDTO>();
            foreach (var entity in entities)
            {
                res.Add(GroupDTO.Map(entity));
            }
            return res;
        }
    }
}
