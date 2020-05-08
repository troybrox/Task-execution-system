using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using TaskExecutionSystem.BLL.DTO;
using TaskExecutionSystem.BLL.Interfaces;
using static TaskExecutionSystem.BLL.Infrastructure.Contracts.ErrorMessageContract;
using TaskExecutionSystem.DAL.Data;
using TaskExecutionSystem.DAL.Entities;
using TaskExecutionSystem.DAL.Entities.Identity;
using TaskExecutionSystem.DAL.Entities.Registration;

namespace TaskExecutionSystem.BLL.Services
{
    // [этот класс не используется, используется другая реализация интерфейся - AuthService]
    public class AccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _context;

        public AccountService(UserManager<User> userManager, SignInManager<User> signInManager,
            IHttpContextAccessor httpContextAccessor, DataContext context)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
        }

        // авторизация пользователя в системе 
        public async Task<OperationDetailDTO<SignInUserDetailDTO>> SignInAsync(UserLoginDTO dto)
        {
            List<string> errors = new List<string>();
            
            try
            {
                if (!String.IsNullOrEmpty(dto.UserName))
                {
                    // try username
                    var userNameRes = await _signInManager.PasswordSignInAsync(dto.UserName, dto.Password, true, true);
                    if (userNameRes.Succeeded)
                    {
                        var user = await _userManager.FindByNameAsync(dto.UserName);
                        var roleList = await _userManager.GetRolesAsync(user);
                        return new OperationDetailDTO<SignInUserDetailDTO> { Succeeded = true, Data = new SignInUserDetailDTO { User = user, UserRoles = roleList } };
                    }

                    else
                    {
                        // try email
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

        // создание сущности студента и добавление в БД
        public async Task<OperationDetailDTO> CreateStudentAsync(StudentRegisterDTO dto)
        {
            OperationDetailDTO resultDetail;
            List<string> errors = new List<string>();
            try
            {
                var student = new Student
                {
                    Name = dto.Name,
                    Surname = dto.Surname,
                    Patronymic = dto.Patronymic,
                    GroupId = dto.GroupId
                };

                var user = new User
                {
                    Student = student,
                    Email = dto.Email,
                    PasswordHash = dto.Password,
                    UserName = dto.UserName,
                    EntityId = student.Id
                };

                var userResult = await _userManager.CreateAsync(user, dto.Password);
                if (userResult.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(user, Role.Types.Student.ToString());
                    if (roleResult.Succeeded)
                    {
                        resultDetail = new OperationDetailDTO { Succeeded = true };
                    }
                    else
                    {
                        await _userManager.DeleteAsync(user);
                        foreach (var error in userResult.Errors)
                            errors.Add("Ошибка при регистрации пользователя-студента. Код ошибки: " + error.Code + ". Описание ошибки: " + error.Description);
                        resultDetail = new OperationDetailDTO { Succeeded = false, ErrorMessages = errors };
                    }
                }
                else
                {
                    foreach (var error in userResult.Errors)
                        errors.Add("Ошибка при регистрации пользователя-студента.\nКод ошибки: " + error.Code + " Описание ошибки: " + error.Description);
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

        
        public async Task<OperationDetailDTO> CreateTeacherAsync(TeacherRegisterDTO dto)
        {
            OperationDetailDTO resultDetail;
            List<string> errors = new List<string>();
            try
            {
                var teacher = new Teacher
                {
                    Name = dto.Name,
                    Surname = dto.Surname,
                    Patronymic = dto.Patronymic,
                    Position = dto.Position,
                    DepartmentId = dto.DepartmentId,
                };

                var user = new User
                {
                    Teacher = teacher,
                    Email = dto.Email,
                    PasswordHash = dto.Password,
                    UserName = dto.UserName,
                    EntityId = teacher.Id
                };

                var userResult = await _userManager.CreateAsync(user, dto.Password);
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

        public Task<OperationDetailDTO> CreateStudentRegisterRequestAsync(StudentRegisterDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task<OperationDetailDTO> CreateTeacherRegisterRequestAsync(TeacherRegisterDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task<OperationDetailDTO<SignInUserDetailDTO>> SignOutAsync()
        {
            throw new NotImplementedException();
        }


        public Task<OperationDetailDTO> CreateStudentAsync(StudentRegisterRequest registerEntity)
        {
            throw new NotImplementedException();
        }

        public Task<OperationDetailDTO> CreateTeacherAsync(TeacherRegisterRequest registerEntity)
        {
            throw new NotImplementedException();
        }

        public Task<OperationDetailDTO> CreateStudentAsync(int registerEntityId)
        {
            throw new NotImplementedException();
        }

        public Task<OperationDetailDTO> CreateStudentAsync(List<int> registerEntityIdList)
        {
            throw new NotImplementedException();
        }
    }

}
