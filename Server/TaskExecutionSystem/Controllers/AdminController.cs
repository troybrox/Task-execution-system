using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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


    // TODO: test accept methods
    

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

        [HttpGet("exist_teachers")]
        public async Task<IActionResult> GetExistTeachersAsync()
        {
            var res = await _adminService.GetExistTeachersAsync();
            return Ok(res);
        }

        [HttpGet("exist_students")]
        public async Task<IActionResult> GetExistStudentsAsync()
        {
            var res = await _adminService.GetExistStudentsAsync();
            return Ok(res);
        }

        [HttpGet("reg_teachers")]
        public async Task<IActionResult> GetRegTeachersAsync()
        {
            var res = await _adminService.GetTeacherRegisterRequestsAsync();
            return Ok(res);
        }

        [HttpGet("reg_students")]
        public async Task<IActionResult> GetRegStudentsAsync()
        {
            var res = await _adminService.GetStudentRegisterRequestsAsync();
            return Ok(res);
        }



        [HttpPost]
        [Route("reg_teachers_filtered")]
        public async Task<IActionResult> GetFilteredRegTeachersAsync([FromBody]FilterModelDTO model)
        {
            var res = await _adminService.GetTeacherRegisterRequestsAsync(model.Filters);
            return Ok(res);
        }

        [HttpPost]
        [Route("reg_students_filtered")]
        public async Task<IActionResult> GetFilteredRegStudentsAsync([FromBody]FilterModelDTO model)
        {
            var res = await _adminService.GetStudentRegisterRequestsAsync(model.Filters);
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
            var result = await _adminService.RejectTeacherRequstsAsync(idList);
            return Ok(result);
        }

        [HttpPost("delete_reg_students")]
        public async Task<IActionResult> DeleteStudentRegsAsync([FromBody]int[] idList)
        {
            var result = await _adminService.RejectStudentRequstsAsync(idList);
            return Ok(result);
        }

        [HttpPost("delete_exist_teachers")]
        public async Task<IActionResult> DeleteTeachersAsync([FromBody]int[] idList)
        {
            await _adminService.DeleteExistTeachersAsync(idList);
            return Ok();
        }

        [HttpPost("delete_exist_students")]
        public async Task<IActionResult> DeleteStudentsAsync([FromBody]int[] idList)
        {
            await _adminService.DeleteExistStudentsAsync(idList);
            return Ok();
        }
    }
}
