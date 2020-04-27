using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TaskExecutionSystem.BLL.DTO;
using TaskExecutionSystem.BLL.DTO.Task;

namespace TaskExecutionSystem.BLL.Interfaces
{
    public interface ITeacherService
    {
        public Task<OperationDetailDTO<List<TaskDTO>>> GetCurrentTeachersTaskFromDBAsync();
    }
}dsf
