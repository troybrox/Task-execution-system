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

namespace TaskExecutionSystem.BLL.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;

        public TeacherService(DataContext context, IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
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
                
                var dto = TeacherDTO.Map(currentUserEntity.Teacher);

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

        public Task<OperationDetailDTO> UpdateProfileDataAsync(TeacherDTO dto)
        {
            throw new NotImplementedException();
        }


        public Task<OperationDetailDTO<List<SubjectDTO>>> GetMainDataAsync()
        {
            throw new NotImplementedException();
        }


        public Task<OperationDetailDTO<List<SubjectDTO>>> GetTaskFiltersAsync()
        {
            throw new NotImplementedException();
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

        private async Task<User> GetCurrentUser() => await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
    }
}
