using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Auth;
using WebAPI.Base.Guard;
using WebAPI.Base.Jwt;

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
        [Roles(Role.Admin, Role.Student, Role.Teacher)]
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

