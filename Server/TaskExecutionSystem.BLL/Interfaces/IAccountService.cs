using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TaskExecutionSystem.BLL.DTO;

namespace TaskExecutionSystem.BLL.Interfaces
{
    public interface IAccountService
    {
        public Task<OperationDetailDTO> RegisterTeacherAsync();

        public Task<OperationDetailDTO> RegisterStudentAsync();

        public Task<OperationDetailDTO> LoginAsync();
    }
}
