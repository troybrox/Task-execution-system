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
    public class TeacherService : ITeacherService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly ITaskService _taskService;
        private readonly SignInManager<User> _signInManager;

       public TeacherService(DataContext context, IHttpContextAccessor httpContextAccessor, 
            UserManager<User> userManager, ITaskService taskService, SignInManager<User> signInManager)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _taskService = taskService;
            _signInManager = signInManager;
        }

        // получение данных профиля преподавателя
        public async Task<OperationDetailDTO<TeacherDTO>> GetProfileDataAsync()
        {
            var detail = new OperationDetailDTO<TeacherDTO>();
            try
            {
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
            return detail;
        }

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

                if(await _userManager.Users.Where(u => u.UserName == newTeacherDTO.UserName).Where(u => u.Id != teacherUser.Id).FirstAsync() != null)
                {
                    detail.Succeeded = false;
                    detail.ErrorMessages.Add("Пользователь с таким именем пользователя уже существует, подберите другое.");
                    return detail;
                }

                if(teacherUser != null)
                {
                    teacherUser.Email = newTeacherDTO.Email;
                    teacherUser.UserName = newTeacherDTO.UserName;
                    var userUpdateResult = await _userManager.UpdateAsync(teacherUser);
                    
                    if (teacherEntity != null)
                    {
                        teacherEntity.Position = newTeacherDTO.Position;

                        _context.Teachers.Update(teacherEntity);
                        await _context.SaveChangesAsync();

                        detail.Succeeded = true;
                    }
                }
                return detail;
            }
            catch (Exception e)
            {
                detail.ErrorMessages.Add(_serverErrorMessage + e.Message);
                return detail;
            }
        }

        // получение списков-фильтров для создания задачи
        // форируются списки существующих предметов, типов, групп для отправки на клиент
        public async Task<OperationDetailDTO<TaskAddingFiltersModelDTO>> GetAddingTaskFiltersAsync()
        {
            var detail = new OperationDetailDTO<TaskAddingFiltersModelDTO>();

            try
            {
                var types = await _context.TaskTypes.AsNoTracking().ToListAsync();
                var subjects = await _context.Subjects.AsNoTracking().ToListAsync();
                var groups = await _context.Groups
                    .Include(g => g.Students)
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
                detail.Data = new TaskAddingFiltersModelDTO
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

        // создаине сущности задачи и добавление в БД
        public async Task<OperationDetailDTO<TaskDTO>> CreateNewTaskAsync(TaskCreateModelDTO dto = null)
        {
            var detail = new OperationDetailDTO<TaskDTO>();
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

                if (dto != null)
                {
                    var newTask = TaskCreateModelDTO.Map(dto);
                    newTask.TeacherId = teacherEntity.Id;

                    await AddStudentsToTaskAsync(newTask, dto.StudentIds);
                    await _context.TaskModels.AddAsync(newTask);
                    await _context.SaveChangesAsync();

                    var createdTask = await _context.TaskModels.FirstOrDefaultAsync(t => t == newTask);
                    var taskDTO = TaskDTO.Map(createdTask);

                    detail.Succeeded = true;
                    detail.Data = taskDTO;
                    return detail;
                }
                else
                {
                    detail.ErrorMessages.Add("Параметр модели создаваемой задачи был равен NULL");
                    return detail;
                }

            }
            catch (Exception e)
            {
                detail.ErrorMessages.Add(e.Message);
                return detail;
            }
        }

        // метод в стадии fixing [GROUPS error ---]
        public async Task<OperationDetailDTO<List<SubjectDTO>>> GetMainDataAsync()
        {
            var detail = new OperationDetailDTO<List<SubjectDTO>>();
            var resSubjectDTOList = new List<SubjectDTO>();

            var currentUser = await GetUserFromClaimsAsync();
            var currentTeacher = currentUser.Teacher;


            // все задачи текущего преподавателя
            IQueryable<TaskModel> teacherTaskQueryList = from t in _context.TaskModels
                                 .Include(t => t.TeacherId == currentTeacher.Id)
                                 .Include(t => t.Group)
                                 .ThenInclude(g => g.Tasks)
                                 .Include(t => t.Group)
                                 .ThenInclude(g => g.Students) // [?]
                                 .Include(t => t.Subject)
                                 .ThenInclude(s => s.Tasks) // также задания от других учитетелей [-]
                                 .Include(t => t.TaskStudentItems)
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
                // добавляем группу в список для всех групп с задачами текущего препода
                resGroupEntityList.Add(task.Group);

                // если такой предмет уже добавлен в список предметов препода, добавляем ему группу этой задачи
                var exSubject = new Subject();
                if ((exSubject = resSubjectEntityList.FirstOrDefault(s => s == task.Subject)) != null)
                {
                    // GROUPS error ---
                    //if (!exSubject.Groups.Contains(task.Group))
                    //{
                    //    exSubject.Groups.Add(task.Group);
                    //}
                }

                // если нет - добавляем предмет и к нему группу
                else
                {
                    var newSubject = new Subject();
                    newSubject = task.Subject;
                    // GROUPS error ---
                    //newSubject.Groups.Add(task.Group);
                    resSubjectEntityList.Add(task.Subject);
                }


                //foreach(var sub in resSubjectEntityList)
                //{
                //    if (sub.Name == task.Subject.Name) { }

                //}

                //// у одной задачи - ОДНА ГРУППА
                //var groupsWithThisTask = (from g in _context.Groups
                //                  .Include(g => g.Tasks)
                //                  .Where(g => g.Tasks.Contains(task))
                //                  .Include(g => g.Students)
                //                       select g).ToList();

                //// найти группу этой задачи

                //task.Subject.Groups.AddRange(groupsWithThisTask);
                //resSubjectEntityList.Add(task.Subject);

                //// добавить студентов
                //foreach (var group in groupsWithThisTask)
                //{ }
            }

            foreach (var subject in resSubjectEntityList)
            {
                // GROUPS error ---
                //foreach (var group in subject.Groups) // студенты подгружены
                //{
                //    foreach (var student in group.Students)
                //    {
                //        var tSItem = await _context.TaskStudentItems
                //            .Include(ts => ts.Task)
                //            .ThenInclude(t => t.Solutions)
                //            .Where(ts => ts.StudentId == student.Id)
                //            .Where(ts => ts.Task.TeacherId == currentTeacher.Id)
                //            .ToListAsync();

                //        var tasks = await _context.TaskModels
                //        .Include(t => t.Group)
                //        .Include(t => t.Subject)
                //        .Include(t => t.TaskStudentItems == tSItem) // сравнение списков связующих сущностей
                //        .Where(t => t.Subject.Id == subject.Id)
                //        .Where(t => t.Group.Id == group.Id)
                //        .ToListAsync();

                //    }
                //}
            }

            foreach (var subject in resSubjectEntityList)
            {
                var subDTO = SubjectDTO.Map(subject);
                // GROUPS error ---
                //subDTO.Groups = GroupDTO.Map(subject.Groups);
                resSubjectDTOList.Add(SubjectDTO.Map(subject));
            }

            detail.Succeeded = true;
            detail.Data = resSubjectDTOList;

            return new OperationDetailDTO<List<SubjectDTO>> { Data = resSubjectDTOList, Succeeded = true };

            //try
            //{

            //}
            //catch (Exception e)
            //{
            //    detail.Succeeded = false;
            //    detail.ErrorMessages.Add(_serverErrorMessage + e.Message);
            //    return detail;
            //}
        }

        // получение дерева объектов для фильтрации заданий
        public async Task<OperationDetailDTO<List<SubjectDTO>>> GetTaskFiltersAsync()
        {
            var detail = new OperationDetailDTO<List<SubjectDTO>>();
            try
            {
                var resSubjectDTOList = new List<SubjectDTO>();

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

                IQueryable<TaskModel> teacherTaskQueryList = from t in _context.TaskModels
                                     .Include(t => t.TeacherId == teacherEntity.Id)
                                     .Include(t => t.Group)
                                     .Include(t => t.Subject)
                                                             select t;

                var currentGroupEntityList = new List<Group>();

                foreach (var task in teacherTaskQueryList)
                {
                    currentGroupEntityList.Add(task.Group);

                    var subjectDTO = new SubjectDTO();
                    if ((subjectDTO = resSubjectDTOList.FirstOrDefault(s => s.Id == task.SubjectId)) != null)
                    {
                        var groupDTO = GroupDTO.Map(task.Group);
                        if (!subjectDTO.Groups.Contains(groupDTO))
                        {
                            subjectDTO.Groups.Add(groupDTO);
                        }
                    }
                    else
                    {
                        var newSubjectDTO = SubjectDTO.Map(task.Subject);
                        resSubjectDTOList.Add(newSubjectDTO);
                    }
                }
                detail.Data = resSubjectDTOList;
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
                            //.Include(t => t.Group)
                            //.Include(t => t.Subject)
                            //.Include(t => t.TaskStudentItems)
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

        public async Task<OperationDetailDTO<TaskDTO>> GetTaskByIDAsync(int id)
        {
            var detail = new OperationDetailDTO<TaskDTO>();
            var resultTaskDTO = new TaskDTO();
            var resSolutionDTOList = new List<SolutionDTO>();
            var resStudentDTOList = new List<StudentDTO>();
            try
            {
                var entity = await _context.TaskModels
                    .Include(t => t.TaskStudentItems)
                    .Include(t => t.Solutions)
                    .Include(t => t.Subject)
                    .Include(t => t.Type)
                    .Include(t => t.File)
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


        public Task<OperationDetailDTO> CreateNewRepositoryAsync()
        {
            throw new NotImplementedException();
        }

        public Task<OperationDetailDTO> CreateNewThemeAsync()
        {
            throw new NotImplementedException();
        }

        public Task<OperationDetailDTO> CreateNewParagraphAsync()
        {
            throw new NotImplementedException();
        }

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
    }
}





// GROUPS error ---
//public async Task<OperationDetailDTO<List<SubjectDTO>>> GetTaskFiltersAsync__error()
//{
//    var detail = new OperationDetailDTO<List<SubjectDTO>>();

//    var resSubjectDTOList = new List<SubjectDTO>();
//    var resSubjectEntityList = new List<Subject>();

//    var currentUser = await GetCurrentUser();
//    var currentTeacher = currentUser.Teacher;

//    IQueryable<Subject> subjects = from s in _context.Subjects select s;

//    // все задачи текущего преподавателя
//    IQueryable<TaskModel> teacherTaskQueryList = from t in _context.TaskModels
//                         .Include(t => t.TeacherId == currentTeacher.Id)
//                         .Include(t => t.Group)
//                         .ThenInclude(g => g.Tasks)
//                         .Include(t => t.Group)
//                         .ThenInclude(g => g.Students) // ?
//                         .Include(t => t.Subject)
//                         .ThenInclude(s => s.Tasks) // также задания от других учитетелей
//                         .Include(t => t.TaskStudentItems)
//                                                 select t;

//    var currentGroupEntityList = new List<Group>();

//    // из всех задач препода получить список предметов по которым у препода есть задачи
//    foreach (var task in teacherTaskQueryList)
//    {
//        // добавляем группу в список для всех групп с задачами текущего препода
//        currentGroupEntityList.Add(task.Group);

//        // если такой предмет уже добавлен в список предметов препода, добавляем ему группу этой задачи
//        var exSubject = new Subject();
//        if ((exSubject = resSubjectEntityList.FirstOrDefault(s => s == task.Subject)) != null)
//        {
//            // GROUPS error ---
//            //if (!exSubject.Groups.Contains(task.Group))
//            //{
//            //    exSubject.Groups.Add(task.Group);
//            //}
//        }
//        // если нет - добавляем предмет и к нему группу
//        else
//        {
//            var newSubject = new Subject();
//            newSubject = task.Subject;
//            // GROUPS error ---
//            //newSubject.Groups.Add(task.Group);
//            resSubjectEntityList.Add(task.Subject);
//        }
//    }

//    foreach (var subject in resSubjectEntityList)
//    {
//        resSubjectDTOList.Add(SubjectDTO.Map(subject));
//    }

//    detail.Succeeded = true;
//    detail.Data = resSubjectDTOList;

//    return detail;


//    // получение задач
//    //foreach (var subject in resSubjectEntityList )
//    //{
//    //    foreach(var group in subject.Groups)
//    //    {
//    //        var tasks = await _context.TaskModels
//    //            .Include(t => t.Group)
//    //            .Include(t => t.Subject)
//    //            .Include(t => t.TaskStudentItems)
//    //            .Where(t => t.Subject.Id == subject.Id)
//    //            .Where(t => t.Group.Id == group.Id)
//    //            .ToListAsync();
//    //    }
//    //}

//}
// -----


//// наполнение групп
//            // получение задач
//            foreach (var subject in resSubjectEntityList)
//            {
//                foreach (var group in subject.Groups) // студенты подгружены
//                {
//                    foreach (var student in group.Students)
//                    {
//                        var tSItem = await _context.TaskStudentItems
//                            .Include(ts => ts.Task)
//                            .ThenInclude(t => t.Solutions)
//                            .Where(ts => ts.StudentId == student.Id)
//                            .Where(ts => ts.Task.TeacherId == currentTeacher.Id)
//                            .ToListAsync();

//var tasks = await _context.TaskModels
//.Include(t => t.Group)
//.Include(t => t.Subject)
//.Include(t => t.TaskStudentItems == tSItem) // сравнение списков связующих сущностей
//.Where(t => t.Subject.Id == subject.Id)
//.Where(t => t.Group.Id == group.Id)
//.ToListAsync();

//                    }



//                    //var students = await _context.Students
//                    //    .Include(s => s.TaskStudentItems)
//                    //    .Include(s => s.Solutions)
//                    //    .Where()


//                }
//            }


// GET MAIN 
// поиск групп для каждого предмета  из списка => просеить группы
//foreach (var subject in resSubjectEntityList.Distinct())
//{
//    subject.Groups.Distinct();
//}


//------------------
// поиск через анализ по всем предметам, высокая сложность алгоритма
//foreach(var subject in subjectQueryList_)
//{
//    SubjectDTO dto = new SubjectDTO();

//    foreach(var task in subject.Tasks)
//    {
//        //if(!dto.Groups.Contains())
//        if(task.TeacherId == currentTeacher.Id)
//        {
//            var groupDto = new GroupDTO();
//            groupDto = GroupDTO.Map(task.Group);

//            foreach(var tsItem in task.TaskStudentItems)
//            {



//                //var students = await _context.Students
//                //    .Where(s => s.TaskStudentItems.Contains(tsItem))
//                //    .ToListAsync();


//                //foreach(var st in students)
//                //{
//                //    var stDTO = StudentDTO.Map(st);
//                //    groupDto.Students.Add(stDTO);
//                //    groupDto.Students.Distinct();
//                //}
//            }

//            dto.Groups.Add(GroupDTO.Map(task.Group)); // + solution
//            dto.Groups.Distinct();
//        }

//    }
//    dto.Groups.Distinct();
//}

//if (resSubjectEntityList.Contains(task.Subject))
//{
//    var existSub = resSubjectEntityList.Find(s => s == task.Subject);
//    if (!existSub.Groups.Contains(task.Group))
//    {
//        existSub.Groups.Add(task.Group);
//    }
//}

//----
//IQueryable<Subject> subjectQueryList = from s in _context.Subjects
//                                       .Include(s => s.Tasks)
//                                       .Where(s => s.Tasks.Count > 0)
//                                       select s;


//IQueryable<Group> groupsQueryList;
//IQueryable<Subject> subjectQueryList;


//foreach (var task in taskQueryList)
//{
//    subjectQueryList = from s in _context.Subjects
//                                       .Include(s => s.Tasks)
//                                       .Where(s => s.Tasks.Count > 0)
//                       select s;

//    //groupsQueryList = from g in _context.Groups
//    //                  .Include(g => g.Tasks)
//    //                  .Where(g => g.Tasks.Contains(task))
//    //                  select g;

//    foreach (var subject in subjectQueryList) 
//    {

//        groupsQueryList = from g in _context.Groups
//                          .Include(g => g.Tasks)
//                          .Where(g => g.Tasks.Contains(task))
//                          select g;


//        if (subject.Tasks.Contains(task))
//        {
//            resSubjectDTOList.Add(SubjectDTO.Map(subject));
//        }
//    }
//}


//foreach (var subject in subjectQueryList)
//{
//    taskQueryList = from t in _context.TaskModels
//                     .Include(t => t.Group)
//}