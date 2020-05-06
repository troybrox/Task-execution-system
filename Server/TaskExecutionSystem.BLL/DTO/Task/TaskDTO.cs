using System;

using System.Collections.Generic;
using System.Text;
using TaskExecutionSystem.BLL.DTO;
using TaskExecutionSystem.DAL.Entities.Task;

namespace TaskExecutionSystem.BLL.DTO.Task
{
    public class TaskDTO
    {
        public int Id { get; set; }
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
        public DateTime? UpdateDate { get; set; }
        public int TimeBar { get; set; }

        public string Group { get; set; }

        public int SolutionsCount { get; set; }

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
                Id = entity.Id,
                Name = entity.Name,
                Type = entity.Type.Name,
                Subject = entity.Subject.Name,

                ContentText = entity.ContentText,
                IsOpen = entity.IsOpen,

                BeginDate = entity.BeginDate,
                FinishDate = entity.FinishDate,
                UpdateDate = entity.UpdateDate,
            };
            if(entity.File != null)
            {
                dto.FileURI = entity.File.FileURI;
            }
            if(entity.Teacher != null)
            {
                dto.TeacherName = entity.Teacher.Name;
                dto.TeacherSurname = entity.Teacher.Surname;
                dto.TeacherPatronymic = entity.Teacher.Patronymic;
            }
            if (entity.Group != null)
            {
                dto.Group = entity.Group.NumberName;
            }
            return dto;
        }

        public static TaskDTO MapForTeacherList(TaskModel entity)
        {
            var dto = new TaskDTO
            {
                Id = entity.Id,
                Type = entity.Type.Name,
                //Subject = entity.Subject.Name,

                ContentText = entity.ContentText,
                IsOpen = entity.IsOpen,
                FileURI = entity.File.FileURI,

                BeginDate = entity.BeginDate,
                FinishDate = entity.FinishDate,
                UpdateDate = entity.UpdateDate,

                SolutionsCount = entity.Solutions.Count
            };
            return dto;
        }

    }
}
