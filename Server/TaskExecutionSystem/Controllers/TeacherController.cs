using System;
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
    [Route("api/teacher")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        public class FileModel
        {
            public IFormFile uploadedFile { get; set; }
        }

        private readonly ITaskService _taskService;
        //private readonly ITeacherService _teacherService;
        public static IWebHostEnvironment _environment;

        public TeacherController(ITaskService taskService, IWebHostEnvironment environment)
        {
            _taskService = taskService;
            _environment = environment;
        }

        [HttpPost("add_task_file")]
        public async  Task<IActionResult> AddFileToTaskAsync(FileModel file)
        {
            try
            {
                string message = "";
                if (file.uploadedFile.Length > 0)
                {
                    if(!Directory.Exists(_environment.WebRootPath + "\\TaskFiles\\"))
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
        }

        [HttpPost("add_task")]
        public async Task<IActionResult> AddTaskAsync(TaskCreateModelDTO dto)
        {
            var res = await _taskService.CreateTaskAsync(dto);
            return Ok(res);
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
    }
}