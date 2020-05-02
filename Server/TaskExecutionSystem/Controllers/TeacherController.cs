﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskExecutionSystem.BLL.DTO;
using TaskExecutionSystem.BLL.DTO.Task;
using TaskExecutionSystem.BLL.Interfaces;

namespace TaskExecutionSystem.Controllers
{
    // todo: скинуть Олегу объекты

    // api/teacher/profile

    // api/teacher/profile/update [POST]

    // api/teacher/profile/updatepassword

    // api/teacher/main


    // api/teacher/task/add/filters [список групп - у каждой студенты; список типов заданий]

    // api/teacher/task/add [POST (filters)]

    // api/teacher/task/filters [список предметов - у каждого группы; список типов заданий]

    // api/teacher/task [POST  (filters)]

    // api/teacher/task/{id}

    // api/teacher/task/{id}/close



    [Route("api/teacher")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        public class FileModel
        {
            public IFormFile uploadedFile { get; set; }
        }

        private readonly ITaskService _taskService;
        private readonly ITeacherService _teacherService;
        public static IWebHostEnvironment _environment;

        public TeacherController(ITaskService taskService, IWebHostEnvironment environment, 
            ITeacherService teacherService)
        {
            _taskService = taskService;
            _environment = environment;
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

        [HttpGet("main")]
        public async Task<IActionResult> GetMainPageDataAsync()
        {
            var res = await _teacherService.GetMainDataAsync();
            return Ok(res);
        }


        [HttpPost("task/filters")]
        public async Task<IActionResult> GetTaskListFiltersAsync()
        {
            var res = await _teacherService.GetTaskFiltersAsync();
            return Ok(res);
        }

        [HttpPost("task/add/filters")]
        public async Task<IActionResult> GetTaskAddingListFiltersAsync()
        {
            var res = await _teacherService.GetAddingTaskFiltersAsync();
            return Ok(res);
        }

        [HttpPost("task/add")]
        public async Task<IActionResult> AddTaskAsync([FromBody]TaskFileModelDTO dto)
        {
            var detail = new OperationDetailDTO();
            var res = await _teacherService.CreateNewTaskAsync(dto.Task);
            if (res.Succeeded)
            {
                var file = dto.File;
                if (file.Length > 0)
                {
                    var fileName = file.FileName;
                    using (var fileStream = System.IO.File.Create(_environment.WebRootPath + "\\TaskFiles\\" + fileName))
                    {
                        file.CopyTo(fileStream);
                    }
                    var fileRes = await _taskService.AddFileToTaskAsync(res.Data.Id, file.FileName);

                    if (!fileRes.Succeeded)
                    {
                        detail.ErrorMessages.Add("Не удалось загрузить файл к задаче.");
                        detail.ErrorMessages.AddRange(fileRes.ErrorMessages);
                    }
                    else
                    {
                        detail.Succeeded = true;
                    }
                }
            }
            return Ok(detail);
        }




        [HttpPost("upload")]
        public async Task<IActionResult> Upload()
        {
            try
            {
                var file = Request.Form.Files[0];

                if(file.Length > 0)
                {
                    var fileName = file.FileName;
                    using (var fileStream = System.IO.File.Create(_environment.WebRootPath + "\\TaskFiles\\" + fileName))
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

        [HttpPost("add_task_file")]
        public async Task<IActionResult> AddFileToTaskAsync(FileModel file)
        {
            try
            {
                string message = "";
                if (file.uploadedFile.Length > 0)
                {
                    if (!Directory.Exists(_environment.WebRootPath + "\\TaskFiles\\"))
                    {
                        Directory.CreateDirectory(_environment.WebRootPath + "\\TaskFiles\\");
                    }
                    using (var fileStream = System.IO.File.Create(_environment.WebRootPath + "\\TaskFiles\\" + file.uploadedFile.FileName))
                    {
                        file.uploadedFile.CopyTo(fileStream);
                        fileStream.Flush();
                        return Ok("Файл загружен " + file.uploadedFile.FileName);
                    }
                }
                else
                {
                    message = "Параметр был null";
                    return Ok(message);
                }
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }
    }
}



//var uploadedFile = file.uploadedFile;
//try 
//{
//    string message = "";
//    if (uploadedFile != null)
//    {
//        string path = "/TaskFiles/" + uploadedFile.FileName;
//        using (var fileStream = new FileStream(_environment.WebRootPath + path, FileMode.Create))
//        {
//            await uploadedFile.CopyToAsync(fileStream);
//        }
//        message = "Файл загружен";
//    }
//    else
//    {
//        message = "Параметр был null";
//    }
//    return Ok(message);
//}
//catch(Exception e)
//{
//    return Ok(e.Message);
//}