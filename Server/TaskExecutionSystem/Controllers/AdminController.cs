using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static TaskExecutionSystem.Identity.Contracts.IdentityPolicyContract;
using TaskExecutionSystem.BLL.DTO;
using TaskExecutionSystem.BLL.DTO.Filters;
using TaskExecutionSystem.BLL.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaskExecutionSystem.Controllers
{
    //api/admin/filters

    //api/admin/reg_students
    //api/admin/reg_teachers
    //api/admin/exist_students
    //api/admin/exist_teachers

    //api/admin/add_reg_students
    //api/admin/add_reg_teachers
    //api/admin/delete_reg_students
    //api/admin/delete_reg_teachers

    //api/admin/delete_exist_students
    //api/admin/delete_exist_teachers

    //api/admin/delete_group

    // контроллер, предоставляющий эндпоинты для рабаты пользователя с ролью администратора 
    [Authorize(AdministratorPolicy)]
    [Route("api/[controller]")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly IAccountService _accountService;

        public AdminController(IAdminService adminService, IAccountService accountService)
        {
            _adminService = adminService;
            _accountService = accountService;
        }

        // GET: api/<controller>
        [HttpGet("filters")]
        public async Task<IActionResult> GetFilterListsAsync()
        {
            var resultList = await _adminService.GetAllStudyFiletrsAsync();
            return Ok(resultList);
        }


        [HttpGet("filters/{userType}")]
        public async Task<IActionResult> GetFilterByTypeAsync(string userType)
        {
            var res = await _adminService.GetAllStudyFiltersAsync(userType);
            return Ok(res);
        }


        [HttpGet("exist_teachers_")]
        public async Task<IActionResult> GetExistTeachersAsync_()
        {
            var res = await _adminService.GetExistTeachersAsync();
            return Ok(res);
        }

        [HttpGet("exist_students_")]
        public async Task<IActionResult> GetExistStudentsAsync_()
        {
            var res = await _adminService.GetExistStudentsAsync();
            return Ok(res);
        }

        [HttpGet("reg_teachers_")]
        public async Task<IActionResult> GetRegTeachersAsync_()
        {
            var res = await _adminService.GetTeacherRegisterRequestsAsync();
            return Ok(res);
        }

        [HttpGet("reg_students_")] 
        public async Task<IActionResult> GetRegStudentsAsync_()
        {
            var res = await _adminService.GetStudentRegisterRequestsAsync();
            return Ok(res);
        }


        [HttpPost("reg_teachers")]
        public async Task<IActionResult> GetRegTeachersAsync([FromBody]FilterDTO[] filters)
        {
            var res = await _adminService.GetTeacherRegisterRequestsAsync(filters);
            return Ok(res);
        }

        [HttpPost("reg_students")]
        public async Task<IActionResult> GetRegStudentsAsync([FromBody]FilterDTO[] filters)
        {
            var res = await _adminService.GetStudentRegisterRequestsAsync(filters);
            return Ok(res);
        }

        [HttpPost("exist_teachers")]
        public async Task<IActionResult> GetExistTeachersAsync([FromBody]FilterDTO[] filters)
        {
            var res = await _adminService.GetExistTeachersAsync(filters);
            return Ok(res);
        }

        [HttpPost("exist_students")]
        public async Task<IActionResult> GetExistStudentsAsync([FromBody]FilterDTO[] filters)
        {
            var res = await _adminService.GetExistStudentsAsync(filters);
            return Ok(res);
        }
        

        [HttpPost("add_reg_teachers")]
        public async Task<IActionResult> AcceptTeacherUsersAsync([FromBody]int[] idList)
        {
            var result = await _adminService.AcceptTeacherRequestsAsync(idList);
            return Ok(result);
        }

        [HttpPost("add_reg_students")]
        public async Task<IActionResult> AcceptStudentUsersAsync([FromBody]int[] idList)
        {
            var result = await _adminService.AcceptStudentRequestsAsync(idList);
            return Ok(result);
        }

        [HttpPost("delete_reg_teachers")]
        public async Task<IActionResult> DeleteTeacherRegsAsync([FromBody]int[] idList)
        {
            var result = await _adminService.RejectTeacherRequestsAsync(idList);
            return Ok(result);
        }

        [HttpPost("delete_reg_students")]
        public async Task<IActionResult> DeleteStudentRegsAsync([FromBody]int[] idList)
        {
            var result = await _adminService.RejectStudentRequestsAsync(idList);
            return Ok(result);
        }

        [HttpPost("delete_exist_teachers")]
        public async Task<IActionResult> DeleteTeachersAsync([FromBody]int[] idList)
        {
            var result = await _adminService.DeleteExistTeachersAsync(idList);
            return Ok(result);
        }

        [HttpPost("delete_exist_students")]
        public async Task<IActionResult> DeleteStudentsAsync([FromBody]int[] idList)
        {
            var result = await _adminService.DeleteExistStudentsAsync(idList);
            return Ok(result);
        }

        [HttpPost("delete_group")]
        public async Task<IActionResult> DeleteGroupAstnc([FromBody]int groupId)
        {
            var result = await _adminService.DeleteGroupAsync(groupId);
            return Ok(result);
        }
    }
}
