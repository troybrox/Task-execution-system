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
    public interface ITeacherService
    {
        // [профиль]
        public Task<OperationDetailDTO<TeacherDTO>> GetProfileDataAsync();
        public Task<OperationDetailDTO> UpdateProfileDataAsync(TeacherDTO dto); // модель в параметре ?? без данных для user 

        // данные для [задачи]
        public Task<OperationDetailDTO<TaskDTO>> CreateNewTaskAsync(TaskCreateModelDTO dto);
        public Task<OperationDetailDTO<List<SubjectDTO>>> GetTaskFiltersAsync();
        public Task<OperationDetailDTO<List<SubjectDTO>>> GetAddingTaskFiltersAsync();
        public Task<OperationDetailDTO<List<TaskDTO>>> GetTasksFromDBAsync(FilterDTO[] filters);

        // данные для [главная]
        public Task<OperationDetailDTO<List<SubjectDTO>>> GetMainDataAsync();

        // данные для [репозитория]
        public Task<OperationDetailDTO> CreateNewRepositoryAsync(); // param: RepoDTO
        public Task<OperationDetailDTO> CreateNewThemeAsync();
        public Task<OperationDetailDTO> CreateNewParagraphAsync();
    }
}
