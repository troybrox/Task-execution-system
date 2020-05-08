using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskExecutionSystem.BLL.DTO;
using TaskExecutionSystem.BLL.DTO.Filters;
using TaskExecutionSystem.BLL.DTO.Studies;
using TaskExecutionSystem.BLL.DTO.Task;
using static TaskExecutionSystem.BLL.Infrastructure.Contracts.ErrorMessageContract;
using TaskExecutionSystem.BLL.Interfaces;
using TaskExecutionSystem.DAL.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using TaskExecutionSystem.DAL.Entities.Identity;
using TaskExecutionSystem.DAL.Entities.Studies;
using TaskExecutionSystem.DAL.Entities.Task;
using TaskExecutionSystem.BLL.Validation;
using TaskExecutionSystem.DAL.Entities.Relations;

namespace TaskExecutionSystem.BLL.Services
{
    // TODO: Repository - create, get, update, delete

    public class StudentService : IStudentService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        private readonly ITaskService _taskService;

        public StudentService(DataContext context, IHttpContextAccessor httpContextAccessor,
            UserManager<User> userManager, ITaskService taskService)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _taskService = taskService;
        }

        public async Task<OperationDetailDTO<StudentDTO>> GetProfileDataAsync()
        {
            var detail = new OperationDetailDTO<StudentDTO>();
            try
            {
                var currentUserEntity = await GetUserFromClaimsAsync();

                var studentEntity = await _context.Students
                    .Include(s => s.User)
                    .Include(s => s.Group)
                    .ThenInclude(g => g.Faculty)
                    .Where(s => s.UserId == currentUserEntity.Id)
                    .FirstOrDefaultAsync();

                var dto = StudentDTO.Map(studentEntity);

                detail.Succeeded = true;
                detail.Data = dto;
                return detail;
            }
            catch (Exception e)
            {
                detail.Succeeded = false;
                detail.ErrorMessages.Add(_serverErrorMessage + e.Message);
                return detail;
            }
        }

        // TODO [!]
        public async Task<OperationDetailDTO> UpdateProfileDataAsync(StudentDTO dto)
        {
            var detail = new OperationDetailDTO<TeacherDTO>();
            try
            {
                List<string> errorMessages = new List<string>();

                var currentUserEntity = await GetUserFromClaimsAsync();

                var studentUser = await _context.Users
                    .Where(u => u.Id == currentUserEntity.Id)
                    .FirstOrDefaultAsync();

                var teacherEntity = await _context.Students
                    .Include(t => t.User)
                    .Where(t => t.UserId == currentUserEntity.Id)
                    .FirstOrDefaultAsync();


                if (!UserValidator.Validate(dto, out errorMessages))
                {
                    detail.Succeeded = false;
                    detail.ErrorMessages = errorMessages;
                    return detail;
                }

                if (await _context.StudentRegisterRequests.AnyAsync(x => x.UserName == dto.UserName)
                    || await _context.TeacherRegisterRequests.AnyAsync(x => x.UserName == dto.UserName)
                    || await _userManager.FindByNameAsync(dto.UserName) != null)
                {
                    detail.Succeeded = false;
                    detail.ErrorMessages.Add("Пользователь с таким именем пользователя уже существует, подберите другое.");
                    return detail;
                }

                if (studentUser != null)
                {
                    studentUser.Email = dto.Email;
                    studentUser.UserName = dto.UserName;
                    var userUpdateResult = await _userManager.UpdateAsync(studentUser);
                    detail.Succeeded = true;
                }

                return detail;
            }
            catch (Exception e)
            {
                detail.ErrorMessages.Add(_serverErrorMessage + e.Message);
                return detail;
            }
        }


        public async Task<OperationDetailDTO<List<TaskDTO>>> GetTasksFromDBAsync(FilterDTO[] filters = null)
        {
            var detail = new OperationDetailDTO<List<TaskDTO>>();
            var resultList = new List<TaskDTO>();

            try
            {
                var currentUserEntity = await GetUserFromClaimsAsync();

                var studentEntity = await _context.Students
                    .Include(s => s.User)
                    .Where(s => s.UserId == currentUserEntity.Id)
                    .FirstOrDefaultAsync();


               var tasks = from t in _context.TaskModels
                                     .Include(t => t.Teacher)
                                     .Include(t => t.Subject)
                                     .Include(t => t.Type)
                                     .Where(t => (t.TaskStudentItems.FirstOrDefault(x => x.StudentId == studentEntity.Id) != null))
                                                             select t;

                tasks.OrderBy(t => t.BeginDate);

                if (filters != null)
                {
                    foreach (var filter in filters)
                    {
                        switch (filter.Name)
                        {
                            case "subjectId":
                                {
                                    var value = Convert.ToInt32(filter.Value);
                                    if (value > 0)
                                    {
                                        tasks = tasks.Where(t => t.SubjectId == value);
                                    }
                                    break;
                                }

                            case "typeId":
                                {
                                    var value = Convert.ToInt32(filter.Value);
                                    if (value > 0)
                                    {
                                        tasks = tasks.Where(t => t.TypeId == value);
                                    }
                                    break;
                                }

                            case "searchString":
                                {
                                    var value = filter.Value;
                                    if (!String.IsNullOrEmpty(value))
                                    {
                                        tasks = tasks.Where(t => t.Name.ToUpper().Contains(value)
                                        || t.ContentText.ToUpper().Contains(value));
                                    }
                                    break;
                                }
                        }
                    }
                }

                foreach (var entity in tasks)
                {
                    var taskDTO = TaskDTO.Map(entity);
                    resultList.Add(taskDTO);
                }

                detail.Succeeded = true;
                detail.Data = resultList;
                return detail;
            }
            catch (Exception e)
            {
                detail.Succeeded = false;
                detail.ErrorMessages.Add(_serverErrorMessage + e.Message);
                return detail;
            }
        }

        public async Task<OperationDetailDTO<TaskFiltersModelDTO>> GetTaskFiltersAsync()
        {
            var detail = new OperationDetailDTO<TaskFiltersModelDTO>();
            try
            {
                var resSubjectDTOList = new List<SubjectDTO>();
                var resTypeDTOList = new List<TypeOfTaskDTO>();

                var currentUserEntity = await GetUserFromClaimsAsync();

                var studentEntity = await _context.Students
                    .Include(s => s.User)
                    .Include(s => s.Group)
                    .ThenInclude(g => g.Faculty)
                    .Include(s => s.TaskStudentItems)
                    .Where(s => s.UserId == currentUserEntity.Id)
                    .FirstOrDefaultAsync();

                IQueryable<Subject> subjects = from s in _context.Subjects select s;

                IQueryable<TypeOfTask> types = from t in _context.TaskTypes select t;

                // получить все задания студента
                IQueryable<TaskModel> tasks = from t in _context.TaskModels
                                     .Include(t => t.Teacher)
                                     .Include(t => t.Group)
                                     .Include(t => t.Subject)
                                     .Include(t => t.Type)
                                     .Where(t => (t.TaskStudentItems.FirstOrDefault(x => x.StudentId == studentEntity.Id) != null))
                                                             select t;

                foreach (var task in tasks)
                {
                    var subjectDTO = new SubjectDTO();
                    var typeDTO = new TypeOfTaskDTO();

                    if ((subjectDTO = resSubjectDTOList.FirstOrDefault(s => s.Id == task.SubjectId)) != null)
                    { }
                    else
                    {
                        subjectDTO = SubjectDTO.Map(task.Subject);
                        resSubjectDTOList.Add(subjectDTO);
                    }

                    if((typeDTO = resTypeDTOList.FirstOrDefault(t => t.Id == task.TypeId)) != null) 
                    { }
                    else
                    {
                        typeDTO = TypeOfTaskDTO.Map(task.Type);
                        resTypeDTOList.Add(typeDTO);
                    }
                }

                detail.Data = new TaskFiltersModelDTO
                {
                    Subjects = resSubjectDTOList,
                    Types = resTypeDTOList
                };

                detail.Succeeded = true;
                return detail;
            }
            catch (Exception e)
            {
                detail.ErrorMessages.Add(_serverErrorMessage + e.Message);
                return detail;
            }
        }

        public async Task<OperationDetailDTO<TaskDTO>> GetTaskByIDAsync(int id)
        {
            var detail = new OperationDetailDTO<TaskDTO>();
            var resultTaskDTO = new TaskDTO();
            var resSolutionDTO = new SolutionDTO();
            var resStudentDTOList = new List<StudentDTO>();

            try
            {
                var currentUserEntity = await GetUserFromClaimsAsync();

                var studentEntity = await _context.Students
                    .Include(s => s.User)
                    .Include(s => s.Solutions)
                    .Where(s => s.UserId == currentUserEntity.Id)
                    .FirstOrDefaultAsync();

                var taskEntity = await _context.TaskModels
                    .Include(t => t.Solutions)
                    .Include(t => t.Subject)
                    .Include(t => t.Type)
                    .Include(t => t.File)
                    .Include(t => t.Teacher)
                    .FirstOrDefaultAsync(t => t.Id == id);

                var solutionEntity = studentEntity.Solutions.FirstOrDefault(s => s.TaskId == id);

                if (taskEntity == null)
                {
                    detail.ErrorMessages.Add("Задача не найдена.");
                    return detail;
                }

                resultTaskDTO = TaskDTO.Map(taskEntity);
                if(resSolutionDTO != null)
                {
                    resultTaskDTO.Solutions.Add(resSolutionDTO);
                }
                _taskService.GetCurrentTimePercentage(ref resultTaskDTO);
                detail.Data = resultTaskDTO;
                detail.Succeeded = true;
                return detail;
            }
            catch (Exception e)
            {
                detail.ErrorMessages.Add(_serverErrorMessage + e.Message);
                return detail;
            }
        }

        private async Task<User> GetUserFromClaimsAsync()
        {
            var userNameClaim = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
            string stringID = userNameClaim.Value;
            var user = await _userManager.FindByIdAsync(stringID);
            return user;
        }

        // TODO: try - catch
        public async Task<OperationDetailDTO> CreateSolutionAsync(SolutionCreateModelDTO dto)
        {
            var detail = new OperationDetailDTO();

            var currentUserEntity = await GetUserFromClaimsAsync();

            var studentEntity = await _context.Students
                .Include(s => s.User)
                .Where(s => s.UserId == currentUserEntity.Id)
                .FirstOrDefaultAsync();

            if(dto != null)
            {
                var taskEntity = await _context.TaskModels.FindAsync(dto.TaskId);

                if (taskEntity == null)
                {
                    detail.ErrorMessages.Add("Задача не найдена.");
                    return detail;
                }

                Solution solutionEntity = new Solution
                {
                    ContentText = dto.ContentText,
                    Student = studentEntity,
                    TaskModel = taskEntity,
                    CreationDate = DateTime.Now
                };
                if (solutionEntity.CreationDate > taskEntity.FinishDate)
                {
                    solutionEntity.InTime = false;
                }

                await _context.Solutions.AddAsync(solutionEntity);
                await _context.SaveChangesAsync();

                detail.Succeeded = true;
                return detail;
            }

            else
            {
                detail.ErrorMessages.Add("Параметр модели создаваемого решения был равен NULL.");
                return detail;
            }
        }
    }
}
