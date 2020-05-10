using TaskExecutionSystem.DAL.Entities.Repository;

namespace TaskExecutionSystem.DAL.Entities.File
{
    public class RepoFile : FileModelBase
    {
        public int RepositoryModelId { get; set; }

        public RepositoryModel RepositoryModel { get; set; }

    }
}
