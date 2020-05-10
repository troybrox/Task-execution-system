using System;
using System.Collections.Generic;
using System.Text;
using TaskExecutionSystem.DAL.Entities.File;

namespace TaskExecutionSystem.BLL.DTO.File
{
    public class FileDTO
    {
        public string FileName { get; set; }

        //public string Path { get; set; }

        public string FileURI { get; set; }


        public static FileDTO Map(RepoFile entity)
        {
            var dto = new FileDTO
            {
                FileName = entity.FileName,
                FileURI = entity.FileURI,
            };
            return dto;
        }
    }
}
