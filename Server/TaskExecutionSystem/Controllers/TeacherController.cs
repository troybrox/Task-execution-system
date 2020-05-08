using System;
using System.Collections.Generic;
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
using TaskExecutionSystem.BLL.DTO.Filters;
using TaskExecutionSystem.BLL.DTO.Task;
using TaskExecutionSystem.BLL.Interfaces;
using TaskExecutionSystem.DAL.Entities.Identity;
using static TaskExecutionSystem.Identity.Contracts.IdentityPolicyContract;

namespace TaskExecutionSystem.Controllers
{
    // TODO: Repository - create, get, update, delete

    // api/teacher/profile
    // api/teacher/profile/update [POST]
    // api/teacher/profile/updatepassword

    // api/teacher/main

    // api/teacher/task/add/filters [список групп - у каждой студенты; список типов заданий]
    // api/teacher/task/add [POST]  (filters)
    // api/teacher/task/filters [список предметов - у каждого группы; список типов заданий]
    // api/teacher/task     [POST]  (filters)
    // api/teacher/task/{id}
    // api/teacher/task/close [POST (int id)]
    // api/teacher/task/update [POST (TaskDTO dto)] 


    // контроллер, предоставляющий эндпоинты для рабаты пользователя с ролью преподавателя 
    [Authorize(TeacherUserPolicy)]
    [Route("api/teacher")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly ITeacherService _teacherService;
        public static IWebHostEnvironment _environment;

        public TeacherController(ITaskService taskService, IWebHostEnvironment environment, 
            ITeacherService teacherService)
        {
            _taskService = taskService;
            _environment = environment;
            _teacherService = teacherService;
        }


        [HttpGet("profile")]
        public async Task<IActionResult> GetProfileDataAsync()
        {
            var res = await _teacherService.GetProfileDataAsync();
            return Ok(res);
        }

        [HttpPost("profile/update")]
        public async Task<IActionResult> UpdateProfileAsync([FromBody]TeacherDTO dto)
        {
            var res = await _teacherService.UpdateProfileDataAsync(dto);
            return Ok(res);
        }

        // todo: update Password


        [HttpGet("main")]
        public async Task<IActionResult> GetMainPageDataAsync()
        {
            var res = await _teacherService.GetMainDataAsync();
            return Ok(res);
        }


        [HttpGet("task/filters")]
        public async Task<IActionResult> GetTaskFiltersAsync()
        {
            var res = await _teacherService.GetTaskFiltersAsync();
            return Ok(res);
        }

        [HttpPost("task")]
        public async Task<IActionResult> GetFilteredTasksAsync([FromBody]FilterDTO[] filters)
        {
            var res = await _teacherService.GetTasksFromDBAsync(filters);
            return Ok(res);
        }


        [HttpGet("task/{id}")]
        public async Task<IActionResult> GetTasksByIDAsync(int id)
        {
            var res = await _teacherService.GetTaskByIDAsync(id);
            return Ok(res);
        }

        [HttpGet("task/add/filters")]
        public async Task<IActionResult> GetTaskAddingListFiltersAsync()
        {
            var res = await _teacherService.GetAddingTaskFiltersAsync();
            return Ok(res);
        }

        [HttpPost("_task/add")]
        public async Task<IActionResult> AddTaskAsync_([FromBody]TaskFileModelDTO dto = null)
        {
            var detail = new OperationDetailDTO();
            var res = await _teacherService.CreateNewTaskAsync(dto.Task);
            if (res.Succeeded)
            {
                try
                {
                    var file = Request.Form.Files[0];
                    if(file != null)
                    {
                        var fileName = file.FileName;
                        using (var fileStream = System.IO.File.Create(_environment.WebRootPath + "\\TaskFiles\\" + fileName))
                        {
                            file.CopyTo(fileStream);
                        }
                        var fileRes = await _taskService.AddFileToTaskAsync(int.Parse(res.Data), file.FileName);

                        if (!fileRes.Succeeded)
                        {
                            detail.ErrorMessages.Add("Не удалось загрузить файл к задаче.");
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
                        return Ok(res);
                    }
                    
                }
                catch (Exception e)
                {
                    detail.ErrorMessages.Add("Ошибка на сервере при загрузке файлов: " + e.Message);
                    return Ok(detail);
                }
            }

            else
            {
                return Ok(res);
            }
        }


        [HttpPost("task/add")]
        public async Task<IActionResult> AddTaskAsync([FromBody]TaskCreateModelDTO task = null)
        {
            var res = await _teacherService.CreateNewTaskAsync(task);
            return Ok(res);
        }

        [HttpPost("task/update")]
        public async Task<IActionResult> UpdateTaskAsync([FromBody]TaskCreateModelDTO task = null)
        {
            var res = await _teacherService.UpdateTaskAsync(task);
            return Ok(res);
        }

        [HttpGet("task/{id}/close")]
        public async Task<IActionResult> CloseTaskAsync(int id)
        {
            var res = await _teacherService.CloseTaskAsync(id);
            return Ok(res);
        }

        [HttpPost("task/add/file")]
        public async Task<IActionResult> AddFile()
        {
            var detail = new OperationDetailDTO();
            try
            {
                var allForms = Request.Form;
                StringValues taskIdString;
                var taskIdRes = allForms.TryGetValue(allForms.Keys.First(), out taskIdString);
                var strId = taskIdString.ToString();
                var id = Convert.ToInt32(strId);
                var file = Request.Form.Files[0];
                if (file != null)
                {
                    var fileName = file.FileName;
                    using (var fileStream = System.IO.File.Create(_environment.WebRootPath + "\\Files\\" + "\\TaskFiles\\" + fileName))
                    {
                        file.CopyTo(fileStream);
                    }
                    var fileRes = await _taskService.AddFileToTaskAsync(id, file.FileName);

                    if (!fileRes.Succeeded)
                    {
                        detail.ErrorMessages.Add("Не удалось загрузить файл к задаче.");
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


        // test method
        [HttpPost("upload")]
        public async Task<IActionResult> Upload()
        {
            try
            {
                var file = Request.Form.Files[0];

                if(file.Length > 0)
                {
                    var fileName = file.FileName;
                    using (var fileStream = System.IO.File.Create(_environment.WebRootPath + "\\Files\\" + "\\TaskFiles\\" + fileName))
                    {
                        file.CopyTo(fileStream);
                    }
                    return Ok("Файл загружен");
                }
                else
                {
                    var message = "Параметр был null";
                    return Ok(message);
                }
            }
            catch (Exception e)
            {
                return Ok("Ошибка на сервере при загрузке файлов: " + e.Message);
            }
        }
    }
}
