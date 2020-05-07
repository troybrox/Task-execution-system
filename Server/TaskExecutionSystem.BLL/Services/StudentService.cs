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
    // TODO: Repository - create, get, update, delete

    public class StudentService : IStudentService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;

        public StudentService(DataContext context, IHttpContextAccessor httpContextAccessor,
            UserManager<User> userManager)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public Task<OperationDetailDTO<StudentDTO>> GetProfileDataAsync()
        {
            throw new NotImplementedException();
        }

        public Task<OperationDetailDTO> UpdateProfileDataAsync(StudentDTO dto)
        {
            throw new NotImplementedException();
        }


        public Task<OperationDetailDTO<List<TaskDTO>>> GetTasksFromDBAsync(FilterDTO[] filters)
        {
            throw new NotImplementedException();
        }

        public Task<OperationDetailDTO<List<SubjectDTO>>> GetTaskFiltersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<OperationDetailDTO<TaskDTO>> GetTaskByIDAsync(int id)
        {
            throw new NotImplementedException();
        }


        private async Task<User> GetUserFromClaimsAsync()
        {
            var userNameClaim = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
            string stringID = userNameClaim.Value;
            var user = await _userManager.FindByIdAsync(stringID);
            return user;
        }
    }
}
