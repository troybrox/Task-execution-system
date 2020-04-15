using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskExecutionSystem.BLL.DTO;
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


    [Route("api/[controller]")]
    public class AdminCintroller : Controller
    {

        [HttpPost]
        [Route("reg_students")]
        public async Task<IActionResult> Login([FromBody] dto)
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


        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
