using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
            var detail = new OperationDetailDTO();

            if (dto != null)
            {
                var res = await _acountService.CreateTeacherAsync(dto);
                detail = res;
            }
            else
                detail.ErrorMessages = new List<string> { "Ошибка! Значение параметра было нулевым." };

            return Ok(detail);
        }

        [HttpPost]
        [Route("register/student")]
        public async Task<IActionResult> RegisterStudent([FromBody]StudentRegisterDTO dto)
        {
            var test = JsonConvert.SerializeObject(dto);
            var detail = new OperationDetailDTO();

            if (dto != null)
            {
                var res = await _acountService.CreateStudentAsync(dto);
                detail = res;
            }
            else
            {
                detail.ErrorMessages = new List<string> { "Ошибка! Значение параметра было нулевым." };
            }


            return Ok(detail);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody]UserLoginDTO dto)
        {
            OperationDetailDTO<SignInDetailDTO> detailResult;
            var serviceResult = await _acountService.SignInAsync(dto);
            if (!serviceResult.Succeeded)
                return Unauthorized();
            else
            {
                detailResult = new OperationDetailDTO<SignInDetailDTO>
                {
                    Succeeded = true,
                    Data = new SignInDetailDTO
                    {
                        Role = serviceResult.Data.UserRoles.FirstOrDefault().ToLowerInvariant(),
                        IdToken = "server_token"
                    },
                };
                return Ok(detailResult);
            }
        }

        // todo:
        [HttpPost]
        [Route("signout")]
        public async Task<IActionResult> SignOut([FromBody]UserLoginDTO dto)
        {
            OperationDetailDTO<SignInDetailDTO> detailResult;
            var serviceResult = await _acountService.SignInAsync(dto);
            if (!serviceResult.Succeeded)
                return Unauthorized();
            else
                return Ok();
        }
    }
}
