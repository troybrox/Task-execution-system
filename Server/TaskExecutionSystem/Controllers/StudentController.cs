using System;
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
        // api/student/profile
        // api/student/profile/update [POST]
        // api/student/profile/updatepassword

        // api/student/task/filters
        // api/student/tasks
        // api/student/solution/add 
        // api/student/solution/add/file
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

        // отправить данные профиля
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfileDataAsync()
        {
            var res = await _studentService.GetProfileDataAsync();
            return Ok(res);
        }

        //отправить данные профиля
        [HttpPost("profile/update")]
        public async Task<IActionResult> UpdateProfileAsync([FromBody]StudentDTO dto)
        {
            var res = await _studentService.UpdateProfileDataAsync(dto);
            return Ok(res);
        }


        // отправить списки объектов, используемых далее для фильтрации получения списка задач 
        [HttpGet("task/filters")]
        public async Task<IActionResult> GetTaskFiltersAsync()
        {
            var res = await _studentService.GetTaskFiltersAsync();
            return Ok(res);
        }

        // отправить офильтрованный список задач
        [HttpPost("tasks")]
        public async Task<IActionResult> GetFilteredTasksAsync([FromBody]FilterDTO[] filters)
        {
            var res = await _studentService.GetTasksFromDBAsync(filters);
            return Ok(res);
        }

        // отправить задачу, полученную по её id
        [HttpGet("task/{id}")]
        public async Task<IActionResult> GetTasksByIDAsync(int id)
        {
            var res = await _studentService.GetTaskByIDAsync(id);
            return Ok(res);
        }


        //  добавление решения, возвращается результат: id добавленного решения в случае успеха
        [HttpPost("solution/add")]
        public async Task<IActionResult> AddSolution([FromBody]SolutionCreateModelDTO dto)
        {
            var res = await _studentService.CreateSolutionAsync(dto);
            return Ok(res);
        }

        //  обновление решения, возвращается результат
        [HttpPost("solution/update")]
        public async Task<IActionResult> UpdateSolution([FromBody]SolutionCreateModelDTO dto)
        {
            var res = await _studentService.UpdateSolutionAsync(dto);
            return Ok(res);
        }


        //  добавления/изменение файла к решению, возвращается результат
        [HttpPost("solution/add/file")]
        public async Task<IActionResult> AddFile()
        {
            string solutionFileLoadPath = _environment.WebRootPath + "\\Files\\" + "\\SolutionFiles\\";
            var detail = new OperationDetailDTO();
            try
            {
                var allForms = Request.Form;

                StringValues solutionIdString;
                var solIdRes = allForms.TryGetValue(allForms.Keys.First(), out solutionIdString);
                var strId = solutionIdString.ToString();
                var solutionID = Convert.ToInt32(strId);

                var file = Request.Form.Files[0];
                if (file != null)
                {
                    string userFileName = file.FileName;
                    string uniqueFileName = System.Guid.NewGuid() + userFileName;
                    OperationDetailDTO fileRes = new OperationDetailDTO();

                    var currentFileRes = await _taskService.GetSolutionFileNameAsync(solutionID);
                    if(currentFileRes.Succeeded)
                    {
                        using (var fileStream = System.IO.File.Create(solutionFileLoadPath + uniqueFileName))
                        {
                            file.CopyTo(fileStream);
                        }

                        if (System.IO.File.Exists(_environment.WebRootPath + currentFileRes.filePath))
                        {
                            System.IO.File.Delete(_environment.WebRootPath + currentFileRes.filePath);
                        }

                        fileRes = await _taskService.UpdateSolutionFileAsync(currentFileRes.fileId, userFileName, uniqueFileName);
                    }

                    else 
                    {
                        using (var fileStream = System.IO.File.Create(solutionFileLoadPath + uniqueFileName))
                        {
                            file.CopyTo(fileStream);
                        }

                        fileRes = await _taskService.AddFileToSolutionAsync(solutionID, userFileName, uniqueFileName);
                    }

                    if (!fileRes.Succeeded)
                    {
                        if (System.IO.File.Exists(solutionFileLoadPath + uniqueFileName))
                        {
                            System.IO.File.Delete(solutionFileLoadPath + uniqueFileName);
                        }

                        detail.ErrorMessages.Add("Не удалось загрузить файл к решению задачи.");
                        detail.ErrorMessages.AddRange(fileRes.ErrorMessages);
                    }
                    else
                    {
                        detail.Succeeded = true;
                    }
                }
                else
                {
                    detail.ErrorMessages.Add("Файл равен null");
                }
                return Ok(detail);
            }
            catch (Exception e)
            {
                detail.ErrorMessages.Add("Ошибка на сервере при загрузке файлов: " + e.Message);
                return Ok(detail);
            }
        }


        // отправить списки предметов, используемых далее для фильтрации получения списка репозиториев 
        [HttpGet("repo/subjects")]
        public async Task<IActionResult> GetRepoFiltersAsync()
        {
            var res = await _studentService.GetRepoFiltersAsync();
            return Ok(res);
        }

        // отправить отфильтрованный список репозиториев
        [HttpPost("repo")]
        public async Task<IActionResult> GetReposAsync([FromBody]FilterDTO[] filters)
        {
            var res = await _studentService.GetRepositoriesFromDBAsync(filters);
            return Ok(res);
        }

        // отправить объект репозитория, полученный по id
        [HttpGet("repo/{id}")]
        public async Task<IActionResult> GetRepoByIdAsync(int id)
        {
            var res = await _studentService.GetRepositoryByID(id);
            return Ok(res);
        }
    }
}
