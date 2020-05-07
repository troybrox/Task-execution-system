using System.Collections.Generic;
using System.Threading.Tasks;
using TaskExecutionSystem.BLL.DTO;
using TaskExecutionSystem.BLL.DTO.Auth;
using TaskExecutionSystem.DAL.Entities.Registration;
using TaskExecutionSystem.BLL.DTO.Studies;

namespace TaskExecutionSystem.BLL.Interfaces
{
    public interface IAccountService
    {
        public Task<OperationDetailDTO> CreateStudentRegisterRequestAsync(StudentRegisterDTO dto);
        public Task<OperationDetailDTO> CreateTeacherRegisterRequestAsync(TeacherRegisterDTO dto);

        public Task<OperationDetailDTO> UpdatePasswordAsync(PasswordUpdateDTO dto);

        public Task<OperationDetailDTO<SignInUserDetailDTO>> SignInAsync(UserLoginDTO dto);
        public Task<OperationDetailDTO> SignOutAsync();

        public Task<OperationDetailDTO<List<FacultyDTO>>> GetAllStudiesAsync();

        public Task<OperationDetailDTO> CreateTeacherAsync(TeacherRegisterRequest registerEntity);
        public Task<OperationDetailDTO> CreateStudentAsync(List<int> registerEntityIdList);
    }
}
