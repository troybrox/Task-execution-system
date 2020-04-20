using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System.Text;
using System.Threading.Tasks;
using TaskExecutionSystem.BLL.DTO;
using TaskExecutionSystem.DAL.Entities.Registration;

namespace TaskExecutionSystem.BLL.Interfaces
{
    public interface IAccountService
    {
        public Task<OperationDetailDTO> CreateStudentRegisterRequestAsync(StudentRegisterDTO dto);
        public Task<OperationDetailDTO> CreateTeacherRegisterRequestAsync(TeacherRegisterDTO dto);

        public Task<OperationDetailDTO<SignInUserDetailDTO>> SignInAsync(UserLoginDTO dto);
        public Task<OperationDetailDTO<SignInUserDetailDTO>> SignOutAsync();

        public Task<OperationDetailDTO> CreateTeacherAsync(TeacherRegisterRequest registerEntity);
        public Task<OperationDetailDTO> CreateStudentAsync(List<int> registerEntityIdList);
    }
}
