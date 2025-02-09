﻿using Microsoft.EntityFrameworkCore;
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
using TaskExecutionSystem.BLL.DTO.Repository;
using TaskExecutionSystem.DAL.Entities.Repository;

namespace TaskExecutionSystem.BLL.Services
{
    public class StudentService : IStudentService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        private readonly ITaskService _taskService;
        private readonly IUserValidator<User> _userValidator;

        public StudentService(DataContext context, IHttpContextAccessor httpContextAccessor,
            UserManager<User> userManager, ITaskService taskService, IUserValidator<User> userValidator)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _taskService = taskService;
            _userValidator = userValidator;
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


        public async Task<OperationDetailDTO> UpdateProfileDataAsync(StudentDTO newStudentDTO)
        {
            var detail = new OperationDetailDTO<TeacherDTO>();
            try
            {
                List<string> errorMessages = new List<string>();

                var currentUserEntity = await GetUserFromClaimsAsync();

                var studentUser = await _context.Users
                    .Where(u => u.Id == currentUserEntity.Id)
                    .FirstOrDefaultAsync();

                if (!UserValidator.Validate(newStudentDTO, out errorMessages))
                {
                    detail.ErrorMessages = errorMessages;
                    return detail;
                }

                if (await _context.StudentRegisterRequests.AnyAsync(x => x.UserName == newStudentDTO.UserName)
                    || await _context.TeacherRegisterRequests.AnyAsync(x => x.UserName == newStudentDTO.UserName)
                    || newStudentDTO.UserName != studentUser.UserName && await _userManager.FindByNameAsync(newStudentDTO.UserName) != null)
                {
                    detail.ErrorMessages.Add("Пользователь с таким именем пользователя уже существует, подберите другое.");
                    return detail;
                }

                if (studentUser != null)
                {
                    studentUser.Email = newStudentDTO.Email;
                    studentUser.UserName = newStudentDTO.UserName;

                    var validateRes = _userValidator.ValidateAsync(_userManager, studentUser);
                    if (validateRes.Result.Succeeded)
                    {
                        await _userManager.UpdateNormalizedEmailAsync(studentUser);
                        await _userManager.UpdateNormalizedUserNameAsync(studentUser);

                        detail.Succeeded = true;
                    }
                    else
                    {
                        detail.ErrorMessages.Add("Данные пользователя не прошли валидацию. Подробнее: " + validateRes.Result.Errors.ToList());
                        return detail;
                    }
                    await _context.SaveChangesAsync();
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
                                     .Include(t => t.Solutions)
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


                            case "isOpen":
                                {
                                    var value = filter.Value;
                                    if (!String.IsNullOrEmpty(value))
                                    {
                                        if(value == "true")
                                        {
                                            tasks = tasks
                                            .Where(t => t.IsOpen);
                                        }
                                        if(value == "false")
                                        {
                                            tasks = tasks
                                            .Where(t => !t.IsOpen);
                                        }
                                    }
                                    break;
                                }
                        }
                    }
                }

