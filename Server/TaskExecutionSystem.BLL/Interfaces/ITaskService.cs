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
        public Task<OperationDetailDTO> AddFileToTaskAsync(int taskID, string userFileName, string fileName = null);

        public Task<OperationDetailDTO> AddFileToSolutionAsync(int solutionID, string userFileName, string fileName = null);

        public void GetCurrentTimePercentage(ref TaskDTO dto);

    }
}
