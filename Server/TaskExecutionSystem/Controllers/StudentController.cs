﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using TaskExecutionSystem.BLL.DTO;
using TaskExecutionSystem.BLL.DTO.Filters;
using TaskExecutionSystem.BLL.DTO.Task;
using TaskExecutionSystem.BLL.Interfaces;
using TaskExecutionSystem.DAL.Entities.Identity;
using static TaskExecutionSystem.Identity.Contracts.IdentityPolicyContract;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaskExecutionSystem.Controllers
{
    [Authorize(StudentUserPolicy)]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        // TODO: Repository - get

        // api/student/profile
        // api/student/profile/update [POST]
        // api/student/profile/updatepassword

        // api/student/task/filters
        // api/student/tasks
        // api/student/solution/add 
        // api/student/solution/update [TODO] 
        // api/student/task/{id}

        private readonly IStudentService _studentService;
        private readonly ITaskService _taskService;
        public static IWebHostEnvironment _environment;

        public StudentController(IStudentService studentService, ITaskService taskService,
            IWebHostEnvironment environment)
        {
            _studentService = studentService;
            _environment = environment;
            _taskService = taskService;
        }

        // todo: update Password

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfileDataAsync()
        {
            var res = await _studentService.GetProfileDataAsync();
            return Ok(res);
        }

        [HttpPost("profile/update")]
        public async Task<IActionResult> UpdateProfileAsync([FromBody]StudentDTO dto)
        {
            var res = await _studentService.UpdateProfileDataAsync(dto);
            return Ok(res);
        }


        [HttpGet("task/filters")]
        public async Task<IActionResult> GetTaskFiltersAsync()
        {
            var res = await _studentService.GetTaskFiltersAsync();
            return Ok(res);
        }

        [HttpPost("tasks")]
        public async Task<IActionResult> GetFilteredTasksAsync([FromBody]FilterDTO[] filters)
        {
            var res = await _studentService.GetTasksFromDBAsync(filters);
            return Ok(res);
        }

        [HttpGet("task/{id}")]
        public async Task<IActionResult> GetTasksByIDAsync(int id)
        {
            var res = await _studentService.GetTaskByIDAsync(id);
            return Ok(res);
        }


        [HttpPost("solution/add")]
        public async Task<IActionResult> GetFilteredTasksAsync([FromBody]SolutionCreateModelDTO dto)
        {
            var res = await _studentService.CreateSolutionAsync(dto);
            return Ok(res);
        }

        [HttpPost("solution/add/file")]
        public async Task<IActionResult> AddFile()
        {
            var detail = new OperationDetailDTO();
            try
            {
                var allForms = Request.Form;

                StringValues solutionIdString;
                var solIdRes = allForms.TryGetValue(allForms.Keys.First(), out solutionIdString);
                var strId = solutionIdString.ToString();
                var id = Convert.ToInt32(strId);

                var file = Request.Form.Files[0];
                if (file != null)
                {
                    var fileName = file.FileName;
                    using (var fileStream = System.IO.File.Create(_environment.WebRootPath + "\\Files\\" + "\\SolutionFiles\\" + fileName))
                    {
                        file.CopyTo(fileStream);
                    }
                    var fileRes = await _taskService.AddFileToSolutionAsync(id, file.FileName);

                    if (!fileRes.Succeeded)
                    {
                        detail.ErrorMessages.Add("Не удалось загрузить файл к решению задачи.");
                        detail.ErrorMessages.AddRange(fileRes.ErrorMessages);
                        return Ok(detail);
                    }
                    else
                    {
                        detail.Succeeded = true;
                        return Ok(detail);
                    }
                }
                else
                {
                    detail.ErrorMessages.Add("Файл равен null");
                    return Ok(detail);
                }
            }
            catch (Exception e)
            {
                detail.ErrorMessages.Add("Ошибка на сервере при загрузке файлов: " + e.Message);
                return Ok(detail);
            }
        }
    }
}
