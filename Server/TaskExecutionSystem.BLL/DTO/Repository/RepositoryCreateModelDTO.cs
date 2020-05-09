using System;
using System.Collections.Generic;
using System.Text;
using TaskExecutionSystem.DAL.Entities.Repository;

namespace TaskExecutionSystem.BLL.DTO.Repository
{
    public class RepositoryCreateModelDTO
    {
        public string Name { get; set; }

        public string ContentText { get; set; }

        public int SubjectId { get; set; }


        public static RepositoryModel Map(RepositoryCreateModelDTO dto)
        {
            var entity = new RepositoryModel
            {
                Name = dto.Name,
                ContentText = dto.ContentText
            };
            return entity;
        }
    }
}
