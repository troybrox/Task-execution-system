using System;
using System.Collections.Generic;
using System.Text;
using TaskExecutionSystem.DAL.Entities.File;

namespace TaskExecutionSystem.DAL.Entities.Repository
{
    public class Paragraph
    {
        public int Id { get; set; }

        public string ContentText { get; set; }

        public RepoFile FileItem { get; set; }


        public int ThemeId { get; set; }

        public Theme Theme { get; set; }
    }
}
