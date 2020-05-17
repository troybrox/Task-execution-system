using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskExecutionSystem.BLL.DTO;
using TaskExecutionSystem.BLL.DTO.Auth;
using TaskExecutionSystem.BLL.DTO.Studies;
using static TaskExecutionSystem.BLL.Infrastructure.Contracts.ErrorMessageContract;
using TaskExecutionSystem.BLL.Interfaces;
using TaskExecutionSystem.BLL.Validation;
using TaskExecutionSystem.DAL.Data;
using TaskExecutionSystem.DAL.Entities;
using TaskExecutionSystem.DAL.Entities.Identity;
using TaskExecutionSystem.DAL.Entities.Registration;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;

namespace TaskExecutionSystem.BLL.Services
{
    public class AuthService : IAccountService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly DataContext _context;
        private readonly IUserValidator<User> _userValidator;

        public AuthService(IHttpContextAccessor httpContextAccessor, UserManager<User> userManager,
            SignInManager<User> signInManager, DataContext context, IUserValidator<User> userValidator)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _userValidator = userValidator;
        }

        // авторизация пользователя в системе 
        public async Task<OperationDetailDTO<SignInUserDetailDTO>> SignInAsync(UserLoginDTO dto)
        {
            var detail = new OperationDetailDTO<SignInUserDetailDTO>();
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
                        detail.Data = new SignInUserDetailDTO { User = user, UserRoles = roleList };
                        detail.Succeeded = true;
                    }

                    else
                    {
                        var user = await _userManager.FindByEmailAsync(dto.UserName);
                        if(user != null)
                        {
                            var emailRes = await _signInManager.PasswordSignInAsync(user.UserName, dto.Password, true, true);
                            if (emailRes.Succeeded)
                            {
                                var roleList = await _userManager.GetRolesAsync(user);
                                detail.Data = new SignInUserDetailDTO { User = user, UserRoles = roleList };
                                detail.Succeeded = true;
                            }
                            else
                            {
                                detail.ErrorMessages.Add(_signInErrorMessage);
                            }
                        }
                        
                    }
                }
                else
                {
                    detail.ErrorMessages.Add("Имя пользователя или почта введены некроректно");
                }
                return detail;
            }

            catch (Exception e)
            {
                errors.Add(_serverErrorMessage + e.Message);
                return new OperationDetailDTO<SignInUserDetailDTO> { Succeeded = false, ErrorMessages = errors };
            }
        }

        // выход пользователя из системы
        public async Task<OperationDetailDTO> SignOutAsync()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return new OperationDetailDTO { Succeeded = true };
            }
            catch (Exception e)
            {
                return new OperationDetailDTO { Succeeded = false, ErrorMessages = { _serverErrorMessage + e.Message} };
            }
        }

        // создание заявки студента
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

                var testUser = new User
                {
                    Email = dto.Email,
                    UserName = dto.UserName,
                    PasswordHash = dto.Password
                };
                var userValidateRes = await _userValidator.ValidateAsync(_userManager, testUser);
                if (!userValidateRes.Succeeded)
                {
                    errorMessages.Add("Введенные данные не прошли валидацию. Подробнее далее.");
                    foreach(var error in userValidateRes.Errors.ToList())
                    {
                        errorMessages.Add(error.Description);
                    }
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

        // создание заявки препода
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

                var testUser = new User
                {
                    Email = dto.Email,
                    UserName = dto.UserName,
                    PasswordHash = dto.Password
                };
                var userValidateRes = await _userValidator.ValidateAsync(_userManager, testUser);
                if (!userValidateRes.Succeeded)
                {
                    errorMessages.Add("Введенные данные не прошли валидацию. Подробнее далее.");
                    foreach (var error in userValidateRes.Errors.ToList())
                    {
                        errorMessages.Add(error.Description);
                    }
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

        // получение дерева-объекта для выбора фильтров при регистрации
        public async Task<OperationDetailDTO<List<FacultyDTO>>> GetAllStudiesAsync()
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
                return new OperationDetailDTO<List<FacultyDTO>> { Succeeded = false, ErrorMessages = { _serverErrorMessage + e.Message } };
            }
        }

        // методы для конвертации >>

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


        // [в настоящий момент методы не используются]
        // --приём заявки студента
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

        // --приём заявки  преподавтеля
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

        public async Task<OperationDetailDTO> UpdatePasswordAsync(PasswordUpdateDTO dto)
        {
            var detail = new OperationDetailDTO();

            if(dto == null)
            {
                detail.ErrorMessages.Add("Параметр, полученный от клиентского приложения, был равен null");
                return detail;
            }

            try
            {
                var user = await GetUserFromClaimsAsync();

                if (String.IsNullOrEmpty(dto.CurrentPassword) || String.IsNullOrEmpty(dto.NewPassword))
                {
                    detail.ErrorMessages.Add("Введены некорректные данные.");
                    return detail;
                }

                var passwordRes = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.CurrentPassword);

                if (passwordRes == PasswordVerificationResult.Success)
                {
                    var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var passwordResetResult = await _userManager.ResetPasswordAsync(user, passwordResetToken, dto.NewPassword);
                    if (passwordResetResult.Succeeded)
                    {
                        detail.Succeeded = true;
                    }
                    else
                    {
                        detail.ErrorMessages.Add("Некорректно введён новый пароль.");
                        foreach (var error in passwordResetResult.Errors.ToList())
                        {
                            detail.ErrorMessages.Add(error.Description);
                        }
                    }
                }
                else
                {
                    detail.ErrorMessages.Add("Введён неверный текущий пароль.");
                }

                return detail;
            }
            catch(Exception e)
            {
                detail.ErrorMessages.Add(_serverErrorMessage + e.Message);
                return detail;
            }
        }

        // ---

        private async Task<User> GetUserFromClaimsAsync()
        {
            var userNameClaim = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
            string stringID = userNameClaim.Value;
            var user = await _userManager.FindByIdAsync(stringID);
            return user;
        }
    }
}
