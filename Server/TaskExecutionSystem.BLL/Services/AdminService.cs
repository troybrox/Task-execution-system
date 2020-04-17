using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using TaskExecutionSystem.BLL.DTO;
using TaskExecutionSystem.BLL.Interfaces;
using TaskExecutionSystem.DAL.Data;
using TaskExecutionSystem.DAL.Entities;
using TaskExecutionSystem.DAL.Entities.Identity;
using TaskExecutionSystem.BLL.DTO.Studies;

namespace TaskExecutionSystem.BLL.Services
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<User> _userManager;
        private readonly DataContext _context;

        private const string _serverExceptionErrorHeader = "Произошло исключение на сервере. Подробнее: \n";
        private const string _filtersErrorHeader = "Ошибка при получении списков для фильтрации по учебным подразделениям. ";

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
                return new OperationDetailDTO<List<FacultyDTO>> { Succeeded = false, ErrorMessages = { _filtersErrorHeader + _serverExceptionErrorHeader +  e.Message } };
            }
        }
    }
}
