using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System.Text;
using System.Threading.Tasks;
using TaskExecutionSystem.BLL.DTO;
using TaskExecutionSystem.BLL.DTO.Studies;
using TaskExecutionSystem.BLL.DTO.Filters;

namespace TaskExecutionSystem.BLL.Interfaces
{
    public interface IAdminService
    {
        public Task<OperationDetailDTO<List<StudentDTO>>> GetStudentRegisterRequestsAsync();
        public Task<OperationDetailDTO<List<TeacherDTO>>> GetTeacherRegisterRequestsAsync();

        public Task<OperationDetailDTO<List<StudentDTO>>> GetStudentRegisterRequestsAsync(FilterDTO[] filters);
        public Task<OperationDetailDTO<List<TeacherDTO>>> GetTeacherRegisterRequestsAsync(FilterDTO[] filters);

        public Task<OperationDetailDTO<List<StudentDTO>>> GetExistStudentsAsync();
        public Task<OperationDetailDTO<List<TeacherDTO>>> GetExistTeachersAsync();

        public Task<OperationDetailDTO> RejectTeacherRequstsAsync(int[] registerEntityIdList);
        public Task<OperationDetailDTO> RejectStudentRequstsAsync(int[] registerEntityIdList);
        public Task<OperationDetailDTO> AcceptTeacherRequestsAsync(int[] registerEntityIdList);
        public Task<OperationDetailDTO> AcceptStudentRequestsAsync(int[] registerEntityIdList);

        public Task<OperationDetailDTO> DeleteExistStudentsAsync(int[] registerEntityIdList);
        public Task<OperationDetailDTO> DeleteExistTeachersAsync(int[] registerEntityIdList);

        public Task<OperationDetailDTO<List<FacultyDTO>>> GetAllStudyFiletrsAsync();
    }
}
