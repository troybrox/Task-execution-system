using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskExecutionSystem.BLL.DTO;
using TaskExecutionSystem.BLL.Interfaces;
using Newtonsoft.Json;
using TaskExecutionSystem.Identity.JWT.Interfaces;
using Microsoft.AspNetCore.Http;
using System;

namespace TaskExecutionSystem.Controllers
{
    // контроллер, предоставляющий эндпоинты для рабаты с регистраицей и авторизацией пользователей 
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IJWTTokenGenerator _jwtTokenGenerator;

        public AccountController(IAccountService accountService, IJWTTokenGenerator tokenGenerator)
        {
            _accountService = accountService;
            _jwtTokenGenerator = tokenGenerator;
        }


        [HttpPost]
        [Route("register/teacher")]
        public async Task<IActionResult> AddTeacherRegRequestAsync([FromBody]TeacherRegisterDTO dto)
        {
            var test = JsonConvert.SerializeObject(dto);
            var detail = new OperationDetailDTO();
            var res = await _accountService.CreateTeacherRegisterRequestAsync(dto);
            detail = res;

            return Ok(detail);
        }


        [HttpPost]
        [Route("register/student")]
        public async Task<IActionResult> AddStudentRegRequestAsync([FromBody]StudentRegisterDTO dto)
        {
            var test = JsonConvert.SerializeObject(dto);
            var detail = new OperationDetailDTO();
            var res = await _accountService.CreateStudentRegisterRequestAsync(dto);
            detail = res;
            return Ok(detail);
        }


        // эндроинт принимающий даддные для входа пользователя в систему и отправляющий результат действия 
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> SignInAsync([FromBody]UserLoginDTO dto)
        {
            OperationDetailDTO<SignInDetailDTO> detailResult = new OperationDetailDTO<SignInDetailDTO>();
            var serviceResult = await _accountService.SignInAsync(dto);

            try
            {
                if (!serviceResult.Succeeded)
                {
                    detailResult.ErrorMessages = serviceResult.ErrorMessages;
                    return Ok(detailResult);
                }

                else
                {
                    var userRoles = serviceResult.Data.UserRoles;
                    var tokenResult = _jwtTokenGenerator.Generate(serviceResult.Data.User, userRoles);

                    //HttpContext.Response.Cookies.Append(".AspNetCore.Application.Id",
                    //tokenResult.AccessToken, new CookieOptions { MaxAge = TimeSpan.FromMinutes(60) });
                    //HttpContext.Response.Cookies.Append("token",
                    //    tokenResult.AccessToken, new CookieOptions { MaxAge = TimeSpan.FromMinutes(60) });

                    detailResult = new OperationDetailDTO<SignInDetailDTO>
                    {
                        Succeeded = true,
                        Data = new SignInDetailDTO
                        {
                            Role = serviceResult.Data.UserRoles.FirstOrDefault().ToLowerInvariant(),
                            IdToken = tokenResult.AccessToken
                        },
                    };

                    return Ok(detailResult);
                }
            }
            catch(Exception e)
            {
                detailResult.ErrorMessages.Add("Произошло исключение на сервере при попытке авторизации. " + e.Message);
                return Ok(detailResult);
            }
        }


        [HttpGet]
        [Route("filters")]
        public async Task<IActionResult> GetAllStudyFiltersAsync()
        {
            var res = await _accountService.GetAllStudiesAsync();
            return Ok(res);
        }


        [HttpGet]
        [Route("signout")]
        public async Task<IActionResult> SignOutAsync()
        {
            var res = await _accountService.SignOutAsync();
            return Ok(res);
        }
    }
}
