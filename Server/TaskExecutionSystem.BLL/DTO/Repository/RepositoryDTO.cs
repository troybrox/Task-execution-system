using System;
using System.Collections.Generic;
using System.Text;
using TaskExecutionSystem.DAL.Entities.Repository;

namespace TaskExecutionSystem.BLL.DTO.Repository
{
    public class RepositoryDTO
    {
        public int Id { get; set; }

        public string Subject { get; set; }
        public int SubjectId { get; set; }

        public string ContentText { get; set; }

        public string FileURI { get; set; }
        public string FileName { get; set; }

        public string TeacherName { get; set; }
        public string TeacherSurname { get; set; }
        public string TeacherPatronymic { get; set; }

        public List<ThemeDTO> Themes { get; set; }


        public RepositoryDTO()
        {
            Themes = new List<ThemeDTO>();
        }

        public static RepositoryDTO Map(RepositoryModel entity)
        {
            var dto = new RepositoryDTO
            {
                Id = entity.Id,
                ContentText = entity.ContentText
            };

            if(entity.Subject != null)
            {
                dto.Subject = entity.Subject.Name;
                dto.SubjectId = entity.Subject.Id;
            }

            if (entity.Teacher != null)
            {
                dto.TeacherName = entity.Teacher.Name;
                dto.TeacherSurname = entity.Teacher.Surname;
                dto.TeacherPatronymic = entity.Teacher.Patronymic;
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