                foreach (var entity in tasks)
                {
                    var solution = new Solution();
                    SolutionDTO currentSolution = null;

                    if (entity.Solutions.Count > 0)
                    {
                        if((solution = entity.Solutions.Where(s => (s.StudentId == studentEntity.Id)).FirstOrDefault()) != null)
                        {
                            currentSolution = SolutionDTO.Map(solution);
                        }
                    }
                    entity.Solutions = null;
                    var taskDTO = TaskDTO.Map(entity);
                    taskDTO.Solution = currentSolution;
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
            SolutionDTO resSolutionDTO = null;
            Solution solutionEntity;

            try
            {
                var currentUserEntity = await GetUserFromClaimsAsync();

                var studentEntity = await _context.Students
                    .Include(s => s.User)
                    .Include(s => s.Solutions)
                    .Where(s => s.UserId == currentUserEntity.Id)
                    .FirstOrDefaultAsync();

                var taskEntity = await _context.TaskModels
                    .Include(t => t.Group)
                    .Include(t => t.Solutions)
                    .Include(t => t.Subject)
                    .Include(t => t.Type)
                    .Include(t => t.File)
                    .Include(t => t.Teacher)
                    .Include(t => t.TaskStudentItems)
                    .FirstOrDefaultAsync(t => t.Id == id);

                if (taskEntity == null)
                {
                    detail.ErrorMessages.Add("Задача не найдена.");
                    return detail;
                }

                resultTaskDTO = TaskDTO.Map(taskEntity);
                _taskService.GetCurrentTimePercentage(ref resultTaskDTO);

                solutionEntity = await _context.Solutions
                    .Include(s => s.Student)
                    .Include(s => s.File)
                    .Where(s => s.TaskId == id)
                    .Where(s => s.StudentId == studentEntity.Id)
                    .FirstOrDefaultAsync();


                if (solutionEntity != null)
                {
                    resSolutionDTO = SolutionDTO.Map(solutionEntity);
                }
                
                resultTaskDTO.Solution = resSolutionDTO;
                resultTaskDTO.Solutions = null;

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


        public async Task<OperationDetailDTO<string>> CreateSolutionAsync(SolutionCreateModelDTO dto)
        {
            var detail = new OperationDetailDTO<string>();
            try
            {
                var currentUserEntity = await GetUserFromClaimsAsync();

                var studentEntity = await _context.Students
                    .Include(s => s.User)
                    .Where(s => s.UserId == currentUserEntity.Id)
                    .FirstOrDefaultAsync();

                if (dto != null)
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

                    if (taskEntity.FinishDate > solutionEntity.CreationDate)
                    {
                        solutionEntity.InTime = true;
                    }

                    await _context.Solutions.AddAsync(solutionEntity);
                    await _context.SaveChangesAsync();

                    var createdSolution = await _context.Solutions.FirstOrDefaultAsync(t => t == solutionEntity);

                    if (createdSolution != null)
                    {
                        detail.Succeeded = true;
                        detail.Data = createdSolution.Id.ToString();
                    }
                    else
                    {
                        detail.ErrorMessages.Add(_serverErrorMessage + "При создании решения задачи что-то пошло не так.");
                    }
                    return detail;
                }

                else
                {
                    detail.ErrorMessages.Add("Параметр модели создаваемого решения был равен NULL.");
                    return detail;
                }
            }
            catch (Exception e)
            {
                detail.ErrorMessages.Add(_serverErrorMessage + e.Message);
                return detail;
            }
        }
        
        public async Task<OperationDetailDTO> UpdateSolutionAsync(SolutionCreateModelDTO dto)
        {
            var detail = new OperationDetailDTO<TaskDTO>();
            try
            {
                if (dto == null)
                {
                    detail.ErrorMessages.Add("Объект входного параметра был равен NULL.");
                    return detail;
                }

                var entity = await _context.Solutions.FindAsync(dto.Id);

                if (entity == null)
                {
                    detail.ErrorMessages.Add("Решение задачи не найдено.");
                    return detail;
                }

                entity.ContentText = dto.ContentText;
                entity.CreationDate = DateTime.Now;

                if (String.IsNullOrEmpty(entity.ContentText))
                {
                    detail.ErrorMessages.Add("Данные решения задачи введены некорректно. Поробуйте снова.");
                    return detail;
                }

                _context.Solutions.Update(entity);
                await _context.SaveChangesAsync();

                detail.Succeeded = true;
                return detail;
            }
            catch (Exception e)
            {
                detail.ErrorMessages.Add(_serverErrorMessage + e.Message);
                return detail;
            }
        }


        public async Task<OperationDetailDTO<List<SubjectDTO>>> GetRepoFiltersAsync()
        {
            var detail = new OperationDetailDTO<List<SubjectDTO>>();

            try
            {
                var currentUserEntity = await GetUserFromClaimsAsync();

                var studentEntity = await _context.Students
                    .Include(s => s.Group)
                    .ThenInclude(g => g.Faculty)
                    .Where(s => s.UserId == currentUserEntity.Id)
                    .FirstOrDefaultAsync();

                var resultSubjectDTOList = new List<SubjectDTO>();

                var subjects = await _context.Subjects
                    .Include(s => s.Repositories)
                    .Where(s => s.Repositories.Count > 0)
                    .ToListAsync();

                foreach (var subject in subjects)
                {
                    SubjectDTO subjectDTO = SubjectDTO.Map(subject);
                    resultSubjectDTOList.Add(subjectDTO);
                }

                resultSubjectDTOList.OrderBy(s => s.Name);

                detail.Data = resultSubjectDTOList;
                detail.Succeeded = true;
            }
            catch (Exception e)
            {
                detail.ErrorMessages.Add(_serverErrorMessage + e.Message);
            }
            
            return detail;
        }

        public async Task<OperationDetailDTO<List<RepositoryDTO>>> GetRepositoriesFromDBAsync(FilterDTO[] filters)
        {
            var detail = new OperationDetailDTO<List<RepositoryDTO>>();

            try
            {
                var resultList = new List<RepositoryDTO>();

                var repos = from r in _context.RepositoryModels
                            .Include(r => r.Subject)
                            .Include(r => r.Files)
                            .Include(r => r.Teacher)
                            select r;

                repos.OrderBy(r => r.Teacher.Name);

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
                                        repos = repos.Where(r => r.SubjectId == value);
                                    }
                                    break;
                                }
                        }
                    }
                }

                foreach (var entity in repos)
                {
                    var repoDTO = RepositoryDTO.Map(entity);
                    resultList.Add(repoDTO);
                }

                detail.Data = resultList;
                detail.Succeeded = true;
            }
            catch (Exception e)
            {
                detail.ErrorMessages.Add(_serverErrorMessage + e.Message);
            }

            return detail;
        }

        public async Task<OperationDetailDTO<RepositoryDTO>> GetRepositoryByID(int id)
        {
            var detail = new OperationDetailDTO<RepositoryDTO>();
            try
            {
                var repoEntity = await _context.RepositoryModels
                     .Include(r => r.Files)
                     .FirstOrDefaultAsync(r => r.Id == id);

                if (repoEntity != null)
                {
                    var dto = RepositoryDTO.Map(repoEntity);
                    detail.Data = dto;
                    detail.Succeeded = true;
                }
                else
                {
                    detail.ErrorMessages.Add("Репозиторий не найден");
                }
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
    }
}
