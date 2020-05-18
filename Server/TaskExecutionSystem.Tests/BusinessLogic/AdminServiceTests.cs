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
    public class AdminServiceTests
    {
        private Mock<UserManager<User>> userManager = MockUserManager.GetUserManager<User>();

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

                    },

                };

            var departmens = new Department[]
            {
                new Department
                {
                    Id = 1,
                    Name = "Программных систем",
                    Faculty = faculties[0],
                },

                new Department
                {
                    Id = 2,
                    Name = "Силовых установок",
                    Faculty = faculties[1]
                }
            };

            var studentRegs = new StudentRegisterRequest[]
                {
                    new StudentRegisterRequest
                    {
                        Id = 1,
                        Group = groups[0],
                        PasswordHash = "qwerty",
                        Email = "sdfsdf@sddsf.cc",
                        Name = "Алёша",
                        Surname = "Бурына",
                        Patronymic = "Котов",
                        UserName = "erttt",
                    },

                    new StudentRegisterRequest
                    {
                        Id = 2,
                        Group = groups[1],
                        PasswordHash = "qwerty",
                        Email = "sdfsdf@sddsf.cc",
                        Name = "Алёша",
                        Surname = "Бурына",
                        Patronymic = "Котов",
                        UserName = "jeep"
                    }
                };

            var teacherRegs = new TeacherRegisterRequest[]
                {
                    new TeacherRegisterRequest
                    {
                        Id = 1,
                        Department = departmens[0],
                        PasswordHash = "qwerty",
                        Email = "sdfsdf@sddsf.cc",
                        Name = "Алёша",
                        Surname = "Бурына",
                        Patronymic = "Котов",
                        UserName = "teacher1",
                        Position = "Доцент"
                    },

                    new TeacherRegisterRequest
                    {
                        Id = 2,
                        Department = departmens[1],
                        PasswordHash = "qwerty",
                        Email = "sdfsdf@sddsf.cc",
                        Name = "Алёша",
                        Surname = "Бурына",
                        Patronymic = "Котов",
                        UserName = "teacher2",
                        Position = "Профессор"
                    },
                };


            if (!await context.Faculties.AnyAsync())
            {
                await context.Faculties.AddRangeAsync(faculties);
            }
            
            if(!await context.Groups.AnyAsync())
            {
                await context.Groups.AddRangeAsync(groups);
            }

            if(!await context.StudentRegisterRequests.AnyAsync())
            {
                await context.StudentRegisterRequests.AddRangeAsync(studentRegs);
            }

            if(!await context.Departments.AnyAsync())
            {
                await context.Departments.AddRangeAsync(departmens);
            }

            if (!await context.TeacherRegisterRequests.AnyAsync())
            {
                await context.TeacherRegisterRequests.AddRangeAsync(teacherRegs);
            }

            await context.SaveChangesAsync();
        }

        // studentRegs tests
        [Fact]
        public async void GetStudentRegRequests_NullFilters_CheckSuccedeed()
        {
            using (var context = InMemoryDBContext.GetDBContext())
            {
                // Arrange
                var adminSerice = new AdminService(userManager.Object, context);
                InitContext(context);

                // Act
                var result = await adminSerice.GetStudentRegisterRequestsAsync(null);

                // Assert
                Assert.True(result.Succeeded);
            }
        }

        [Fact]
        public async void GetStudentRegRequests_NullFilters_CheckListCount()
        {
            using (var context = InMemoryDBContext.GetDBContext())
            {
                // Arrange
                var adminSerice = new AdminService(userManager.Object, context);
                InitContext(context);

                // Act
                var result = await adminSerice.GetStudentRegisterRequestsAsync(null);
                var resultStudents = result.Data;

                // Assert
                Assert.True(resultStudents.Count > 0);
            }
        }

        [Fact]
        public async void GetStudentRegRequests_FacultyFilter_CheckSuccedeed()
        {
            using (var context = InMemoryDBContext.GetDBContext())
            {
                // Arrange
                var adminSerice = new AdminService(userManager.Object, context);
                InitContext(context);
                var filters = new FilterDTO[]
                {
                    new FilterDTO
                    {
                        Name = "facultyId",
                        Value = "1"
                    }
                };

                // Act
                var result = await adminSerice.GetStudentRegisterRequestsAsync(filters);

                // Assert
                Assert.True(result.Succeeded);
            }
        }


        [Fact]
        public async void GetStudentRegRequests_FacultyFilter_CheckListCount()
        {
            using (var context = InMemoryDBContext.GetDBContext())
            {
                // Arrange
                var adminSerice = new AdminService(userManager.Object, context);
                InitContext(context);
                var filters = new FilterDTO[]
                {
                    new FilterDTO
                    {
                        Name = "facultyId",
                        Value = "1"
                    }
                };

                // Act
                var result = await adminSerice.GetStudentRegisterRequestsAsync(filters);
                var resultStudents = result.Data;

                // Assert
                Assert.True(resultStudents.Count > 0);
            }
        }

        [Fact]
        public async void GetStudentRegRequests_GroupFilter_CheckEmptyList()
        {
            using (var context = InMemoryDBContext.GetDBContext())
            {
                // Arrange
                var adminSerice = new AdminService(userManager.Object, context);
                InitContext(context);
                var filters = new FilterDTO[]
                {
                    new FilterDTO
                    {
                        Name = "groupId",
                        Value = "3"
                    }
                };

                // Act
                var result = await adminSerice.GetStudentRegisterRequestsAsync(filters);
                var resultStudents = result.Data;

                // Assert
                Assert.Empty(resultStudents);
            }
        }

        [Fact]
        public async void GetStudentRegRequests_GroupFilter_CheckSingle()
        {
            using (var context = InMemoryDBContext.GetDBContext())
            {
                // Arrange
                var adminSerice = new AdminService(userManager.Object, context);
                InitContext(context);
                var filters = new FilterDTO[]
                {
                    new FilterDTO
                    {
                        Name = "groupId",
                        Value = "2"
                    }
                };

                // Act
                var result = await adminSerice.GetStudentRegisterRequestsAsync(filters);
                var resultStudents = result.Data;

                // Assert
                Assert.Single(resultStudents);
            }
        }


        // teacherRegs tests
        [Fact]
        public async void GetTeacherRegRequests_NullFilter_CheckSuccedeed()
        {
            using (var context = InMemoryDBContext.GetDBContext())
            {
                // Arrange
                var adminSerice = new AdminService(userManager.Object, context);
                InitContext(context);

                // Act
                var result = await adminSerice.GetTeacherRegisterRequestsAsync(null);

                // Assert
                Assert.True(result.Succeeded);
            }
        }

        [Fact]
        public async void GetTeacherRegRequests_NullFilters_CheckListCount()
        {
            using (var context = InMemoryDBContext.GetDBContext())
            {
                // Arrange
                var adminSerice = new AdminService(userManager.Object, context);
                InitContext(context);

                // Act
                var result = await adminSerice.GetTeacherRegisterRequestsAsync(null);
                var resultStudents = result.Data;

                // Assert
                Assert.True(resultStudents.Count > 0);
            }
        }

        [Fact]
        public async void GetTeacherRegRequests_DepartmentFilter_CheckSuccedeed()
        {
            using (var context = InMemoryDBContext.GetDBContext())
            {
                // Arrange
                var adminSerice = new AdminService(userManager.Object, context);
                InitContext(context);
                var filters = new FilterDTO[]
                {
                    new FilterDTO
                    {
                        Name = "departmentId",
                        Value = "1"
                    }
                };

                // Act
                var result = await adminSerice.GetTeacherRegisterRequestsAsync(filters);

                // Assert
                Assert.True(result.Succeeded);
            }
        }

        [Fact]
        public async void GetTeacherRegRequests_DepartmentFilter_CheckListSingle()
        {
            using (var context = InMemoryDBContext.GetDBContext())
            {
                // Arrange
                var adminSerice = new AdminService(userManager.Object, context);
                InitContext(context);
                var filters = new FilterDTO[]
                {
                    new FilterDTO
                    {
                        Name = "departmentId",
                        Value = "1"
                    }
                };

                // Act
                var result = await adminSerice.GetTeacherRegisterRequestsAsync(filters);
                var resultTeachers = result.Data;

                // Assert
                Assert.Single(resultTeachers);
            }
        }

        [Fact]
        public async void GetTeacherRegRequests_FacultyFilter_CheckEmptyList()
        {
            using (var context = InMemoryDBContext.GetDBContext())
            {
                // Arrange
                var adminSerice = new AdminService(userManager.Object, context);
                InitContext(context);
                var filters = new FilterDTO[]
                {
                    new FilterDTO
                    {
                        Name = "facultyId",
                        Value = "3"
                    }
                };

                // Act
                var result = await adminSerice.GetTeacherRegisterRequestsAsync(filters);
                var resultTeachers = result.Data;

                // Assert
                Assert.Empty(resultTeachers);
            }
        }
    }
}
