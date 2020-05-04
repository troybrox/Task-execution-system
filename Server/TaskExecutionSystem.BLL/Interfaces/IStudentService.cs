using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TaskExecutionSystem.BLL.DTO;
using TaskExecutionSystem.BLL.DTO.Filters;
using TaskExecutionSystem.BLL.DTO.Studies;
using TaskExecutionSystem.BLL.DTO.Task;

namespace TaskExecutionSystem.BLL.Interfaces
{
    public interface IStudentService
    {
        // [профиль]
        public Task<OperationDetailDTO> UpdateProfileDataAsync(StudentDTO dto); // модель в параметре ?? без данных для user 

        // данные для [задачи]
        public Task<OperationDetailDTO<List<TaskDTO>>> GetTasksFromDBAsync(FilterDTO[] filters);
        public Task<OperationDetailDTO<List<SubjectDTO>>> GetTaskFiltersAsync();
        public Task<OperationDetailDTO<TaskDTO>> GetTaskByIDAsync(int id);

        // данные для [репозиторий]
        public Task<OperationDetailDTO> CreateNewRepositoryAsync(); // param: RepoDTO
        public Task<OperationDetailDTO> CreateNewThemeAsync();
        public Task<OperationDetailDTO> CreateNewParagraphAsync();
    }
}
