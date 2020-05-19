using System;
using System.Collections.Generic;
using System.Text;
using TaskExecutionSystem.DAL.Entities.Task;

namespace TaskExecutionSystem.BLL.DTO.Task
{
    public class TypeOfTaskDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }


        public static TypeOfTaskDTO Map(TypeOfTask entity) => new TypeOfTaskDTO
        {
            Id = entity.Id,
            Name = entity.Name
        };

    }
}
