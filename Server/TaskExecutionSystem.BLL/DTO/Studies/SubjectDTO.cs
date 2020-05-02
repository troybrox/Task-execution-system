using System;
using System.Collections.Generic;
using System.Text;
using TaskExecutionSystem.DAL.Entities.Studies;

namespace TaskExecutionSystem.BLL.DTO.Studies
{
    public class SubjectDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<GroupDTO> Groups { get; set; }


        public SubjectDTO()
        {
            Groups = new List<GroupDTO>();
        }

        // поиск групп, у кого есть задиния по данному предмет
        public static SubjectDTO Map(Subject entity)
        {
            var dto = new SubjectDTO
            {
                Id = entity.Id,
                Name = entity.Name
            };
            return dto;
        }

    }
}
