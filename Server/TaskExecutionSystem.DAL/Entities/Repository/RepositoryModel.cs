using System;
using System.Collections.Generic;
using System.Text;
using TaskExecutionSystem.DAL.Entities.Identity;
using TaskExecutionSystem.DAL.Entities.File;
using TaskExecutionSystem.DAL.Entities.Studies;

namespace TaskExecutionSystem.DAL.Entities.Repository
{
    public class RepositoryModel
    {
        public int Id { get; set; }

        public string ContentText { get; set; }

        public RepoFile File { get; set; }

        public List<Theme> Themes { get; set; } 


        public int SubjectId { get; set; }

        public int TeacherId { get; set; }


        public Subject Subject { get; set; }

        public Teacher Teacher { get; set; }


        public RepositoryModel()
        {
            Themes = new List<Theme>();
        }
    }
}
