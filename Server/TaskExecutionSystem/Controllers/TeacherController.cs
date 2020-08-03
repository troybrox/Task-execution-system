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
        private readonly IRepoService _repoService;
        private readonly ITeacherService _teacherService;
        public static IWebHostEnvironment _environment;
        private readonly IAccountService _accountService;

        public TeacherController(ITaskService taskService, IRepoService repoService, 
            IWebHostEnvironment environment, ITeacherService teacherService, IAccountService accountService)
        {
            _taskService = taskService;
            _repoService = repoService;
            _environment = environment;
            _teacherService = teacherService;
            _accountService = accountService;
        }

        // отправить данные профиля
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfileDataAsync()
        {
            var res = await _teacherService.GetProfileDataAsync();

            //test
            string cookieKey = "myToken";
            CookieOptions cookieOptions = new CookieOptions();
            cookieOptions.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Append(cookieKey, "test_value", cookieOptions);
            //

            return Ok(res);
        }

        // изменить данные профиля, отправить результат
        [HttpPost("profile/update")]
        public async Task<IActionResult> UpdateProfileAsync([FromBody]TeacherDTO dto)
        {
            var res = await _teacherService.UpdateProfileDataAsync(dto);
            return Ok(res);
        }


        [HttpPost("profile/updatepassword")]
        public async Task<IActionResult> UpdatePasswordAsync([FromBody]PasswordUpdateDTO dto)
        {
            var res = await _accountService.UpdatePasswordAsync(dto);
            return Ok(res);
        }

        // отправить данные главной страницы
        [HttpGet("main")]
        public async Task<IActionResult> GetMainPageDataAsync()
        {
            var res = await _teacherService.GetMainDataAsync();
            return Ok(res);
        }

        // отправить списки объектов, используемых далее для фильтрации получения списка задач 
        [HttpGet("task/filters")]
        public async Task<IActionResult> GetTaskFiltersAsync()
        {
            var res = await _teacherService.GetTaskFiltersAsync();
            return Ok(res);
        }

        // отправить офильтрованный список задач
        [HttpPost("task")]
        public async Task<IActionResult> GetFilteredTasksAsync([FromBody]FilterDTO[] filters)
        {
            var res = await _teacherService.GetTasksFromDBAsync(filters);
            return Ok(res);
        }

        // отправить задачу, полученную по её id
        [HttpGet("task/{id}")]
        public async Task<IActionResult> GetTasksByIDAsync(int id)
        {
            var res = await _teacherService.GetTaskByIDAsync(id);
            return Ok(res);
        }

        // отправить фильтры, нужные при создании задачи
        [HttpGet("task/add/filters")]
        public async Task<IActionResult> GetTaskAddingListFiltersAsync()
        {
            var res = await _teacherService.GetAddingTaskFiltersAsync();
            return Ok(res);
        }

        //  добавление задачи, возвращается результат: id добавленной задачи в случае успеха
        [HttpPost("task/add")]
        public async Task<IActionResult> AddTaskAsync([FromBody]TaskCreateModelDTO task = null)
        {
            var res = await _teacherService.CreateNewTaskAsync(task);
            return Ok(res);
        }

        //  обновление задачи, возвращается результат
        [HttpPost("task/update")]
        public async Task<IActionResult> UpdateTaskAsync([FromBody]TaskCreateModelDTO task = null)
        {
            var res = await _teacherService.UpdateTaskAsync(task);
            return Ok(res);
        }

        //  закрытие задачи, возвращается результат
        [HttpGet("task/{id}/close")]
        public async Task<IActionResult> CloseTaskAsync(int id)
        {
            var res = await _teacherService.CloseTaskAsync(id);
            return Ok(res);
        }

        //  добавления/изменение файла к задаче, возвращается результат
        [HttpPost("task/add/file")]
        public async Task<IActionResult> AddFileForTaskAsync()
        {
            string taskFileLoadPath = _environment.WebRootPath + "\\Files\\" + "\\TaskFiles\\";
            var detail = new OperationDetailDTO();
            
            try
            {
                var allForms = Request.Form;
                StringValues taskIdString;
                var taskIdRes = allForms.TryGetValue(allForms.Keys.First(), out taskIdString);
                var strId = taskIdString.ToString();
                var taskID = Convert.ToInt32(strId);

                var file = Request.Form.Files[0];
                if (file != null)
                {
                    string userFileName = file.FileName;
                    string uniqueFileName = System.Guid.NewGuid() + userFileName;
                    OperationDetailDTO fileRes = new OperationDetailDTO();

                    var currentFileRes = await _taskService.GetTaskFileNameAsync(taskID);
                    if (currentFileRes.Succeeded)
                    {
                        using (var fileStream = System.IO.File.Create(taskFileLoadPath + uniqueFileName))
                        {
                            file.CopyTo(fileStream);
                        }

                        if(System.IO.File.Exists(_environment.WebRootPath + currentFileRes.filePath))
                        {
                            System.IO.File.Delete(_environment.WebRootPath + currentFileRes.filePath);
                        }

                        fileRes = await _taskService.UpdateTaskFileAsync(currentFileRes.fileId, userFileName, uniqueFileName);
                    }

                    else
                    {
                        using (var fileStream = System.IO.File.Create(taskFileLoadPath + uniqueFileName))
                        {
                            file.CopyTo(fileStream);
                        }

                        fileRes = await _taskService.AddFileToTaskAsync(taskID, userFileName, uniqueFileName);
                    }


                    if (!fileRes.Succeeded)
                    {
                        if (System.IO.File.Exists(taskFileLoadPath + uniqueFileName))
                        {
                            System.IO.File.Delete(taskFileLoadPath + uniqueFileName);
                        }
                        detail.ErrorMessages.Add("Не удалось загрузить файл к задаче.");
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


        // отправить фильтры, нужные при создании репозитория 
        [HttpGet("repo/add/filters")]
        public async Task<IActionResult> GetRepoAddingFilters()
        {
            var res = await _teacherService.GetRepoCreateSubjectFiltersAsync();
            return Ok(res);
        }


        //  добавление репозитория, возвращается результат: id добавленного репозитория в случае успеха
        [HttpPost("repo/add")]
        public async Task<IActionResult> AddRepoAsync([FromBody]RepositoryCreateModelDTO dto = null)
        {
            var res = await _teacherService.CreateNewRepositoryAsync(dto);
            return Ok(res);
        }

        // отправить списки предметов, используемых далее для фильтрации получения списка репозиториев 
        [HttpGet("repo/subjects")]
        public async Task<IActionResult> GetRepoFiltersAsync()
        {
            var res = await _teacherService.GetRepositoryListFilters();
            return Ok(res);
        }

        // отправить отфильтрованный список репозиториев
        [HttpPost("repo")]
        public async Task<IActionResult> GetReposAsync([FromBody]FilterDTO[] filters)
        {
            var res = await _teacherService.GetRepositoriesFromDBAsync(filters);
            return Ok(res);
        }

        // удаление репозитория, возвращается результат
        [HttpPost("repo/delete")]
        public async Task<IActionResult> DeleteRepoAsync([FromBody]int[] id)
        {
            var detail = new OperationDetailDTO();

            try
            {
                var res = await _teacherService.DeleteRepositoryAsync(id[0]);
                if (res.Succeeded)
                {
                    detail.Succeeded = true;
                    foreach (var path in res.Data)
                    {
                        var filePath = _environment.WebRootPath + path;
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                }
                return Ok(detail);
            }
            catch(Exception e)
            {
                detail.ErrorMessages.Add("Ошибка на сервере при удалении файлов репозитория. Описании ошибки: " + e.Message);
                Debug.WriteLine(e.InnerException);
                Debug.WriteLine("Source :" + e.Source);
                return Ok(detail);
            }
            
        }

        // редактирование репозитория, возвращается результат
        [HttpPost("repo/update")]
        public async Task<IActionResult> UpdateRepoAsync([FromBody]RepositoryCreateModelDTO dto)
        {
            var res = await _teacherService.UpdateRepositoryAsync(dto);
            return Ok(res);
        }


        //  добавление файла к репозиторию, возвращается результат
        [HttpPost("repo/add/file")]
        public async Task<IActionResult> AddFileForRepositoryaAsync()
        {
            string repoFileLoadPath = _environment.WebRootPath + "\\Files\\" + "\\RepoFiles\\";
            var detail = new OperationDetailDTO();

            try
            {
                var allForms = Request.Form;
                StringValues repoIdString;
                var taskIdRes = allForms.TryGetValue(allForms.Keys.First(), out repoIdString);
                var strId = repoIdString.ToString();
                var id = Convert.ToInt32(strId);
                var file = Request.Form.Files[0];
                if (file != null)
                {
                    var fileName = file.FileName;
                    var fileRes = new OperationDetailDTO();

                    var newFileName = System.Guid.NewGuid() + fileName;
                    using (var fileStream = System.IO.File.Create(repoFileLoadPath + newFileName))
                    {
                        file.CopyTo(fileStream);
                    }
                    fileRes = await _repoService.AddFileToRepoAsync(id, fileName, newFileName);

                    if (!fileRes.Succeeded)
                    {
                        detail.ErrorMessages.Add("Не удалось загрузить файл для репозитория.");
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
                Debug.WriteLine(e.InnerException);
                Debug.WriteLine("Source :" + e.Source);
                return Ok(detail);
            }
        }


        // отправить объект репозитория, полученный по id
        [HttpGet("repo/{id}")]
        public async Task<IActionResult> GetRepoByIdAsync(int id)
        {
            var res = await _teacherService.GetRepositoryByID(id);
            return Ok(res);
        }



        // test method-------------
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
