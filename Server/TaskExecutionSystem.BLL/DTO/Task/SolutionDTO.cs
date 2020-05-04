using System;
using System.Collections.Generic;
using System.Text;
using TaskExecutionSystem.DAL.Entities.Task;

namespace TaskExecutionSystem.BLL.DTO.Task
{
    public class SolutionDTO
    {
        public int Id { get; set; }

        public int TaskId { get; set; } // [?]

        public int StudentId { get; set; } // [?]

        public string ContentText { get; set; }

        public DateTime CreationDate { get; set; }

        public bool IsInTime { get; set; }


        public static SolutionDTO Map(Solution entity) => new SolutionDTO
        {
            Id = entity.Id,
            ContentText = entity.ContentText,
            CreationDate = entity.CreationDate,
            IsInTime = entity.InTime,
            StudentId = entity.StudentId,
            TaskId = entity.TaskId
        };
    }
}
