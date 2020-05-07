using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskExecutionSystem.BLL.DTO;
using TaskExecutionSystem.BLL.DTO.Filters;
using TaskExecutionSystem.BLL.DTO.Studies;
using TaskExecutionSystem.BLL.DTO.Task;
using TaskExecutionSystem.DAL.Entities.Identity;

namespace TaskExecutionSystem.BLL.Interfaces
{
    public interface ITeacherService
    {
        // [профиль]
        public Task<OperationDetailDTO<TeacherDTO>> GetProfileDataAsync();
        public Task<OperationDetailDTO> UpdateProfileDataAsync(TeacherDTO dto); // модель в параметре ?? без данных для user 

        // данные для [задачи]
        public Task<OperationDetailDTO<string>> CreateNewTaskAsync(TaskCreateModelDTO dto);
        public Task<OperationDetailDTO<List<SubjectDTO>>> GetTaskFiltersAsync();
        public Task<OperationDetailDTO<TaskAddingFiltersModelDTO>> GetAddingTaskFiltersAsync();
        public Task<OperationDetailDTO<List<TaskDTO>>> GetTasksFromDBAsync(FilterDTO[] filters);
        public Task<OperationDetailDTO<TaskDTO>> GetTaskByIDAsync(int id);
        public Task<OperationDetailDTO> UpdateTaskAsync(TaskDTO dto);
        public Task<OperationDetailDTO> CloseTaskAsync(int id);

        // данные для [главная]
        public Task<OperationDetailDTO<List<SubjectDTO>>> GetMainDataAsync();

        // данные для [репозитория]
        public Task<OperationDetailDTO> CreateNewRepositoryAsync(); // param: RepoDTO
        public Task<OperationDetailDTO> CreateNewThemeAsync();
        public Task<OperationDetailDTO> CreateNewParagraphAsync();

        public Task<User> GetUserFromClaimsAsync();
    }
}
