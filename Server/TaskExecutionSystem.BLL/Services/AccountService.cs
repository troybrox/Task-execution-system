using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using TaskExecutionSystem.BLL.DTO;
using TaskExecutionSystem.BLL.Interfaces;
using TaskExecutionSystem.DAL.Data;
using TaskExecutionSystem.DAL.Entities;
using TaskExecutionSystem.DAL.Entities.Identity;

namespace TaskExecutionSystem.BLL.Services
{
    public class AccountService : IAccountService
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

        public async Task<OperationDetailDTO> LoginAsync()
        {
            
            throw new NotImplementedException();
        }

        public Task<OperationDetailDTO> CreateStudentAsync(StudentRegisterDTO dto)
        {
            throw new NotImplementedException();
        }

        public async Task<OperationDetailDTO> CreateTeacherAsync(TeacherRegisterDTO dto)
        {
            try
            {
                var user = new User
                {
                    Teacher = new Teacher
                    {
                        Name = dto.Name,
                        Surname = dto.Surname,
                        Patronymic = dto.Patronymic,
                        Department = dto.Department,
                        Position = dto.Position,
                        MainSubject = dto.Discipline
                        // UserEntity = user
                    },
                    Email = dto.Email,
                    PasswordHash = dto.Password,
                    UserName = dto.UserName
                };

                //user.PasswordHash = _hasher.HashPassword(user, "Qwe123!");
                var createResult = await _userManager.CreateAsync(user, dto.Password);
                //var roleResult = await _userManager.AddToRoleAsync(user, Role.Types.Teacher.ToString());

                List<string> errors = new List<string>();

                if (!createResult.Succeeded)
                {
                    foreach (var error in createResult.Errors)
                        errors.Add("Ошибка при регистрации пользователя. Код ошибки: " + error.Code + " Описание ошибки: " + error.Description);
                    return new OperationDetailDTO { Succeeded = false, ErrorMessages = errors };
                }

                //if (!roleResult.Succeeded)
                //{
                //    foreach (var error in roleResult.Errors)
                //        errors.Add("Ошибка при регистрации роли пользователя. Код ошибки: " + error.Code + " Описание ошибки: " + error.Description);
                //    return new OperationDetailDTO { Succeeded = false, ErrorMessages = errors };
                //}

                return new OperationDetailDTO { Succeeded = true };
               // return createResult;
               
            }
            catch(Exception e)
            {
                return new OperationDetailDTO { Succeeded = false, ErrorMessages = new List<string>() { e.Message } };
            }
            
        }


    }
}
