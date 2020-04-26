using System;
using System.Collections.Generic;
using System.Text;
using TaskExecutionSystem.BLL.DTO;
using TaskExecutionSystem.BLL.DTO.Task;
using TaskExecutionSystem.BLL.DTO.File;
using System.Threading.Tasks;

namespace TaskExecutionSystem.BLL.Interfaces
{
    public interface ITaskService
    {
        public Task<OperationDetailDTO> CreateTaskAsync(TaskCreateModelDTO dto);

        public Task<OperationDetailDTO> CreateFileAsync();
    }
}
