using System;
using System.Collections.Generic;
using System.Text;
using TaskExecutionSystem.BLL.DTO.File;
using TaskExecutionSystem.DAL.Entities.Repository;

namespace TaskExecutionSystem.BLL.DTO.Repository
{
    public class RepositoryDTO
    {
        public int Id { get; set; }

        public string Subject { get; set; }
        public int SubjectId { get; set; }

        public string Name { get; set; }
        public string ContentText { get; set; }

        public string TeacherName { get; set; }
        public string TeacherSurname { get; set; }
        public string TeacherPatronymic { get; set; }

        public List<FileDTO> Files { get; set; }


        public RepositoryDTO()
        {
            Files = new List<FileDTO>();
        }

        public static RepositoryDTO Map(RepositoryModel entity)
        {
            if (entity == null)
            {
                return null;
            }

            var dto = new RepositoryDTO
            {
                Id = entity.Id,
                Name = entity.Name,
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

            if (entity.Files.Count > 0)
            {
                foreach(var file in entity.Files)
                {
                    dto.Files.Add(FileDTO.Map(file));
                }
            }

            return dto;
        }
    }
}
