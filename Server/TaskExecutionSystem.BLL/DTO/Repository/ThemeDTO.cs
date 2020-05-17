using System;
using System.Collections.Generic;
using System.Text;
using TaskExecutionSystem.DAL.Entities.Repository;

namespace TaskExecutionSystem.BLL.DTO.Repository
{
    public class ThemeDTO
    {
        public int Id { get; set; }

        public string ContentText { get; set; }

        public int RepositoryId { get; set; }

        public string FileURI { get; set; }
        public string FileName { get; set; }

        public List<ParagraphDTO> Paragraphs { get; set; }


        public ThemeDTO()
        {
            Paragraphs = new List<ParagraphDTO>();
        }


        public static ThemeDTO Map(Theme entity)
        {
            var dto = new ThemeDTO
            {
                Id = entity.Id,
                ContentText = entity.ContentText
            };

            if (entity.Repository != null)
            {
                dto.RepositoryId = entity.Repository.Id;
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
