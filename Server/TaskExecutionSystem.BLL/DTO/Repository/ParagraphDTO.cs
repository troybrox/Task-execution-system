using System;
using System.Collections.Generic;
using System.Text;
using TaskExecutionSystem.DAL.Entities.Repository;

namespace TaskExecutionSystem.BLL.DTO.Repository
{
    public class ParagraphDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ContentText { get; set; }

        public int ThemeId { get; set; }

        public string FileURI { get; set; }
        public string FileName { get; set; }


        public static ParagraphDTO Map(Paragraph entity)
        {
            var dto = new ParagraphDTO
            {
                Id = entity.Id,
                ContentText = entity.ContentText
            };

            if (entity.Theme != null)
            {
                dto.ThemeId = entity.Theme.Id;
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
