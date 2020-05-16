using System;
using System.Collections.Generic;
using System.Text;
using TaskExecutionSystem.BLL.DTO;
using TaskExecutionSystem.BLL.DTO.Task;
using TaskExecutionSystem.BLL.DTO.File;
using System.Threading.Tasks;
using TaskExecutionSystem.BLL.DTO.Filters;

namespace TaskExecutionSystem.BLL.Interfaces
{
    public interface ITaskService
    {
        public Task<(bool Succeeded, string filePath, int fileId)> GetTaskFileNameAsync(int taskId);
        public Task<(bool Succeeded, string filePath, int fileId)> GetSolutionFileNameAsync(int solutionID);

        public Task<OperationDetailDTO> AddFileToTaskAsync(int taskID, string userFileName, string fileName = null);

        public Task<OperationDetailDTO> AddFileToSolutionAsync(int fileID, string userFileName, string fileName = null);

        public Task<OperationDetailDTO> UpdateTaskFileAsync(int fileID, string newUserFileName, string newFilePath);
        public Task<OperationDetailDTO> UpdateSolutionFileAsync(int fileID, string newUserFileName, string newUniqueFileName);

        public void GetCurrentTimePercentage(ref TaskDTO dto);

    }
}
