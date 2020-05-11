using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskExecutionSystem.BLL.DTO;
using TaskExecutionSystem.BLL.DTO.Filters;
using TaskExecutionSystem.BLL.DTO.Studies;
using TaskExecutionSystem.BLL.DTO.Task;
using TaskExecutionSystem.BLL.DTO.Repository;
using TaskExecutionSystem.DAL.Entities.Identity;
using TaskExecutionSystem.DAL.Entities.Repository;

namespace TaskExecutionSystem.BLL.Interfaces
{
    public interface IRepoService
    {
        public Task<OperationDetailDTO> AddFileToRepoAsync(int id, string userfileName, string fileName = null);

        public Task<OperationDetailDTO> UpdateRepoAsync(RepositoryDTO dto);
    }
}
