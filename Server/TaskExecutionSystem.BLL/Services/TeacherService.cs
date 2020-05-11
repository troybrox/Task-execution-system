using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskExecutionSystem.BLL.DTO;
using TaskExecutionSystem.BLL.DTO.Auth;
using TaskExecutionSystem.BLL.DTO.Filters;
using TaskExecutionSystem.BLL.DTO.Studies;
using TaskExecutionSystem.BLL.DTO.Task;
using static TaskExecutionSystem.BLL.Infrastructure.Contracts.DirectoryContract;
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
using TaskExecutionSystem.DAL.Entities.File;

namespace TaskExecutionSystem.BLL.Services
{
    // TODO: CLOSE TASK
    // TODO: Repository - create, get, update, delete
    public class TeacherService : ITeacherService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        private readonly ITaskService _taskService;

        public TeacherService(DataContext context, IHttpContextAccessor httpContextAccessor, 
            UserManager<User> userManager, ITaskService taskService)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _taskService = taskService;
        }

        // получение данных профиля преподавателя
        public async Task<OperationDetailDTO<TeacherDTO>> GetProfileDataAsync()
        {
            var detail = new OperationDetailDTO<TeacherDTO>();
            try
            {
                var currentUserEntity = await GetUserFromClaimsAsync();

                var teacherEntity = await _context.Teachers
                    .Include(t => t.User)
                    .Include(t => t.Department)
                    .ThenInclude(d => d.Faculty)
                    .Where(t => t.UserId == currentUserEntity.Id)
                    .FirstOrDefaultAsync();

                var dto = TeacherDTO.Map(teacherEntity);

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

        // todo: userName - PK, добавить изменение для сввязанных сущностей - teacher
        // изменение данных профиля преподавателя
        public async Task<OperationDetailDTO> UpdateProfileDataAsync(TeacherDTO newTeacherDTO)
        {
            var detail = new OperationDetailDTO<TeacherDTO>();
            try
            {
                List<string> errorMessages = new List<string>();

                var currentUserEntity = await GetUserFromClaimsAsync();

                var teacherUser = await _context.Users
                    .Where(u => u.Id == currentUserEntity.Id)
                    .FirstOrDefaultAsync();

                var teacherEntity = await _context.Teachers
                    .Include(t => t.User)
                    .Where(t => t.UserId == currentUserEntity.Id)
                    .FirstOrDefaultAsync();


                if (!UserValidator.Validate(newTeacherDTO, out errorMessages))
                {
                    detail.Succeeded = false;
                    detail.ErrorMessages = errorMessages;
                    return detail;
                }

                User findSameUser;

                if (await _context.StudentRegisterRequests.AnyAsync(x => x.UserName == newTeacherDTO.UserName)
                    || await _context.TeacherRegisterRequests.AnyAsync(x => x.UserName == newTeacherDTO.UserName)
                    || ((findSameUser = await _userManager.FindByNameAsync(newTeacherDTO.UserName)) != null && findSameUser != teacherUser))
                {
                    detail.Succeeded = false;
                    detail.ErrorMessages.Add("Пользователь с таким именем пользователя уже существует, подберите другое.");
                    return detail;
                }

                if (teacherUser != null)
                {
                    teacherUser.Email = newTeacherDTO.Email;
                    teacherUser.UserName = newTeacherDTO.UserName;
                    teacherUser.Teacher = null;

                    // delete dependent
                    teacherEntity.User = null;
                    _context.Teachers.Update(teacherEntity);
                    await _context.SaveChangesAsync();

                    var userUpdateResult = await _userManager.UpdateAsync(teacherUser);

                    // update dependent
                    teacherEntity.User = teacherUser;
                    _context.Teachers.Update(teacherEntity);
                    await _context.SaveChangesAsync();

                    if (teacherEntity != null)
                    {
                        teacherEntity.Position = newTeacherDTO.Position;
                        //
                        teacherEntity.User = teacherUser;
                        //
                        _context.Teachers.Update(teacherEntity);
                        await _context.SaveChangesAsync();
                    }

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

        // - только тот факультет, на котором препод
        // получение списков-фильтров для создания задачи
        // форируются списки существующих предметов, типов, групп для отправки на клиент
        public async Task<OperationDetailDTO<TaskFiltersModelDTO>> GetAddingTaskFiltersAsync()
        {
            var detail = new OperationDetailDTO<TaskFiltersModelDTO>();

            try
            {
                var currentUserEntity = await GetUserFromClaimsAsync();

                //var teacherUser = await _context.Users
                //    .Where(u => u.Id == currentUserEntity.Id)
                //    .FirstOrDefaultAsync();

                var teacherEntity = await _context.Teachers
                    .Include(t => t.User)
                    .Include(t => t.Department)
                    .ThenInclude(d => d.Faculty)
                    .Where(t => t.UserId == currentUserEntity.Id)
                    .FirstOrDefaultAsync();

                var types = await _context.TaskTypes.AsNoTracking().ToListAsync();

                var subjects = await _context.Subjects.AsNoTracking().ToListAsync();

                var groups = await _context.Groups
                    .Include(g => g.Students)
                    .Where(g => g.Faculty.Id == teacherEntity.Department.FacultyId)
                    .AsNoTracking()
                    .ToListAsync();

                var typeList = new List<TypeOfTaskDTO>();
                var subjectList = new List<SubjectDTO>();
                var groupList = new List<GroupDTO>();

                foreach (var t in types)
                {
                    typeList.Add(TypeOfTaskDTO.Map(t));
                }
                foreach (var s in subjects)
                {
                    subjectList.Add(SubjectDTO.Map(s));
                }
                foreach (var g in groups)
                {
                    groupList.Add(GroupDTO.Map(g));
                }
                detail.Data = new TaskFiltersModelDTO
                {
                    Subjects = subjectList,
                    Groups = groupList,
                    Types = typeList
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

        // создание сущности задачи и добавление в БД
        public async Task<OperationDetailDTO<string>> CreateNewTaskAsync(TaskCreateModelDTO dto = null)
        {
            var detail = new OperationDetailDTO<string>();
            try
            {
                var currentUserEntity = await GetUserFromClaimsAsync();

                var teacherEntity = await _context.Teachers
                    .Include(t => t.User)
                    .Where(t => t.UserId == currentUserEntity.Id)
                    .FirstOrDefaultAsync();

                if (dto != null)
                {
                    var newTask = TaskCreateModelDTO.Map(dto);
                    newTask.TeacherId = teacherEntity.Id;

                    await AddStudentsToTaskAsync(newTask, dto.StudentIds);
                    await _context.TaskModels.AddAsync(newTask);
                    await _context.SaveChangesAsync();

                    var createdTask = await _context.TaskModels.FirstOrDefaultAsync(t => t == newTask);

                    if(createdTask != null)
                    {
                        detail.Data = createdTask.Id.ToString();
                        detail.Succeeded = true;
                    }
                    else
                    {
                        detail.ErrorMessages.Add(_serverErrorMessage + "При создании задачи что-то пошло не так.");
                    }
                    return detail;
                }
                else
                {
                    detail.ErrorMessages.Add("Параметр модели создаваемой задачи был равен NULL.");
                    return detail;
                }

            }
            catch (Exception e)
            {
                detail.ErrorMessages.Add(e.Message);
                return detail;
            }
        }

                
        // test
        // сразу формировать DTO
        public async Task<OperationDetailDTO<List<SubjectDTO>>> GetMainDataAsync()
        {
            var detail = new OperationDetailDTO<List<SubjectDTO>>();
            var resSubjectDTOList = new List<SubjectDTO>();

            try
            {
                var currentUserEntity = await GetUserFromClaimsAsync();

                var currentTeacher = await _context.Teachers
                    .Include(t => t.User)
                    .Include(t => t.Department)
                    .ThenInclude(d => d.Faculty)
                    .Where(t => t.UserId == currentUserEntity.Id)
                    .FirstOrDefaultAsync();

                // все задачи текущего преподавателя
                IQueryable<TaskModel> teacherTaskQueryList = from t in _context.TaskModels
                                     .Include(t => t.Teacher)
                                     .Include(t => t.Group)
                                     .ThenInclude(g => g.Tasks)
                                     .Include(t => t.Group)
                                     .ThenInclude(g => g.Students) 
                                     .Include(t => t.Subject)
                                     .Include(t => t.Type)
                                     .Include(t => t.Solutions)
                                     .Include(t => t.TaskStudentItems)
                                     .Where(t => t.TeacherId == currentTeacher.Id)
                                                             select t;

                // все предметы, по которым есть задачи
                IQueryable<Subject> subjectQueryList_ = from s in _context.Subjects
                                                       .Include(s => s.Tasks)
                                                       .Where(s => s.Tasks.Count > 0)
                                                        select s;


                // формируем список сущностей предметов
                var resSubjectEntityList = new List<Subject>();

                var resGroupEntityList = new List<Group>();

                // из всех задач препода получить список предметов по которым у препода есть задачи
                foreach (var task in teacherTaskQueryList)
                {
                    var currentSubjectDTO = new SubjectDTO();
                    var currentGroupDTO = GroupDTO.Map(task.Group);
                    var newStudentDTO = new StudentDTO();

                    if ((currentSubjectDTO = resSubjectDTOList.FirstOrDefault(s => s.Id == task.SubjectId)) != null)
                    {
                        if ((currentGroupDTO = currentSubjectDTO.Groups.FirstOrDefault(g => g.Id == currentGroupDTO.Id)) != null)
                        {
                            foreach (var student in currentGroupDTO.Students)
                            {
                                // get every student solution for current task
                                var ts = new TaskStudentItem();
                                var solution = new Solution();
                                if ((ts = task.TaskStudentItems.FirstOrDefault(ts => (ts.StudentId == student.Id) && (ts.TaskId == task.Id))) != null)
                                {
                                    var taskDTO = TaskDTO.Map(task);
                                    var solEnt = task.Solutions.Where(s => s.StudentId == student.Id).FirstOrDefault();
                                    //var solEnt = await _context.Solutions
                                    //    .Where(s => s.StudentId == student.Id)
                                    //    .Where(s => s.TaskId == task.Id)
                                    //    .FirstOrDefaultAsync();
                                    //if((solution = task.Solutions.FirstOrDefault(s => s.StudentId == student.Id)) != null)
                                    //{
                                    //    taskDTO.Solution = SolutionDTO.Map(solution);
                                    //}
                                    if (solEnt != null)
                                    {
                                        taskDTO.Solution = SolutionDTO.Map(solEnt);
                                    }
                                    student.Tasks.Add(taskDTO);
                                    //if(solution != null)
                                    //{
                                        //student.Tasks.
                                        //student.Solution = SolutionDTO.Map(solution);
                                        //student.Solutions.Add(SolutionDTO.Map(solution));
                                        //student.Solution = student.Solutions.FirstOrDefault(s => s.TaskId == task.Id);
                                    //}
                                }
                            }
                        }
                        else
                        {
                            currentSubjectDTO.Groups.Add(currentGroupDTO);
                            foreach (var student in currentGroupDTO.Students)
                            {
                                var ts = new TaskStudentItem();
                                var solution = new Solution();
                                if ((ts = task.TaskStudentItems.FirstOrDefault(ts => (ts.StudentId == student.Id) && (ts.TaskId == task.Id))) != null)
                                {
                                    var taskDTO = TaskDTO.Map(task);
                                    var solEnt = task.Solutions.Where(s => s.StudentId == student.Id).FirstOrDefault();
                                    //var solEnt = await _context.Solutions
                                    //    .Where(s => s.StudentId == student.Id)
                                    //    .Where(s => s.TaskId == task.Id)
                                    //    .FirstOrDefaultAsync();
                                    //if((solution = task.Solutions.FirstOrDefault(s => s.StudentId == student.Id)) != null)
                                    //{
                                    //    taskDTO.Solution = SolutionDTO.Map(solution);
                                    //}
                                    if (solEnt != null)
                                    {
                                        taskDTO.Solution = SolutionDTO.Map(solEnt);
                                    }
                                    student.Tasks.Add(taskDTO);
                                    //student.Solution = student.Solutions.FirstOrDefault(s => s.TaskId == task.Id);
                                }
                            }
                        }
                    }

                    else
                    {
                        currentSubjectDTO = SubjectDTO.Map(task.Subject);
                        // наполняем студентов группы заданиями и решениями 
                        foreach (var student in currentGroupDTO.Students)
                        {
                            var ts = new TaskStudentItem();
                            if ((ts = task.TaskStudentItems.FirstOrDefault(ts => (ts.StudentId == student.Id) && (ts.TaskId == task.Id))) != null)
                            {
                                var taskDTO = TaskDTO.Map(task);
                                var solEnt = task.Solutions.Where(s => s.StudentId == student.Id).FirstOrDefault();
                                if (solEnt != null)
                                {
                                    taskDTO.Solution = SolutionDTO.Map(solEnt);
                                }
                                student.Tasks.Add(taskDTO);
                                //student.Solution = student.Solutions.FirstOrDefault(s => s.TaskId == task.Id);
                            }
                        }
                        currentSubjectDTO.Groups.Add(currentGroupDTO);
                        resSubjectDTOList.Add(currentSubjectDTO);
                    }
                }

                detail.Succeeded = true;
                detail.Data = resSubjectDTOList;

                return new OperationDetailDTO<List<SubjectDTO>> { Data = resSubjectDTOList, Succeeded = true };
            }
            catch (Exception e)
            {
                detail.Succeeded = false;
                detail.ErrorMessages.Add(_serverErrorMessage + e.Message);
                return detail;
            }
        }

        // done
        // получение дерева объектов для фильтрации заданий
        public async Task<OperationDetailDTO<TaskFiltersModelDTO>> GetTaskFiltersAsync()
        {
            var detail = new OperationDetailDTO<TaskFiltersModelDTO>();
            try
            {
                var resSubjectDTOList = new List<SubjectDTO>();
                var resTypeDTOList = new List<TypeOfTaskDTO>();

                var currentUserEntity = await GetUserFromClaimsAsync();

                var teacherUser = await _context.Users
                    .Where(u => u.Id == currentUserEntity.Id)
                    .FirstOrDefaultAsync();

                var teacherEntity = await _context.Teachers
                    .Include(t => t.User)
                    .Include(t => t.Department)
                    .ThenInclude(d => d.Faculty)
                    .Where(t => t.UserId == currentUserEntity.Id)
                    .FirstOrDefaultAsync();

                IQueryable<Subject> subjects = from s in _context.Subjects select s;

                // error
                IQueryable<TaskModel> teacherTaskQueryList = from t in _context.TaskModels
                                     .Include(t => t.Teacher)
                                     .Include(t => t.Group)
                                     .Include(t => t.Subject)
                                     .Include(t => t.Type)
                                     .Where(t => t.TeacherId == teacherEntity.Id)
                                                             select t;

                foreach (var task in teacherTaskQueryList)
                {
                    var groupDTO = GroupDTO.Map(task.Group);
                    var subjectDTO = new SubjectDTO();
                    var typeDTO = new TypeOfTaskDTO();

                    if ((subjectDTO = resSubjectDTOList.FirstOrDefault(s => s.Id == task.SubjectId)) != null)
                    {
                        // куда добавляется группа
                        if (subjectDTO.Groups.FirstOrDefault(g => g.Id == groupDTO.Id) != null) // добавить проверку по id
                        { }
                        else
                        {
                            subjectDTO.Groups.Add(groupDTO);
                        }
                    }
                    else
                    {
                        subjectDTO = SubjectDTO.Map(task.Subject);
                        subjectDTO.Groups.Add(groupDTO);
                        resSubjectDTOList.Add(subjectDTO);
                    }

                    if ((typeDTO = resTypeDTOList.FirstOrDefault(t => t.Id == task.TypeId)) != null)
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

        // получение отфильтрованного списка заданий
        public async Task<OperationDetailDTO<List<TaskDTO>>> GetTasksFromDBAsync(FilterDTO[] filters)
        {
            var detail = new OperationDetailDTO<List<TaskDTO>>();
            var resultList = new List<TaskDTO>();

            try
            {
                var currentUserEntity = await GetUserFromClaimsAsync();

                var teacherUser = await _context.Users
                    .Where(u => u.Id == currentUserEntity.Id)
                    .FirstOrDefaultAsync();

                var teacherEntity = await _context.Teachers
                    .Include(t => t.User)
                    .Where(t => t.UserId == currentUserEntity.Id)
                    .FirstOrDefaultAsync();


                var tasks = from t in _context.TaskModels
                            .Where(t => t.TeacherId == teacherEntity.Id)
                            .Include(t => t.Group)
                            .Include(t => t.Subject)
                            .Include(t => t.Type)
                            .Include(t => t.TaskStudentItems)
                            .Include(t => t.Solutions)
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

                            case "groupId":
                                {
                                    var value = Convert.ToInt32(filter.Value);
                                    if (value > 0)
                                    {
                                        tasks = tasks.Where(t => t.GroupId == value);
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

        // done
        public async Task<OperationDetailDTO<TaskDTO>> GetTaskByIDAsync(int id)
        {
            var detail = new OperationDetailDTO<TaskDTO>();
            var resultTaskDTO = new TaskDTO();
            var resSolutionDTOList = new List<SolutionDTO>();
            var resStudentDTOList = new List<StudentDTO>();
            try
            {
                var entity = await _context.TaskModels
                    .Include(t => t.Group)
                    .Include(t => t.TaskStudentItems)
                    .Include(t => t.Solutions)
                    .Include(t => t.Subject)
                    .Include(t => t.Type)
                    .Include(t => t.File)
                    .Include(t => t.Teacher)
                    .FirstOrDefaultAsync(t => t.Id == id);

                if (entity == null)
                {
                    detail.ErrorMessages.Add("Задача не найдена.");
                    return detail;
                }

                foreach (var ts in entity.TaskStudentItems)
                {
                    var studentEntuty = await _context.Students.FindAsync(ts.StudentId);
                    resStudentDTOList.Add(StudentDTO.Map(studentEntuty));
                }

                foreach (var s in entity.Solutions)
                {
                    resSolutionDTOList.Add(SolutionDTO.Map(s));
                }

                resultTaskDTO = TaskDTO.Map(entity);
                _taskService.GetCurrentTimePercentage(ref resultTaskDTO);
                resultTaskDTO.Solutions = resSolutionDTOList;
                resultTaskDTO.Students = resStudentDTOList;

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

        // done 
        public async Task<OperationDetailDTO> UpdateTaskAsync(TaskCreateModelDTO dto)
        {
            var detail = new OperationDetailDTO<TaskDTO>();
            try
            {
                if(dto == null)
                {
                    detail.ErrorMessages.Add("Объект входного параметра был равен NULL.");
                    return detail;
                }

                var entity = await _context.TaskModels.FindAsync(dto.Id);

                if (entity == null)
                {
                    detail.ErrorMessages.Add("Задача не найдена.");
                    return detail;
                }

                if(dto.BeginDate < dto.FinishDate)
                {
                    entity.ContentText = dto.ContentText;
                    entity.BeginDate = dto.BeginDate;
                    entity.FinishDate = dto.FinishDate;
                    entity.Name = dto.Name;
                    entity.TypeId = dto.TypeId;
                    entity.UpdateDate = DateTime.Now;
                    _context.TaskModels.Update(entity);
                    await _context.SaveChangesAsync();
                    detail.Succeeded = true;
                }
                else
                {
                    detail.ErrorMessages.Add("Дата крайнего срока сдачи должна быть позднее даты начала.");
                }

                
                return detail;
            }
            catch (Exception e)
            {
                detail.ErrorMessages.Add(_serverErrorMessage + e.Message);
                return detail;
            }
        }

        // done
        public async Task<OperationDetailDTO> CloseTaskAsync(int id)
        {
            var detail = new OperationDetailDTO<TaskDTO>();
            try
            {
                var entity = await _context.TaskModels.FindAsync(id); 

                if (entity == null)
                {
                    detail.ErrorMessages.Add("Задача не найдена.");
                    return detail;
                }

                entity.IsOpen = false;
                entity.UpdateDate = DateTime.Now;

                _context.TaskModels.Update(entity);
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

        // TODO: update Repo

        // todo: exceptions; return errors 
        // получение пользователя, сделавшего текущий запрос
        private async Task AddStudentsToTaskAsync(TaskModel task, int[] studentIDs)
        {
            var students = new List<Student>();
            foreach (var studentID in studentIDs)
            {
                var taskStudentItem = new TaskStudentItem();
                var student = await _context.Students.FindAsync(studentID);
                if (student != null)
                {
                    taskStudentItem.Student = student;
                    taskStudentItem.Task = task;
                    task.TaskStudentItems.Add(taskStudentItem);
                }
            }
        }

        // todo: exceptions; return errors [!]
        public async Task<User> GetUserFromClaimsAsync()
        {
            var userNameClaim = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
            string stringID = userNameClaim.Value;
            var user = await _userManager.FindByIdAsync(stringID);
            return user;
        }

        private string GetUserName()
        {
            var userNameClaim = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
            return userNameClaim.Value;
        }


        // Test
        public async Task<OperationDetailDTO<List<SubjectDTO>>> GetRepoCreateSubjectFiltersAsync()
        {
            var detail = new OperationDetailDTO<List<SubjectDTO>>();

            var resultSubjectDTOList = new List<SubjectDTO>();

            var subjects = await _context.Subjects.ToListAsync();

            foreach(var sub in subjects)
            {
                resultSubjectDTOList.Add(SubjectDTO.Map(sub));
            }
             
            detail.Data = resultSubjectDTOList;
            detail.Succeeded = true;
            return detail;
        }

        // test
        // add try - catch
        public async Task<OperationDetailDTO<string>> CreateNewRepositoryAsync(RepositoryCreateModelDTO dto = null)
        {
            var detail = new OperationDetailDTO<string>();

            if(dto != null)
            {
                var currentUserEntity = await GetUserFromClaimsAsync();

                var teacherEntity = await _context.Teachers
                    .Where(t => t.UserId == currentUserEntity.Id)
                    .FirstOrDefaultAsync();

                var subject = await _context.Subjects.FindAsync(dto.SubjectId);

                if(subject != null)
                {
                    var repositoryEntity = RepositoryCreateModelDTO.Map(dto);
                    repositoryEntity.Teacher = teacherEntity;
                    repositoryEntity.Subject = subject;
                    await _context.RepositoryModels.AddAsync(repositoryEntity);
                    await _context.SaveChangesAsync();

                    var createdRepo = await _context.RepositoryModels.FirstOrDefaultAsync(r => r == repositoryEntity); // by id ?
                    if(createdRepo != null)
                    {
                        detail.Data = createdRepo.Id.ToString();
                        detail.Succeeded = true;
                    }
                    else
                    {
                        detail.ErrorMessages.Add("Не удалось получить созданный репозиторий");
                    }
                }
                else
                {
                    detail.ErrorMessages.Add("Параметр учебного предмета создаваемого репозитория был равен NULL");
                }

                return detail;
            }

            else
            {
                detail.ErrorMessages.Add("Входные данные создаваемого репозитория равны NULL");
                return detail;
            }
        }

        public async Task<OperationDetailDTO> UpdateRepositoryAsync(RepositoryCreateModelDTO dto)
        {
            var detail = new OperationDetailDTO();

            if(dto != null)
            {
                var repoEntity = await _context.RepositoryModels.FindAsync(dto.Id);

                if(repoEntity == null)
                {
                    detail.ErrorMessages.Add("Репозиторий не найден."); 
                    return detail;
                }

                repoEntity.ContentText = dto.ContentText;
                repoEntity.Name = dto.Name;

                _context.RepositoryModels.Update(repoEntity);
                await _context.SaveChangesAsync();

                detail.Succeeded = true;
            }
            else
            {
                detail.ErrorMessages.Add("Объекта параметра репозитория был равен null");
            }

            return detail;
        }

        public async Task<OperationDetailDTO> DeleteRepositoryAsync(int id)
        {
            var detail = new OperationDetailDTO();

            var entity = await _context.RepositoryModels.FindAsync(id);

            if (entity != null)
            {
                _context.RepositoryModels.Remove(entity);
                await _context.SaveChangesAsync();
                detail.Succeeded = true;
            }
            else
            {
                detail.ErrorMessages.Add("Репозиторий не найден.");
            }

            return detail;
        }

        // test
        public async Task<OperationDetailDTO<List<SubjectDTO>>> GetRepositoryListFilters()
        {
            var detail = new OperationDetailDTO<List<SubjectDTO>>();

            var resultSubjectDTOList = new List<SubjectDTO>();

            var currentUserEntity = await GetUserFromClaimsAsync();

            var teacherEntity = await _context.Teachers
                .Where(t => t.UserId == currentUserEntity.Id)
                .FirstOrDefaultAsync();

            IQueryable<RepositoryModel> repositories = from r in _context.RepositoryModels
                                      .Include(r => r.Teacher)
                                      .Include(r => r.Subject)
                                      .Where(r => r.TeacherId == teacherEntity.Id)
                                      select r;

            foreach(var repo in repositories)
            {
                SubjectDTO subjectDTO;
                if((subjectDTO = resultSubjectDTOList.FirstOrDefault(s => s.Id == repo.SubjectId)) != null)
                { }
                else
                {
                    subjectDTO = SubjectDTO.Map(repo.Subject);
                    resultSubjectDTOList.Add(subjectDTO);
                }
            }

            detail.Data = resultSubjectDTOList;
            detail.Succeeded = true;
            return detail;
        }
        // test
        public async Task<OperationDetailDTO<List<RepositoryDTO>>> GetRepositoriesFromDBAsync(FilterDTO[] filters = null)
        {
            var detail = new OperationDetailDTO<List<RepositoryDTO>>();

            var resultList = new List<RepositoryDTO>();

            var currentUserEntity = await GetUserFromClaimsAsync();

            var teacherEntity = await _context.Teachers
                .Include(t => t.User)
                .Where(t => t.UserId == currentUserEntity.Id)
                .FirstOrDefaultAsync();


            var repos = from r in _context.RepositoryModels
                        .Where(r => r.TeacherId == teacherEntity.Id)
                        .Include(r => r.Subject)
                        .Include(r => r.Files)
                        select r;

            repos.OrderBy(r => r.Name);

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
            return detail;
        }
        // test
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



        // test
        // add try - catch
        public async Task<OperationDetailDTO<string>> CreateNewThemeAsync(ThemeCreateModelDTO dto)
        {
            var detail = new OperationDetailDTO<string>();

            if (dto != null)
            {
                var repositoryEntity = await _context.RepositoryModels.FindAsync(dto.RepositoryId);

                if (repositoryEntity != null)
                {
                    var themeEntity = ThemeCreateModelDTO.Map(dto);
                    themeEntity.Repository = repositoryEntity;
                    await _context.Themes.AddAsync(themeEntity);
                    await _context.SaveChangesAsync();

                    var createdTheme = await _context.Themes.FindAsync(themeEntity);  // by id ?
                    if (createdTheme != null)
                    {
                        detail.Data = createdTheme.Id.ToString();
                        detail.Succeeded = true;
                    }
                    else
                    {
                        detail.ErrorMessages.Add("Не удалось получить созданную тему");
                    }
                }

                else
                {
                    detail.ErrorMessages.Add("Параметр репозитория создаваемой темы был равен NULL");
                }

                return detail;
            }

            else
            {
                detail.ErrorMessages.Add("Входные данные создаваемой темы равны NULL");
                return detail;
            }
        }

        // test
        // add try - catch
        public async Task<OperationDetailDTO<string>> CreateNewParagraphAsync(ParagraphCreateModelDTO dto)
        {
            var detail = new OperationDetailDTO<string>();

            if (dto != null)
            {
                var themeEntity = await _context.Themes.FindAsync(dto.ThemeId);

                if (themeEntity != null)
                {
                    var paragraphEntity = ParagraphCreateModelDTO.Map(dto);
                    await _context.Paragraphs.AddAsync(paragraphEntity);
                    await _context.SaveChangesAsync();

                    var createdParagraph = await _context.Paragraphs.FindAsync(paragraphEntity);  // by id ?
                    if (createdParagraph != null)
                    {
                        detail.Data = createdParagraph.Id.ToString();
                        detail.Succeeded = true;
                    }
                    else
                    {
                        detail.ErrorMessages.Add("Не удалось получить созданный параграф");
                    }
                }

                else
                {
                    detail.ErrorMessages.Add("Параметр темы создаваемого параграфа был равен NULL");
                }

                return detail;
            }

            else
            {
                detail.ErrorMessages.Add("Входные данные создаваемого параграфа равны NULL");
                return detail;
            }
        }

        
    }
}