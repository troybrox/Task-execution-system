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
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }


        [HttpPost]
        [Route("register/teacher")]
        public async Task<IActionResult> AddTeacherRegRequestAsync(TeacherRegisterDTO dto)
        {
            var test = JsonConvert.SerializeObject(dto);
            var detail = new OperationDetailDTO();
            var res = await _accountService.CreateTeacherRegisterRequestAsync(dto);
            detail = res;

            return Ok(detail);
        }


        [HttpPost]
        [Route("register/student")]
        public async Task<IActionResult> AddStudentRegRequestAsync(StudentRegisterDTO dto)
        {
            var test = JsonConvert.SerializeObject(dto);
            var detail = new OperationDetailDTO();
            var res = await _accountService.CreateStudentRegisterRequestAsync(dto);
            detail = res;
            return Ok(detail);
        }


        // Todo: add tokenGenerator
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> SignInAsync([FromBody]UserLoginDTO dto)
        {
            OperationDetailDTO<SignInDetailDTO> detailResult;
            var serviceResult = await _accountService.SignInAsync(dto);
            if (!serviceResult.Succeeded)
            {
                detailResult = new OperationDetailDTO<SignInDetailDTO> { Succeeded = false, ErrorMessages = serviceResult.ErrorMessages };
            }
                
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
            }
            return Ok(detailResult);
        }


        [HttpGet]
        [Route("filters")]
        public async Task<IActionResult> GetAllStudyFiltersAsync()
        {
            var res = await _accountService.GetAllStudiesAsync();
            return Ok(res);
        }


        [HttpPost]
        [Route("signout")]
        public async Task<IActionResult> SignOutAsync([FromBody]UserLoginDTO dto)
        {
            var res = await _accountService.SignOutAsync();
            return Ok(res);
        }
    }
}


// todo: edit method
//[HttpPost]
//[Route("register_/teacherUser")] // url edited
//public async Task<IActionResult> RegisterTeacherAsync([FromBody]TeacherRegisterDTO dto)
//{
//    var test = JsonConvert.SerializeObject(dto);
//    var detail = new OperationDetailDTO();

//    if (dto != null)
//    {
//        var res = await _accountService.CreateTeacherAsync(dto);
//        detail = res;
//    }
//    else
//        detail.ErrorMessages = new List<string> { "Ошибка! Значение параметра было нулевым." };

//    return Ok(detail);
//}

// todo: edit method