using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
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
    public class AdminService : IAdminService
    {
        private readonly UserManager<User> _userManager;
        private readonly DataContext _context;

        private const string _serverErrorMessage = "Ошибка, произошло исключение на сервере. Подробнее: ";
        private const string _signInErrorMessage = "Ошибка при авторизации. Неверное имя пользователя/электронная почта или пароль. Проверьте правильность ввода и повторите попытку.";

        public AdminService(UserManager<User> userManager, DataContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        public Task<OperationDetailDTO<List<StudentDTO>>> GetStudentRegisterRequestsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<OperationDetailDTO<List<TeacherDTO>>> GetTeacherRegisterRequestsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<OperationDetailDTO<List<StudentDTO>>> GetExistStudentsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<OperationDetailDTO<List<TeacherDTO>>> GetExistTeachersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<OperationDetailDTO> RejectRequstsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<OperationDetailDTO> AcceptRequestsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
