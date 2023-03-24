using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Auth.Decorator;
using WebAPI.Auth.Guards;
using WebAPI.Auth.Jwt;

namespace WebAPI.Controllers
{
    [Route("api/dev")]
    [ApiController]
    [UseGuard(typeof(AuthGuard))]
    [UseGuard(typeof(RoleGuard))]
    public class DevController : Controller
    {
        [Route("exec")]
        [HttpGet]
        [Roles("student", "teacher")]
        public IActionResult ExecTest()
        {
            var testToken = CustomJwt.GenerateToken(new
            {
                Data = "Hello World"
            });

            return Ok(testToken);
        }
    }
}

