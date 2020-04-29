using System;

using System.Collections.Generic;
using System.Text;
using TaskExecutionSystem.BLL.DTO;
using TaskExecutionSystem.DAL.Entities.Task;

namespace TaskExecutionSystem.BLL.DTO.Task
{
    public class TaskDTO
    {
        public string Type { get; set; }
        public string Subject { get; set; }
        public string Name { get; set; }
        public string ContentText { get; set; }
        public bool IsOpen { get; set; }
        public string FileURI { get; set; }
        
        public string TeacherName { get; set; }
        public string TeacherSurname { get; set; }
        public string TeacherPatronymic { get; set; }
        
        public DateTime BeginDate { get; set; }
        public DateTime FinishDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int TimeBar { get; set; }

        public string Group { get; set; }

        public List<StudentDTO> Students { get; set; }
        public List<SolutionDTO> Solutions { get; set; }

        public TaskDTO()
        {
            Students = new List<StudentDTO>();
            Solutions = new List<SolutionDTO>();
        }
        

        public static TaskDTO Map(TaskModel entity)
        {
            var dto = new TaskDTO
            {
                Type = entity.Type.Name,
                Subject = entity.Subject.Name,
                ContentText = entity.ContentText,
                IsOpen = entity.IsOpen,
                FileURI = entity.File.FileURI,
                TeacherName = entity.Teacher.Name,
                TeacherSurname = entity.Teacher.Surname,
                TeacherPatronymic = entity.Teacher.Patronymic,
                BeginDate = entity.BeginDate,
                FinishDate = entity.FinishDate,
                UpdateDate = entity.UpdateDate,
                Group = entity.Group.NumberName
            };
            return dto;
        }

    }
}
