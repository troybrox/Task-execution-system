using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using TaskExecutionSystem.BLL.Services;
using Moq;
using TaskExecutionSystem.DAL.Data;
using TaskExecutionSystem.BLL.DTO;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using TaskExecutionSystem.DAL.Entities.Identity;
using TaskExecutionSystem.DAL.Entities.Registration;
using TaskExecutionSystem.DAL.Entities.Studies;
using System.Text.RegularExpressions;
using Moq.Protected;
using TaskExecutionSystem.BLL.DTO.Filters;

namespace TaskExecutionSystem.Tests.BusinessLogic
{
    public class TeacherServiceTests
    {
        private Faculty[] faculties = new Faculty[]
        {
            new Faculty
            {
                Id = 1,
                Name = "Информатики"
            },
            new Faculty
            {
                Id = 2,
                Name = "Двигателей"
            }
        };

        // в тестах производить авторизацию ?

        private async void InitContext(DataContext context)
        {
            var groups = new TaskExecutionSystem.DAL.Entities.Studies.Group[]
            {
                new  TaskExecutionSystem.DAL.Entities.Studies.Group
                {
                    Id = 1,
                    Faculty = faculties[0],
                    NumberName = "1111-010203A",
                },
                new  TaskExecutionSystem.DAL.Entities.Studies.Group
                {
                    Id = 2,
                    Faculty = faculties[1],
                    NumberName = "3333-010203A",
                }
            };




            if (!await context.Faculties.AnyAsync())
            {
                await context.Faculties.AddRangeAsync(faculties);
            }

            if (!await context.Groups.AnyAsync())
            {
                await context.Groups.AddRangeAsync(groups);
            }

            if (!await context.StudentRegisterRequests.AnyAsync())
            {
                await context.StudentRegisterRequests.AddRangeAsync(studentRegs);
            }

            if (!await context.Departments.AnyAsync())
            {
                await context.Departments.AddRangeAsync(departmens);
            }

            if (!await context.TeacherRegisterRequests.AnyAsync())
            {
                await context.TeacherRegisterRequests.AddRangeAsync(teacherRegs);
            }

            await context.SaveChangesAsync();
        }
    }
}
