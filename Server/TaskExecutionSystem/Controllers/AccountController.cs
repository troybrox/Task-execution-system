using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskExecutionSystem.Models;

namespace TaskExecutionSystem.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {


        // POST api/<controller>
        [HttpPost]
        [Route("api/account/register/teacher")]
        public async Task<IActionResult> RegisterTeacher([FromBody]TeacherRegisterModel model)
        {
            // todo: send model to service
            return null;
        }

        [HttpPost]
        [Route("api/account/register/student")]
        public async Task<IActionResult> RegisterStudent([FromBody]StudentRegisterModel model)
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
