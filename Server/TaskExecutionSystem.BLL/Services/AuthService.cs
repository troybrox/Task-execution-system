using System;
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

namespace TaskExecutionSystem.BLL.Services
{
    public class AuthService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly DataContext _context;

        private const string _serverErrorMessage = "Ошибка, произошло исключение на сервере. Подробнее: ";
        private const string _signInErrorMessage = 
            "Ошибка при авторизации. Неверное имя пользователя/электронная почта или пароль. Проверьте правильность ввода и повторите попытку.";

        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, DataContext context)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // авторизация пользователя в системе 
        public async Task<OperationDetailDTO<SignInUserDetailDTO>> SignInAsync(UserLoginDTO dto)
        {
            List<string> errors = new List<string>();

            try
            {
                if (!String.IsNullOrEmpty(dto.UserName))
                {
                    var userNameRes = await _signInManager.PasswordSignInAsync(dto.UserName, dto.Password, true, true);
                    if (userNameRes.Succeeded)
                    {
                        var user = await _userManager.FindByNameAsync(dto.UserName);
                        var roleList = await _userManager.GetRolesAsync(user);
                        return new OperationDetailDTO<SignInUserDetailDTO> { Succeeded = true, Data = new SignInUserDetailDTO { User = user, UserRoles = roleList } };
                    }

                    else
                    {
                        var user = await _userManager.FindByEmailAsync(dto.UserName);
                        var emailRes = await _signInManager.PasswordSignInAsync(user, user.PasswordHash, true, true);
                        if (emailRes.Succeeded)
                        {
                            var roleList = await _userManager.GetRolesAsync(user);
                            return new OperationDetailDTO<SignInUserDetailDTO> { Succeeded = true, Data = new SignInUserDetailDTO { User = user, UserRoles = roleList } };
                        }
                        else
                        {
                            errors.Add(_signInErrorMessage);
                            return new OperationDetailDTO<SignInUserDetailDTO> { Succeeded = false, ErrorMessages = errors };
                        }
                    }
                }
                else
                {
                    errors.Add("Имя пользователя или почта не соответствует требованиям");
                    return new OperationDetailDTO<SignInUserDetailDTO> { Succeeded = false, ErrorMessages = errors };
                }
            }

            catch (Exception e)
            {
                errors.Add(_serverErrorMessage + e.Message);
                return new OperationDetailDTO<SignInUserDetailDTO> { Succeeded = false, ErrorMessages = errors };
            }
        }

        public Task<OperationDetailDTO<SignInUserDetailDTO>> SignOutAsync()
        {
            throw new NotImplementedException();
        }


        public async Task<OperationDetailDTO> CreateStudentRegisterRequestAsync(StudentRegisterDTO dto)
        {
            List<string> errorMessages = new List<string>();
            try
            {
                if(!UserValidator.Validate(dto, out errorMessages))
                {
                    return new OperationDetailDTO { Succeeded = false, ErrorMessages = errorMessages };
                }
                if(await _context.StudentRegisterRequests.AnyAsync(x => x.UserName == dto.UserName)
                    || await _context.TeacherRegisterRequests.AnyAsync(x => x.UserName == dto.UserName)
                    || await _userManager.FindByNameAsync(dto.UserName) != null)
                {
                    errorMessages.Add("Пользователь с таким имененем пользователем уже существует. Пожалуйста, выберите другое.");
                    return new OperationDetailDTO { Succeeded = false, ErrorMessages = errorMessages };
                }

                await _context.StudentRegisterRequests.AddAsync(GetStudentRegEntityFromDTO(dto));
                await _context.SaveChangesAsync();
                return new OperationDetailDTO { Succeeded = true };
            }
            catch(Exception e)
            {
                return new OperationDetailDTO { Succeeded = false, ErrorMessages = { _serverErrorMessage + e.Message } };
            }
        }

        public async Task<OperationDetailDTO> CreateTeacherRegisterRequestAsync(TeacherRegisterDTO dto)
        {
            List<string> errorMessages = new List<string>();
            try
            {
                if (!UserValidator.Validate(dto, out errorMessages))
                {
                    return new OperationDetailDTO { Succeeded = false, ErrorMessages = errorMessages };
                }
                if (await _context.StudentRegisterRequests.AnyAsync(x => x.UserName == dto.UserName)
                    || await _context.TeacherRegisterRequests.AnyAsync(x => x.UserName == dto.UserName)
                    || await _userManager.FindByNameAsync(dto.UserName) != null)
                {
                    errorMessages.Add("Пользователь с таким имененем пользователем уже существует. Пожалуйста, выберите другое.");
                    return new OperationDetailDTO { Succeeded = false, ErrorMessages = errorMessages };
                }

                await _context.TeacherRegisterRequests.AddAsync(GetTeacherRegEntityFromDTO(dto));
                await _context.SaveChangesAsync();
                return new OperationDetailDTO { Succeeded = true };
            }
            catch (Exception e)
            {
                return new OperationDetailDTO { Succeeded = false, ErrorMessages = { _serverErrorMessage + e.Message } };
            }
        }

        
        private StudentRegisterRequest GetStudentRegEntityFromDTO(StudentRegisterDTO dto) => new StudentRegisterRequest
        {
            GroupId = dto.GroupId,
            Name = dto.Name,
            Surname = dto.Surname,
            Patronymic = dto.Patronymic,
            Email = dto.Email,
            PasswordHash = dto.Password,
            UserName = dto.UserName,
        };

        private TeacherRegisterRequest GetTeacherRegEntityFromDTO(TeacherRegisterDTO dto) => new TeacherRegisterRequest
        {
            DepartmentId = dto.DepartmentId,
            Name = dto.Name,
            Surname = dto.Surname,
            Patronymic = dto.Patronymic,
            Position = dto.Position,
            Email = dto.Email,
            PasswordHash = dto.Password,
            UserName = dto.UserName,
        };

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
            Email = teacherRegister.Email
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


        // ---

        // приём заявки студента
        //todo: delete regRequest
        public async Task<OperationDetailDTO> CreateStudentAsync(List<int> registerEntityIdList)
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

        // приём заявки  преподавтеля
        public async Task<OperationDetailDTO> CreateTeacherAsync(TeacherRegisterRequest registerEntity)
        {
            OperationDetailDTO resultDetail;
            List<string> errors = new List<string>();
            try
            {
                var user = GetTeacherUserFromRegEntity(registerEntity);
                var userResult = await _userManager.CreateAsync(user, registerEntity.PasswordHash);
                if (userResult.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(user, Role.Types.Teacher.ToString());
                    if (roleResult.Succeeded)
                    {
                        resultDetail = new OperationDetailDTO { Succeeded = true };
                    }
                    else
                    {
                        await _userManager.DeleteAsync(user);
                        foreach (var error in userResult.Errors)
                            errors.Add("Ошибка при регистрации пользователя-преподавателя. Код ошибки: " + error.Code + ". Описание ошибки: " + error.Description);
                        resultDetail = new OperationDetailDTO { Succeeded = false, ErrorMessages = errors };
                    }
                }
                else
                {
                    foreach (var error in userResult.Errors)
                        errors.Add("Ошибка при регистрации пользователя-преподавателя. Код ошибки: " + error.Code + " Описание ошибки: " + error.Description);
                    resultDetail = new OperationDetailDTO { Succeeded = false, ErrorMessages = errors };
                }
                return resultDetail;
            }

            catch (Exception e)
            {
                errors.Add(_serverErrorMessage + e.Message);
                return new OperationDetailDTO { Succeeded = false, ErrorMessages = errors };
            }
        }

        // ---
    }
}
