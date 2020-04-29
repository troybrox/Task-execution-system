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
        public Task<OperationDetailDTO<List<TaskDTO>>> GetTasksFromDBAsync(FilterDTO[] filters);
        public Task<OperationDetailDTO<TaskDTO>> GetTaskByIDAsync(int id);
        public Task<OperationDetailDTO> CreateTaskAsync(TaskCreateModelDTO dto);
        public Task<OperationDetailDTO> AddFileToTaskAsync(int taskID, string fileName);
        public Task<OperationDetailDTO> UpdateTaskAsync();

    }
}
