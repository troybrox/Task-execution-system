using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System.Text;
using System.Threading.Tasks;
using TaskExecutionSystem.BLL.DTO;
using TaskExecutionSystem.BLL.DTO.Studies;

namespace TaskExecutionSystem.BLL.Interfaces
{
    public interface IAdminService
    {
        public Task<OperationDetailDTO<List<StudentDTO>>> GetStudentRegisterRequestsAsync();
        public Task<OperationDetailDTO<List<TeacherDTO>>> GetTeacherRegisterRequestsAsync();
        public Task<OperationDetailDTO<List<StudentDTO>>> GetExistStudentsAsync();
        public Task<OperationDetailDTO<List<TeacherDTO>>> GetExistTeachersAsync();
        public Task<OperationDetailDTO> RejectRequstsAsync();
        public Task<OperationDetailDTO> AcceptRequestsAsync();
        public Task<OperationDetailDTO<List<FacultyDTO>>> GetAllStudyFiletrsAsync();
    }
}
