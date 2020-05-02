using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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

        public TeacherService(DataContext context, IHttpContextAccessor httpContextAccessor, 
            UserManager<User> userManager, ITaskService taskService)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _taskService = taskService;
        }


        // user.Include(u => u.Teacher) ??
        public async Task<OperationDetailDTO<TeacherDTO>> GetProfileDataAsync()
        {
            var detail = new OperationDetailDTO<TeacherDTO>();
            try
            {
                var currentUserEntity = await GetCurrentUser();
                
                // wtf?? -> check
                var teacher = _context.Users.Include(t => t.Teacher).Where(u => u.Id == currentUserEntity.Id).FirstOrDefault();
                
                var dto = TeacherDTO.Map(teacher.Teacher);

                // если без включения
                var teacherEntity = await _context.Teachers
                    .Where(t => t.UserId == currentUserEntity.Id)
                    .Include(t => t.User)
                    .FirstOrDefaultAsync();

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

        public async Task<OperationDetailDTO> UpdateProfileDataAsync(TeacherDTO newTeacherDTO)
        {
            var detail = new OperationDetailDTO<TeacherDTO>();
            try
            {
                List<string> errorMessages = new List<string>();
                var currentUser = await GetCurrentUser();
                var currentTeacherUser = await _context.Users
                    .Include(u => u.Teacher)
                    .FirstOrDefaultAsync(u => u.Id == currentUser.Id);

                var currentTeacherEntity = await _context.Teachers.FindAsync(currentTeacherUser.Teacher.Id);

                if(!UserValidator.Validate(newTeacherDTO, out errorMessages))
                {
                    detail.Succeeded = false;
                    detail.ErrorMessages = errorMessages;
                    return detail;
                }

                if(currentTeacherUser!= null)
                {
                    currentTeacherUser.Email = newTeacherDTO.Email;
                    currentTeacherUser.UserName = newTeacherDTO.UserName;
                    var userUpdateResult = await _userManager.UpdateAsync(currentTeacherUser);
                    
                    if (currentTeacherEntity != null)
                    {
                        currentTeacherEntity.Name = newTeacherDTO.Name;
                        currentTeacherEntity.Surname = newTeacherDTO.Surname;
                        currentTeacherEntity.Patronymic = newTeacherDTO.Patronymic;
                        currentTeacherEntity.Position = newTeacherDTO.Position;

                        _context.Teachers.Update(currentTeacherEntity);
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

        // todo: adding file [?]
        public async Task<OperationDetailDTO<TaskDTO>> CreateNewTaskAsync(TaskCreateModelDTO dto = null)
        {
            var detail = new OperationDetailDTO<TaskDTO>();
            try
            {
                if (dto != null)
                {
                    var newTask = TaskCreateModelDTO.Map(dto);

                    await _context.TaskModels.AddAsync(newTask);
                    await _context.SaveChangesAsync();
                    await AddStudentsToTaskAsync(newTask.Id, dto.StudentIds);
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

        public async Task<OperationDetailDTO<List<SubjectDTO>>> GetMainDataAsync()
        {
            var detail = new OperationDetailDTO<List<SubjectDTO>>();
            var resSubjectDTOList = new List<SubjectDTO>();

            try
            {
                var currentUser = await GetCurrentUser();
                var currentTeacher = currentUser.Teacher;

                // все задачи текущего преподавателя
                IQueryable<TaskModel> teacherTaskQueryList = from t in _context.TaskModels
                                     .Include(t => t.TeacherId == currentTeacher.Id)
                                     .Include(t => t.Group)
                                     .ThenInclude(g => g.Tasks)
                                     .Include(t => t.Group)
                                     .ThenInclude(g => g.Students) // ?
                                     .Include(t => t.Subject)
                                     .ThenInclude(s => s.Tasks) // также задания от других учитетелей
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
                        if (!exSubject.Groups.Contains(task.Group))
                        {
                            exSubject.Groups.Add(task.Group);
                        }
                    }
                    // если нет - добавляем предмет и к нему группу
                    else
                    {
                        var newSubject = new Subject();
                        newSubject = task.Subject;
                        newSubject.Groups.Add(task.Group);
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

                // наполнение групп
                // получение задач
                foreach (var subject in resSubjectEntityList)
                {
                    foreach (var group in subject.Groups) // студенты подгружены
                    {
                        foreach (var student in group.Students)
                        {
                            var tSItem = await _context.TaskStudentItems
                                .Include(ts => ts.Task)
                                .ThenInclude(t => t.Solutions)
                                .Where(ts => ts.StudentId == student.Id)
                                .Where(ts => ts.Task.TeacherId == currentTeacher.Id)
                                .ToListAsync();

                            var tasks = await _context.TaskModels
                            .Include(t => t.Group)
                            .Include(t => t.Subject)
                            .Include(t => t.TaskStudentItems == tSItem) // сравнение списков связующих сущностей
                            .Where(t => t.Subject.Id == subject.Id)
                            .Where(t => t.Group.Id == group.Id)
                            .ToListAsync();

                        }
                    }
                }

                foreach (var subject in resSubjectEntityList)
                {
                    var subDTO = SubjectDTO.Map(subject);
                    subDTO.Groups = GroupDTO.Map(subject.Groups);
                    resSubjectDTOList.Add(SubjectDTO.Map(subject));
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

        public async Task<OperationDetailDTO<List<SubjectDTO>>> GetTaskFiltersAsync()
        {
            var detail = new OperationDetailDTO<List<SubjectDTO>>();

            var resSubjectDTOList = new List<SubjectDTO>();
            var resSubjectEntityList = new List<Subject>();

            var currentUser = await GetCurrentUser();
            var currentTeacher = currentUser.Teacher;

            IQueryable<Subject> subjects = from s in _context.Subjects select s;

            // все задачи текущего преподавателя
            IQueryable<TaskModel> teacherTaskQueryList = from t in _context.TaskModels
                                 .Include(t => t.TeacherId == currentTeacher.Id)
                                 .Include(t => t.Group)
                                 .ThenInclude(g => g.Tasks)
                                 .Include(t => t.Group)
                                 .ThenInclude(g => g.Students) // ?
                                 .Include(t => t.Subject)
                                 .ThenInclude(s => s.Tasks) // также задания от других учитетелей
                                 .Include(t => t.TaskStudentItems)
                                 select t;

            var currentGroupEntityList = new List<Group>();

            // из всех задач препода получить список предметов по которым у препода есть задачи
            foreach (var task in teacherTaskQueryList)
            {
                // добавляем группу в список для всех групп с задачами текущего препода
                currentGroupEntityList.Add(task.Group);

                // если такой предмет уже добавлен в список предметов препода, добавляем ему группу этой задачи
                var exSubject = new Subject();
                if ((exSubject = resSubjectEntityList.FirstOrDefault(s => s == task.Subject)) != null)
                {
                    if (!exSubject.Groups.Contains(task.Group))
                    {
                        exSubject.Groups.Add(task.Group);
                    }
                }
                // если нет - добавляем предмет и к нему группу
                else
                {
                    var newSubject = new Subject();
                    newSubject = task.Subject;
                    newSubject.Groups.Add(task.Group);
                    resSubjectEntityList.Add(task.Subject);
                }
            }

            foreach(var subject in resSubjectEntityList)
            {
                resSubjectDTOList.Add(SubjectDTO.Map(subject));
            }

            detail.Succeeded = true;
            detail.Data = resSubjectDTOList;

            return detail;
            

            // получение задач
            //foreach (var subject in resSubjectEntityList )
            //{
            //    foreach(var group in subject.Groups)
            //    {
            //        var tasks = await _context.TaskModels
            //            .Include(t => t.Group)
            //            .Include(t => t.Subject)
            //            .Include(t => t.TaskStudentItems)
            //            .Where(t => t.Subject.Id == subject.Id)
            //            .Where(t => t.Group.Id == group.Id)
            //            .ToListAsync();
            //    }
            //}

        }

        public async Task<OperationDetailDTO<List<TaskDTO>>> GetTasksFromDBAsync(FilterDTO[] filters)
        {
            var detail = new OperationDetailDTO<List<TaskDTO>>();
            var resultList = new List<TaskDTO>();

            try
            {
                var currentTeacher = await GetCurrentTeacherEntityAsync();

                var tasks = from t in _context.TaskModels
                            .Where(t => t.TeacherId == currentTeacher.Id)
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


        private async Task<Teacher> GetCurrentTeacherEntityAsync()
        {
            var currentUser = await GetCurrentUser();
            return currentUser.Teacher;
        }

        private async Task<User> GetCurrentUser() => await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

        private async Task AddStudentsToTaskAsync(int taskID, int[] studentIDs)
        {
            var task = await _context.TaskModels.FindAsync(taskID);
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

        public Task<OperationDetailDTO<List<SubjectDTO>>> GetAddingTaskFiltersAsync()
        {
            var detail = new OperationDetailDTO();
            try
            {
                var types = _context.TaskTypes.AsNoTracking().ToListAsync();
                var subjects = _context.Subjects.AsNoTracking().ToListAsync();
                var groups = _context.Groups
                    .Include(g => g.Students)
                    .ToListAsync();

            }
            catch (Exception e)
            {

            }
            throw new NotImplementedException();
        }
    }
}




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