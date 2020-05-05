using System;
using System.Collections.Generic;
using TaskExecutionSystem.DAL.Entities.Studies;

namespace TaskExecutionSystem.BLL.DTO.Studies
{
    public class GroupDTO
    {
        public int Id { get; set; }

        public string Number { get; set; }

        public int FacultyId { get; set; }

        public List<StudentDTO> Students { get; set; }

        
        public GroupDTO()
        {
            Students = new List<StudentDTO>();
        }


        public static GroupDTO Map(Group entity)
        {
            var dto = new GroupDTO
            {
                Id = entity.Id,
                Number = entity.NumberName,
                FacultyId = entity.FacultyId
            };
            foreach(var s in entity.Students)
            {
                dto.Students.Add(StudentDTO.Map(s));
            }
            return dto;
        }

        public static GroupDTO MapWithoutStudents(Group entity)
        {
            var dto = new GroupDTO
            {
                Id = entity.Id,
                Number = entity.NumberName,
                FacultyId = entity.FacultyId
            };
            return dto;
        }


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
