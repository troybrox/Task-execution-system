namespace TaskExecutionSystem.DAL.Entities.File
{
    public class FileModelBase
    {
        public int Id { get; set; }

        public string FileName { get; set; }

        public string Path { get; set; }

        public string FileURI { get; set; }
    }
}
