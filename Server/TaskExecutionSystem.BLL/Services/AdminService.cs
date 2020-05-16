using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskExecutionSystem.BLL.DTO;
using TaskExecutionSystem.BLL.DTO.Auth;
using TaskExecutionSystem.BLL.Interfaces;
using TaskExecutionSystem.BLL.Validation;
using TaskExecutionSystem.DAL.Data;
using TaskExecutionSystem.DAL.Entities;
using TaskExecutionSystem.DAL.Entities.Identity;
using TaskExecutionSystem.DAL.Entities.Registration;
using TaskExecutionSystem.BLL.DTO.Studies;
using TaskExecutionSystem.BLL.DTO.Filters;
using TaskExecutionSystem.DAL.Entities.Studies;

namespace TaskExecutionSystem.BLL.Services
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<User> _userManager;
        private readonly DataContext _context;

        private const string _serverErrorMessage = "Произошло исключение на сервере. Подробнее: \n";
        private const string _filtersErrorHeader = "Ошибка при получении списков для фильтрации по учебным подразделениям. ";
        private const string _entityGettingError = "Ошибка при получении списков пользователей.";
        private const string _entityDeletingError = "Ошибка при удалении пользователей. ";

        public AdminService(UserManager<User> userManager, DataContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        
        public async Task<OperationDetailDTO<List<FacultyDTO>>> GetAllStudyFiletrsAsync()
        {
            var resFacultyDTOList = new List<FacultyDTO>();
            try
            {
                var entityList = await _context.Faculties
                    .Include(f => f.Groups)
                    .Include(f => f.Departments)
                    .AsNoTracking()
                    .ToListAsync();
                foreach (var entity in entityList)
                {
                    resFacultyDTOList.Add(FacultyDTO.Map(entity));
                }
                return new OperationDetailDTO<List<FacultyDTO>> { Succeeded = true, Data = resFacultyDTOList, ErrorMessages = null };
            }
            catch (Exception e)
            {
                return new OperationDetailDTO<List<FacultyDTO>> { Succeeded = false, ErrorMessages = { _filtersErrorHeader + _serverErrorMessage + e.Message } };
            }
        }

        // списки-фильтры по типу сущностей
        // для заявок только те подразделения факультета, где есть заявки 
        // для существующих факультеты со всех их подразделениями показываются только если в факультете есть подраздения, количество людей не учитывается
        public async Task<OperationDetailDTO<List<FacultyDTO>>> GetAllStudyFiltersAsync(string userType)
        {
            var resFacultyDTOList = new List<FacultyDTO>();
            var resFacEntityList = new List<Faculty>();
            try
            {
                IQueryable<Faculty> faculties = from f in _context.Faculties select f;

                faculties = faculties.OrderBy(f => f.Name);

                switch (userType)
                {
                    case "reg_students":
                        {
                            IQueryable<Group> groups;
                            foreach (var fac in faculties)
                            {
                                groups = from g in _context.Groups
                                                       .Include(g => g.StudentRegisterRequests)
                                                       .Where(g => g.StudentRegisterRequests.Count > 0)
                                                       .Where(g => g.FacultyId == fac.Id)
                                         select g;

                                var testGroupList = groups.ToList();

                                foreach (var group in groups) { }
                                if (fac.Groups.Count > 0)
                                    resFacEntityList.Add(fac);
                            }
                            break;
                        }

                    case "reg_teachers":
                        {
                            IQueryable<Department> departments;
                            foreach (var fac in faculties)
                            {
                                departments = from d in _context.Departments
                                                       .Include(d => d.TeacherRegisterRequests)
                                                       .Where(d => d.TeacherRegisterRequests.Count > 0)
                                                       .Where(d => d.FacultyId == fac.Id)
                                              select d;

                                var testDepList = departments.ToList();

                                foreach (var department in departments) { }
                                if (fac.Departments.Count > 0)
                                    resFacEntityList.Add(fac);
                            }
                            break;
                        }

                    case "exist_students":
                        {
                            // возвращается список всех групп факультета
                            faculties = faculties.Include(f => f.Groups);
                            foreach (var fac in faculties)
                            {
                                if (fac.Groups.Count > 0)
                                    resFacEntityList.Add(fac);
                            }
                            break;
                        }

                    case "exist_teachers":
                        {
                            // возвращается список всех кафедр факультета
                            faculties = faculties.Include(f => f.Departments);
                            foreach (var fac in faculties)
                            {
                                if (fac.Departments.Count > 0)
                                    resFacEntityList.Add(fac);
                            }
                            break;
                        }
                }

                foreach (var entity in resFacEntityList)
                {
                    resFacultyDTOList.Add(FacultyDTO.Map(entity));
                }
                return new OperationDetailDTO<List<FacultyDTO>> { Succeeded = true, Data = resFacultyDTOList, ErrorMessages = null };
            }
            catch (Exception e)
            {
                return new OperationDetailDTO<List<FacultyDTO>> { Succeeded = false, ErrorMessages = { _filtersErrorHeader + _serverErrorMessage + e.Message } };
            }
        }

        // список заявок студентов
        public async Task<OperationDetailDTO<List<StudentDTO>>> GetStudentRegisterRequestsAsync()
        {
            var resultList = new List<StudentDTO>();
            try
            {
                var entityList = await _context.StudentRegisterRequests
                    .AsNoTracking()
                    .ToListAsync();
                foreach(var entity in entityList)
                {
                    var studentDTO = StudentDTO.Map(entity);
                    studentDTO.GroupNumber = _context.Groups.FirstOrDefault(g => g.Id == entity.GroupId).NumberName;
                    resultList.Add(studentDTO);
                }
                return new OperationDetailDTO<List<StudentDTO>> { Succeeded = true, Data = resultList };
            }
            catch (Exception e)
            {
                return new OperationDetailDTO<List<StudentDTO>> { Succeeded = false, ErrorMessages = { _entityGettingError + _serverErrorMessage + e.Message } };
            }
        }

        // список заявок преподов
        public async Task<OperationDetailDTO<List<TeacherDTO>>> GetTeacherRegisterRequestsAsync()
        {
            var resultList = new List<TeacherDTO>();
            try
            {
                var entityList = await _context.TeacherRegisterRequests
                    .AsNoTracking()
                    .ToListAsync();
                foreach (var entity in entityList)
                {
                    var teacherDTO = TeacherDTO.Map(entity);
                    teacherDTO.DepartmentName = _context.Departments.FirstOrDefault(g => g.Id == entity.DepartmentId).Name;
                    resultList.Add(teacherDTO);
                }
                return new OperationDetailDTO<List<TeacherDTO>> { Succeeded = true, Data = resultList };
            }
            catch (Exception e)
            {
                return new OperationDetailDTO<List<TeacherDTO>> { Succeeded = false, ErrorMessages = { _entityGettingError + _serverErrorMessage + e.Message } };
            }
        }

        // список существующих студентов 
        public async Task<OperationDetailDTO<List<StudentDTO>>> GetExistStudentsAsync()
        {
            var resultList = new List<StudentDTO>();
            try
            {
                var entityList = await _context.Students
                    .Include(s => s.Group)
                    .AsNoTracking()
                    .ToListAsync();
                foreach (var entity in entityList)
                {
                    var studentDTO = StudentDTO.Map(entity);
                    resultList.Add(studentDTO);
                }
                return new OperationDetailDTO<List<StudentDTO>> { Succeeded = true, Data = resultList };
            }
            catch (Exception e)
            {
                return new OperationDetailDTO<List<StudentDTO>> { Succeeded = false, ErrorMessages = { _entityGettingError + _serverErrorMessage + e.Message } };
            }
        }

        // список существующих преподов
        public async Task<OperationDetailDTO<List<TeacherDTO>>> GetExistTeachersAsync()
        {
            var resultList = new List<TeacherDTO>();
            try
            {
                var entityList = await _context.Teachers
                    .Include(t => t.Department)
                    .AsNoTracking()
                    .ToListAsync();
                foreach (var entity in entityList)
                {
                    var teacherDTO = TeacherDTO.Map(entity);
                    resultList.Add(teacherDTO);
                }
                return new OperationDetailDTO<List<TeacherDTO>> { Succeeded = true, Data = resultList };
            }
            catch (Exception e)
            {
                return new OperationDetailDTO<List<TeacherDTO>> { Succeeded = false, ErrorMessages = { _entityGettingError + _serverErrorMessage + e.Message } };
            }
        }


        // отклонение (удаление из БД) заявки преподов
        public async Task<OperationDetailDTO> RejectTeacherRequestsAsync(int[] registerEntityIdList)
        {
            try
            {
                var teachers = new List<TeacherRegisterRequest>();
                foreach (var id in registerEntityIdList)
                {
                    teachers.Add(await _context.TeacherRegisterRequests.FirstOrDefaultAsync(t => t.Id == id)); 
                }
                _context.TeacherRegisterRequests.RemoveRange(teachers);
                await _context.SaveChangesAsync();
                return new OperationDetailDTO { Succeeded = true };
            }
            catch (Exception e)
            {
                return new OperationDetailDTO { Succeeded = false, ErrorMessages = {_serverErrorMessage + e.Message } };
            }
        }

        // отклонение (удаление из БД) заявки студентов
        public async Task<OperationDetailDTO> RejectStudentRequestsAsync(int[] registerEntityIdList)
        {
            try
            {
                var students = new List<StudentRegisterRequest>();
                foreach (var id in registerEntityIdList)
                {
                    students.Add(await _context.StudentRegisterRequests.FirstOrDefaultAsync(t => t.Id == id));
                }
                _context.StudentRegisterRequests.RemoveRange(students);
                await _context.SaveChangesAsync();
                return new OperationDetailDTO { Succeeded = true };
            }
            catch (Exception e)
            {
                return new OperationDetailDTO { Succeeded = false, ErrorMessages = { _serverErrorMessage + e.Message } };
            }
        }

        // принятие (добавление пользователя в систему) заявки преподов
        public async Task<OperationDetailDTO> AcceptTeacherRequestsAsync(int[] registerEntityIdList)
        {
            OperationDetailDTO resultDetail = new OperationDetailDTO();
            var errors = new List<string>();
            var newUsers = new List<User>();

            try
            {
                if (registerEntityIdList != null)
                {
                    foreach (var id in registerEntityIdList)
                    {
                        var addingTeacher = await _context.TeacherRegisterRequests.FirstOrDefaultAsync(u => u.Id == id);
                        var user = GetTeacherUserFromRegEntity(addingTeacher);

                        var userResult = await _userManager.CreateAsync(user, user.PasswordHash);
                        if (userResult.Succeeded)
                        {
                            var roleResult = await _userManager.AddToRoleAsync(user, Role.Types.Teacher.ToString());
                            if (roleResult.Succeeded)
                            {
                                _context.TeacherRegisterRequests.Remove(addingTeacher);
                                await _context.SaveChangesAsync();
                                resultDetail = new OperationDetailDTO { Succeeded = true };
                            }
                            else
                            {
                                await _userManager.DeleteAsync(user);
                                foreach (var error in userResult.Errors)
                                    errors.Add("Ошибка при регистрации пользователя-преподавателя " + user.Teacher.Name + " "
                                        + user.Teacher.Patronymic + ". Описание ошибки: " + error.Description);
                                return new OperationDetailDTO { Succeeded = false, ErrorMessages = errors };
                            }
                        }

                        else
                        {
                            foreach (var error in userResult.Errors)
                                errors.Add("Ошибка при регистрации пользователя-преподавателя. " + "Описание ошибки: " + error.Description);
                            return new OperationDetailDTO { Succeeded = false, ErrorMessages = errors };
                        }
                    }
                    return resultDetail;
                }
                else
                    return new OperationDetailDTO { Succeeded = false, ErrorMessages = { "Параметр был нулевым" } };
            }

            catch (Exception e)
            {
                errors.Add(_serverErrorMessage + e.Message);
                return new OperationDetailDTO { Succeeded = false, ErrorMessages = errors };
            }
        }

        // принятие (добавление пользователя в систему) заявки студентов
        public async Task<OperationDetailDTO> AcceptStudentRequestsAsync(int[] registerEntityIdList)
        {
            OperationDetailDTO resultDetail = new OperationDetailDTO();
            var errors = new List<string>();
            var newUsers = new List<User>();
            try
            {
                foreach (int id in registerEntityIdList)
                {
                    var addingStudent = await _context.StudentRegisterRequests.FirstOrDefaultAsync(u => u.Id == id);
                    var user = GetStudentUserFromRegEntity(addingStudent);

                    var userResult = await _userManager.CreateAsync(user, user.PasswordHash);
                    if (userResult.Succeeded)
                    {
                        var roleResult = await _userManager.AddToRoleAsync(user, Role.Types.Student.ToString());
                        if (roleResult.Succeeded)
                        {
                            _context.StudentRegisterRequests.Remove(addingStudent);
                            await _context.SaveChangesAsync();
                            resultDetail = new OperationDetailDTO { Succeeded = true };
                        }
                        else
                        {
                            await _userManager.DeleteAsync(user);
                            foreach (var error in userResult.Errors)
                                errors.Add("Ошибка при регистрации пользователя-студента " + user.Student.Name + " "
                                    + user.Student.Patronymic + ". Описание ошибки: " + error.Description);
                            return new OperationDetailDTO { Succeeded = false, ErrorMessages = errors };
                        }
                    }

                    else
                    {
                        foreach (var error in userResult.Errors)
                            errors.Add("Ошибка при регистрации пользователя-студента. " + "Описание ошибки: " + error.Description);
                        return new OperationDetailDTO { Succeeded = false, ErrorMessages = errors };
                    }
                }
                return resultDetail;
            }

            catch (Exception e)
            {
                errors.Add(_serverErrorMessage + e.Message);
                return new OperationDetailDTO { Succeeded = false, ErrorMessages = errors };
            }
        }


        // удаление из БД студентов
        public async Task<OperationDetailDTO> DeleteExistStudentsAsync(int[] entityIdList)
        {
            try
            {
                foreach(var id in entityIdList)
                {
                    var studentEntity = await _context.Students.Include(s => s.User).FirstOrDefaultAsync(s => s.Id == id);
                    //var user = _context.Users.Include(u => u.Student).Include(u => u.Teacher).FirstOrDefault(u => u.Id == id);
                    await _userManager.DeleteAsync(studentEntity.User);
                }
                return new OperationDetailDTO { Succeeded = true };
            }
            catch (Exception e)
            {
                return new OperationDetailDTO { Succeeded = false, ErrorMessages = { _entityDeletingError +  _serverErrorMessage + e.Message } };
            }
        }

        // удаление из БД преподов
        public async Task<OperationDetailDTO> DeleteExistTeachersAsync(int[] entityIdList)
        {
            try
            {
                if(entityIdList.Length < 1)
                {
                    return new OperationDetailDTO { Succeeded = false, ErrorMessages = { _entityDeletingError + "Параметры удаления пользователей были равны null" } };
                }

                foreach (var id in entityIdList)
                {
                    var teacherEntity = await _context.Teachers
                        .Include(t => t.User)
                        .Include(t => t.Tasks)
                        .FirstOrDefaultAsync(t => t.Id == id);

                    foreach (var task in teacherEntity.Tasks)
                    {
                        var solutions = await _context.Solutions
                            .Where(s => s.TaskId == task.Id)
                            .ToListAsync();

                        var taskStudItems = await _context.TaskStudentItems
                            .Where(ts => ts.TaskId == task.Id)
                            .FirstOrDefaultAsync();

                        _context.Solutions.RemoveRange(solutions);
                        _context.TaskStudentItems.RemoveRange(taskStudItems);
                    }

                    _context.TaskModels.RemoveRange(teacherEntity.Tasks);
                    await _context.SaveChangesAsync();

                    await _userManager.DeleteAsync(teacherEntity.User);
                    await _context.SaveChangesAsync();
                }
                return new OperationDetailDTO { Succeeded = true };
            }
            catch (Exception e)
            {
                return new OperationDetailDTO { Succeeded = false, ErrorMessages = { _entityDeletingError + _serverErrorMessage + e.Message } };
            }
        }

        // удаление из БД группы
        public async Task<OperationDetailDTO> DeleteGroupAsync(int id)
        {
            try
            {
                var group = await _context.Groups.FindAsync(id);
                if (group != null)
                {
                    _context.Groups.Remove(await _context.Groups.FindAsync(id));
                    await _context.SaveChangesAsync();
                    return new OperationDetailDTO { Succeeded = true };
                }
                else
                {
                    var detail = new OperationDetailDTO();
                    detail.ErrorMessages.Add("Ошибка при удалении: группа не найдена");
                    detail.Succeeded = false;
                    return detail;
                }
            }
            catch (Exception e)
            {
                return new OperationDetailDTO { Succeeded = false, ErrorMessages = { _serverErrorMessage + e.Message } };
            }
        }


        // отфильтрованный список заявок студентов
        public async Task<OperationDetailDTO<List<StudentDTO>>> GetStudentRegisterRequestsAsync(FilterDTO[] filters = null)
        {
            var resultList = new List<StudentDTO>();
            try
            {
                var students = from s in _context.StudentRegisterRequests.Include(s => s.Group).ThenInclude(g => g.Faculty) select s;

                students = students.OrderBy(s => s.Surname);

                if (filters != null)
                {
                    foreach(var filter in filters)
                    {
                        switch (filter.Name)
                        {
                            case "facultyId":
                                {
                                    var value = Convert.ToInt32(filter.Value);
                                    if (value > 0)
                                    {
                                        students = students.Where(s => s.Group.FacultyId == value);
                                        students = students.OrderBy(s => s.Group.NumberName);
                                    }
                                    break;
                                }
                            case "groupId":
                                {
                                    var value = Convert.ToInt32(filter.Value);
                                    if (value > 0)
                                    {
                                        students = students.Where(s => s.GroupId == value);
                                    }
                                    break;
                                }
                                
                            case "searchString":
                                {
                                    var value = (string)filter.Value;
                                    if (!String.IsNullOrEmpty(value))
                                    {
                                        students = students.Where(s => s.Name.ToUpper().Contains(value)
                                        || s.Surname.ToUpper().Contains(value)
                                        || s.Patronymic.ToUpper().Contains(value));
                                    }
                                    break;
                                }
                        }
                    }
                }

                foreach (var entity in await students.ToListAsync())
                {
                    var studentDTO = StudentDTO.Map(entity);
                    studentDTO.GroupNumber = _context.Groups.FirstOrDefault(g => g.Id == entity.GroupId).NumberName;
                    resultList.Add(studentDTO);
                }
                return new OperationDetailDTO<List<StudentDTO>> { Succeeded = true, Data = resultList };
            }
            catch (Exception e)
            {
                return new OperationDetailDTO<List<StudentDTO>> { Succeeded = false, ErrorMessages = { _entityGettingError + _serverErrorMessage + e.Message } };
            }
        }

        // отфильтрованный список заявок преподов
        public async Task<OperationDetailDTO<List<TeacherDTO>>> GetTeacherRegisterRequestsAsync(FilterDTO[] filters = null)
        {
            var resultList = new List<TeacherDTO>();
            try
            {
                var teachers = from t in _context.TeacherRegisterRequests.Include(tr => tr.Department).ThenInclude(d => d.Faculty) select t;

                teachers = teachers.OrderBy(t => t.Surname);

                if (filters != null)
                {
                    foreach (var filter in filters)
                    {
                        switch (filter.Name)
                        {
                            case "facultyId":
                                {
                                    var value = Convert.ToInt32(filter.Value);
                                    if (value > 0)
                                    {
                                        teachers = teachers.Where(t => t.Department.FacultyId == value);
                                    }
                                    break;
                                }
                            case "departmentId":
                                {
                                    var value = Convert.ToInt32(filter.Value);
                                    if (value > 0)
                                    {
                                        teachers = teachers.Where(t => t.DepartmentId == value);
                                    }
                                    break;
                                }

                            case "searchString":
                                {
                                    var value = filter.Value;
                                    if (!String.IsNullOrEmpty(value))
                                    {
                                        teachers = teachers.Where(t => t.Name.ToUpper().Contains(value)
                                        || t.Surname.ToUpper().Contains(value)
                                        || t.Patronymic.ToUpper().Contains(value)
                                        || t.Department.Name.Contains(value));
                                    }
                                    break;
                                }
                        }
                    }
                }
                foreach (var entity in await teachers.ToListAsync())
                {
                    var teacherDTO = TeacherDTO.Map(entity);
                    //teacherDTO.DepartmentName = _context.Departments.FirstOrDefault(g => g.Id == entity.DepartmentId).Name;
                    resultList.Add(teacherDTO);
                }
                return new OperationDetailDTO<List<TeacherDTO>> { Succeeded = true, Data = resultList };
            }
            catch (Exception e)
            {
                return new OperationDetailDTO<List<TeacherDTO>> { Succeeded = false, ErrorMessages = { _entityGettingError + _serverErrorMessage + e.Message } };
            }
        }

        // отфильтрованный список существующих студентов
        public async Task<OperationDetailDTO<List<StudentDTO>>> GetExistStudentsAsync(FilterDTO[] filters = null)
        {
            var resultList = new List<StudentDTO>();
            try
            {
                var students = from s in _context.Students.Include(s => s.User).Include(s => s.Group).ThenInclude(g => g.Faculty) select s;

                students = students.OrderBy(s => s.Surname);

                if (filters != null)
                {
                    foreach (var filter in filters)
                    {
                        switch (filter.Name)
                        {
                            case "facultyId":
                                {
                                    var value = Convert.ToInt32(filter.Value);
                                    if (value > 0)
                                    {
                                        students = students.Where(s => s.Group.FacultyId == value);
                                    }
                                    break;
                                }
                            case "groupId":
                                {
                                    var value = Convert.ToInt32(filter.Value);
                                    if (value > 0)
                                    {
                                        students = students.Where(s => s.GroupId == value);
                                    }
                                    break;
                                }

                            case "searchString":
                                {
                                    var value = (string)filter.Value;
                                    if (!String.IsNullOrEmpty(value))
                                    {
                                        students = students.Where(s => s.Name.ToUpper().Contains(value)
                                        || s.Surname.ToUpper().Contains(value)
                                        || s.Patronymic.ToUpper().Contains(value));
                                    }
                                    break;
                                }
                        }
                    }
                }

                foreach (var entity in await students.ToListAsync())
                {
                    var studentDTO = StudentDTO.Map(entity);
                    studentDTO.GroupNumber = _context.Groups.FirstOrDefault(g => g.Id == entity.GroupId).NumberName;
                    resultList.Add(studentDTO);
                }
                return new OperationDetailDTO<List<StudentDTO>> { Succeeded = true, Data = resultList };
            }
            catch (Exception e)
            {
                return new OperationDetailDTO<List<StudentDTO>> { Succeeded = false, ErrorMessages = { _entityGettingError + _serverErrorMessage + e.Message } };
            }
        }

        // отфильтрованный список существующих преподов
        public async Task<OperationDetailDTO<List<TeacherDTO>>> GetExistTeachersAsync(FilterDTO[] filters = null)
        {
            var resultList = new List<TeacherDTO>();
            try
            {
                var teachers = from t in _context.Teachers.Include(tr => tr.User).Include(tr => tr.Department).ThenInclude(d => d.Faculty) select t;

                teachers = teachers.OrderBy(t => t.Surname);

                if (filters != null)
                {
                    foreach (var filter in filters)
                    {
                        switch (filter.Name)
                        {
                            case "facultyId":
                                {
                                    var value = Convert.ToInt32(filter.Value);
                                    if (value > 0)
                                    {
                                        teachers = teachers.Where(t => t.Department.FacultyId == value);
                                    }
                                    break;
                                }
                            case "departmentId":
                                {
                                    var value = Convert.ToInt32(filter.Value);
                                    if (value > 0)
                                    {
                                        teachers = teachers.Where(t => t.DepartmentId == value);
                                    }
                                    break;
                                }

                            case "searchString":
                                {
                                    var value = filter.Value;
                                    if (!String.IsNullOrEmpty(value))
                                    {
                                        teachers = teachers.Where(t => t.Name.ToUpper().Contains(value)
                                        || t.Surname.ToUpper().Contains(value)
                                        || t.Patronymic.ToUpper().Contains(value)
                                        || t.Department.Name.Contains(value));
                                    }
                                    break;
                                }
                        }
                    }
                }
                foreach (var entity in await teachers.ToListAsync())
                {
                    var teacherDTO = TeacherDTO.Map(entity);
                    resultList.Add(teacherDTO);
                }
                return new OperationDetailDTO<List<TeacherDTO>> { Succeeded = true, Data = resultList };
            }
            catch (Exception e)
            {
                return new OperationDetailDTO<List<TeacherDTO>> { Succeeded = false, ErrorMessages = { _entityGettingError + _serverErrorMessage + e.Message } };
            }
        }


        // [тестовый метод] -- testing
        public async Task<OperationDetailDTO<List<TeacherDTO>>> GetTeacherRegisterRequestsAsync_copy(FilterDTO[] filters = null)
        {
            var resultList = new List<TeacherDTO>();
            try
            {
                var teachers = from t in _context.TeacherRegisterRequests.Include(tr => tr.Department).ThenInclude(d => d.Faculty) select t;

                teachers = teachers.OrderBy(t => t.Surname);

                if (filters != null)
                {
                    foreach (var filter in filters)
                    {
                        switch (filter.Name)
                        {
                            case "facultyId":
                                {
                                    var value = Convert.ToInt32(filter.Value);
                                    if (value > 0)
                                    {
                                        teachers = teachers.Where(t => t.Department.FacultyId == value);
                                    }
                                    break;
                                }
                            case "departmentId":
                                {
                                    var value = Convert.ToInt32(filter.Value);
                                    if (value > 0)
                                    {
                                        teachers = teachers.Where(t => t.DepartmentId == value);
                                    }
                                    break;
                                }

                            case "searchString":
                                {
                                    var value = filter.Value;
                                    if (!String.IsNullOrEmpty(value))
                                    {
                                        teachers = teachers.Where(t => t.Name.ToUpper().Contains(value)
                                        || t.Surname.ToUpper().Contains(value)
                                        || t.Patronymic.ToUpper().Contains(value)
                                        || t.Department.Name.Contains(value));
                                    }
                                    break;
                                }
                        }
                    }
                }
                foreach (var entity in await teachers.ToListAsync())
                {
                    var teacherDTO = TeacherDTO.Map(entity);
                    //teacherDTO.DepartmentName = _context.Departments.FirstOrDefault(g => g.Id == entity.DepartmentId).Name;
                    resultList.Add(teacherDTO);
                }
                return new OperationDetailDTO<List<TeacherDTO>> { Succeeded = true, Data = resultList };
            }
            catch (Exception e)
            {
                return new OperationDetailDTO<List<TeacherDTO>> { Succeeded = false, ErrorMessages = { _entityGettingError + _serverErrorMessage + e.Message } };
            }
        }


        // методы конвертации >>
        private User GetTeacherUserFromRegEntity(TeacherRegisterRequest teacherRegister) => new User
        {
            Teacher = new Teacher
            {
                DepartmentId = teacherRegister.DepartmentId,
                Name = teacherRegister.Name,
                Surname = teacherRegister.Surname,
                Patronymic = teacherRegister.Patronymic,
                Position = teacherRegister.Position,
            },
            UserName = teacherRegister.UserName,
            Email = teacherRegister.Email,
            PasswordHash = teacherRegister.PasswordHash
        };

        private User GetStudentUserFromRegEntity(StudentRegisterRequest studentRegister) => new User
        {
            Student = new Student
            {
                GroupId = studentRegister.GroupId,
                Name = studentRegister.Name,
                Surname = studentRegister.Surname,
                Patronymic = studentRegister.Patronymic
            },
            UserName = studentRegister.UserName,
            Email = studentRegister.Email,
            PasswordHash = studentRegister.PasswordHash
        };
    }
}
