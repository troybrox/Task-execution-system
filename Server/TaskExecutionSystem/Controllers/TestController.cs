using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Extensions.Primitives;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskExecutionSystem.BLL.DTO;
using TaskExecutionSystem.BLL.DTO.Studies;
using TaskExecutionSystem.BLL.DTO.Filters;
using TaskExecutionSystem.BLL.DTO.Repository;
using TaskExecutionSystem.BLL.DTO.Task;
using TaskExecutionSystem.BLL.Interfaces;
using TaskExecutionSystem.DAL.Entities.Identity;
using static TaskExecutionSystem.Identity.Contracts.IdentityPolicyContract;
using System.Diagnostics;
using TaskExecutionSystem.BLL.DTO.Auth;

namespace TaskExecutionSystem.Controllers
{
    [Route("api/test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ITeacherService _teacherService;
        public static IWebHostEnvironment _environment;

        public TestController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }


        [HttpGet("main/subjects")]
        public async Task<IActionResult> GeSubjectsForMainPageAsync()
        {
            var res = await _teacherService.GetSubjectsForMainAsync();
            return Ok(res);
        }

        [HttpGet("main/groups/{id}")]
        public async Task<IActionResult> GetGroupsForMainPageAsync(int id)
        {
            var res = await _teacherService.GetGroupsForSubjectAsync(id);
            return Ok(res);
        }

        // get методы с инитом объектов фильтров

        [HttpGet("main/filters/{subjectId}/{groupId}/{studentId}")]
        public async Task<IActionResult> GetTasksForMainPageAsync(string subjectId, string groupId, string studentId)
        {
            FilterDTO[] filters = new FilterDTO[]
            {
                new FilterDTO
                {
                    Name = "groupId",
                    Value = groupId
                },
                new FilterDTO
                {
                    Name = "subjectId",
                    Value = subjectId
                },
                new FilterDTO
                {
                    Name = "studentId",
                    Value = studentId
                }

            };

            var res = await _teacherService.GetTasksForStudentAsync(filters);
            return Ok(res);
        }


        [HttpGet("main/students/filters/{subjectId}/{groupId}")]
        public async Task<IActionResult> GetStudentsForMainPageAsyn(string subjectId, string groupId)
        {
            FilterDTO[] filters = new FilterDTO[]
            {
                new FilterDTO
                {
                    Name = "groupId",
                    Value = groupId
                },
                new FilterDTO
                {
                    Name = "subjectId",
                    Value = subjectId
                }
            };
            var res = await _teacherService.GetStudentsForMainAsync(filters);
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> GetTasksForMainPageAsyn([FromBody]FilterDTO[] filters)
        {
            var res = await _teacherService.GetTasksForStudentAsync(filters);
            return Ok(res);
        }
    }
}
