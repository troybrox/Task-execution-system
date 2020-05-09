using System;
using System.Collections.Generic;
using System.Text;
using TaskExecutionSystem.DAL.Entities.Repository;

namespace TaskExecutionSystem.BLL.DTO.Repository
{
    public class ThemeCreateModelDTO
    {
        public string Name { get; set; }

        public string ContentText { get; set; }

        public int RepositoryId { get; set; }


        public static Theme Map(ThemeCreateModelDTO dto)
        {
            var enity = new Theme
            {
                Name = dto.Name,
                ContentText = dto.ContentText, 
            };
            return enity;
        }
    }
}
