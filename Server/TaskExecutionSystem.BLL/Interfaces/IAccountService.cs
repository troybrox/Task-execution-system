﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System.Text;
using System.Threading.Tasks;
using TaskExecutionSystem.BLL.DTO;

namespace TaskExecutionSystem.BLL.Interfaces
{
    public interface IAccountService
    {
        public Task<OperationDetailDTO> CreateTeacherAsync(TeacherRegisterDTO dto);

        public Task<OperationDetailDTO> CreateStudentAsync(StudentRegisterDTO dto);

        public Task<OperationDetailDTO> LoginAsync();
    }
}
