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
                foreach(var entity in entityList)
                {
                    resFacultyDTOList.Add(FacultyDTO.Map(entity));
                }
                return new OperationDetailDTO<List<FacultyDTO>> { Succeeded = true, Data = resFacultyDTOList, ErrorMessages = null };
            }
            catch (Exception e)
            {
                return new OperationDetailDTO<List<FacultyDTO>> { Succeeded = false, ErrorMessages = { _filtersErrorHeader + _serverErrorMessage +  e.Message } };
            }
        }


        public async Task<OperationDetailDTO> RejectTeacherRequstsAsync(int[] registerEntityIdList)
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

        public async Task<OperationDetailDTO> RejectStudentRequstsAsync(int[] registerEntityIdList)
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

        public async Task<OperationDetailDTO> AcceptTeacherRequestsAsync(int[] registerEntityIdList)
        {
            OperationDetailDTO resultDetail = new OperationDetailDTO();
            var errors = new List<string>();
            var newUsers = new List<User>();
            try
            {
                foreach (int id in registerEntityIdList)
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

            catch (Exception e)
            {
                errors.Add(_serverErrorMessage + e.Message);
                return new OperationDetailDTO { Succeeded = false, ErrorMessages = errors };
            }
        }

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

        // удаление пользователей по id сущности препода/студента
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

        public async Task<OperationDetailDTO> DeleteExistTeachersAsync(int[] entityIdList)
        {
            try
            {
                foreach (var id in entityIdList)
                {
                    var teacherEntity = await _context.Teachers.Include(t => t.User).FirstOrDefaultAsync(t => t.Id == id);
                    await _userManager.DeleteAsync(teacherEntity.User);
                }
                return new OperationDetailDTO { Succeeded = true };
            }
            catch (Exception e)
            {
                return new OperationDetailDTO { Succeeded = false, ErrorMessages = { _entityDeletingError + _serverErrorMessage + e.Message } };
            }
        }

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


        // todo: sort by group / department
        // todo: group object in studentRegRequest
        public async Task<OperationDetailDTO<List<StudentDTO>>> GetStudentRegisterRequestsAsync(FilterDTO[] filters)
        {
            var resultList = new List<StudentDTO>();
            try
            {
                var students = from s in _context.StudentRegisterRequests.Include(s => s.Group) select s;

                if(filters != null)
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

        public async Task<OperationDetailDTO<List<TeacherDTO>>> GetTeacherRegisterRequestsAsync(FilterDTO[] filters)
        {
            var resultList = new List<TeacherDTO>();
            try
            {
                var teachers = from t in _context.TeacherRegisterRequests.Include(tr => tr.Department) select t;

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
    }
}
