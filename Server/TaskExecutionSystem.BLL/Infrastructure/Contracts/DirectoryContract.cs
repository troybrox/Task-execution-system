using System;
using System.Collections.Generic;
using System.Text;

namespace TaskExecutionSystem.BLL.Infrastructure.Contracts
{
    public static class DirectoryContract
    {
        public const string FileURIBase = "https://localhost:44303/Files/";

        public const string RepoFilePath = "/Files/RepoFiles/";
        public const string TaskFilePath = "/Files/TaskFiles/";
        public const string SolutionFilePath = "/Files/SolutionFiles/";

        public const string RepoFileURI = "https://localhost:44303/Files/RepoFiles/";
        public const string TaskFileURI = "https://localhost:44303/Files/TaskFiles/";
        public const string SolutionFileURI = "https://localhost:44303/Files/SolutionFiles/";
    }
}
