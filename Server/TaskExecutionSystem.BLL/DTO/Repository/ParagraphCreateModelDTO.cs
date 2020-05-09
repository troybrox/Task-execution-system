using System;
using System.Collections.Generic;
using System.Text;
using TaskExecutionSystem.DAL.Entities.Repository;

namespace TaskExecutionSystem.BLL.DTO.Repository
{
    public class ParagraphCreateModelDTO
    {
        public string Name { get; set; }

        public string ContentText { get; set; }

        public int ThemeId { get; set; }


        public static Paragraph Map(ParagraphCreateModelDTO dto)
        {
            var entity = new Paragraph
            {
                Name = dto.Name,
                ContentText = dto.ContentText
            };
            return entity;
        }
    }
}
