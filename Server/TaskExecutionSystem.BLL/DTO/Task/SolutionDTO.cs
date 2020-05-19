using System;
using System.Collections.Generic;
using System.Text;
using TaskExecutionSystem.DAL.Entities.Task;

namespace TaskExecutionSystem.BLL.DTO.Task
{
    public class SolutionDTO
    {
        public int Id { get; set; }
        public int TaskId { get; set; } 
        public int StudentId { get; set; } 

        public string ContentText { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public bool IsInTime { get; set; }

        public string FileURI { get; set; }
        public string FileName { get; set; }

        public string StudentName { get; set; }
        public string StudentSurname { get; set; }
        public string StudentPatronymic { get; set; }


        public static SolutionDTO Map(Solution entity) 
        {
            if(entity == null)
            {
                return null;
            }

            var dto = new SolutionDTO
            {
                Id = entity.Id,
                ContentText = entity.ContentText,
                CreationDate = entity.CreationDate,
                IsInTime = entity.InTime,
                StudentId = entity.StudentId,
                TaskId = entity.TaskId
            };

            if(entity.Student != null)
            {
                dto.StudentName = entity.Student.Name;
                dto.StudentSurname = entity.Student.Surname;
                dto.StudentPatronymic = entity.Student.Patronymic;
            }

            if (entity.File != null)
            {
                dto.FileURI = entity.File.FileURI;
                dto.FileName = entity.File.FileName;
            }

            return dto;
        }

    }
}
