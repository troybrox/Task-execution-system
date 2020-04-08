using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskExecutionSystem.Models;
using TaskExecutionSystem.BLL.DTO;
using TaskExecutionSystem.BLL.Interfaces;
using Newtonsoft.Json;

namespace TaskExecutionSystem.Controllers
{
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _acountService;

        public AccountController(IAccountService accountService)
        {
            _acountService = accountService;
        }

        // POST api/<controller>
        [HttpPost]
        [Route("register/teacher")]
        public async Task<IActionResult> RegisterTeacher([FromBody]TeacherRegisterDTO dto)
        {
            var test = JsonConvert.SerializeObject(dto);
            Console.WriteLine(test);

            var detail = new OperationDetailDTO();

            if (dto != null)
            {
                var res = await _acountService.CreateTeacherAsync(dto);
                detail = res;
            }
            else
                detail.ErrorMessages.Add("Ошибка! Значение аргументы было нулевым");

            return Ok(detail);
        }

        [HttpPost]
        [Route("api/account/register/student")]
        public async Task<IActionResult> RegisterStudent([FromBody]StudentRegisterDTO dto)
        {
            // todo: send model to service
            return null;
        }

        [HttpPost]
        [Route("api/account/login")]
        public async Task<IActionResult> Login([FromBody]LoginModel model)
        {
            // todo: send model to service
            return null;
        }
    }
}
