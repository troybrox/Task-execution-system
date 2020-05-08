using TaskExecutionSystem.DAL.Entities.Repository;

namespace TaskExecutionSystem.DAL.Entities.File
{
    public class RepoFile : FileModelBase
    {
        public int? RepositoryItemId { get; set; }

        public int? ThemeId { get; set; }

        public int? ParagraphId { get; set; }


        public RepositoryModel RepositoryItem { get; set; }

        public Theme Theme { get; set; }

        public Paragraph Paragraph { get; set; }
    }
}
