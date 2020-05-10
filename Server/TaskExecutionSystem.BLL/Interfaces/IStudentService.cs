using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskExecutionSystem.BLL.DTO;
using TaskExecutionSystem.BLL.DTO.Filters;
using TaskExecutionSystem.BLL.DTO.Repository;
using TaskExecutionSystem.BLL.DTO.Studies;
using TaskExecutionSystem.BLL.DTO.Task;
using TaskExecutionSystem.DAL.Entities.Identity;

namespace TaskExecutionSystem.BLL.Interfaces
{
    public interface IStudentService
    {
        // [профиль]
        public Task<OperationDetailDTO<StudentDTO>> GetProfileDataAsync();
        public Task<OperationDetailDTO> UpdateProfileDataAsync(StudentDTO dto); // модель в параметре ?? без данных для user 

        // данные для [задачи]
        public Task<OperationDetailDTO<List<TaskDTO>>> GetTasksFromDBAsync(FilterDTO[] filters = null);
        public Task<OperationDetailDTO<TaskFiltersModelDTO>> GetTaskFiltersAsync();
        public Task<OperationDetailDTO<TaskDTO>> GetTaskByIDAsync(int id);

        // решение
        public Task<OperationDetailDTO<string>> CreateSolutionAsync(SolutionCreateModelDTO dto);

        public Task<OperationDetailDTO<List<SubjectDTO>>> GetRepoFiltersAsync();
        public Task<OperationDetailDTO<List<RepositoryDTO>>> GetRepositoriesFromDBAsync(FilterDTO[] filters);
        public Task<OperationDetailDTO<RepositoryDTO>> GetRepositoryByID(int id);

        // данные для [репозиторий]
        // get Repo subjects
    }
}
