using System;
using System.Collections.Generic;
using System.Text;
using TaskExecutionSystem.DAL.Entities.File;

namespace TaskExecutionSystem.DAL.Entities.Repository
{
    public class Theme
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ContentText { get; set; }

        public RepoFile File { get; set; }

        public List<Paragraph> Paragraphs { get; set; }


        public int RepositoryId { get; set; }
        
        public RepositoryModel Repository { get; set; }


        public Theme()
        {
            Paragraphs = new List<Paragraph>();
        }
    }
}
