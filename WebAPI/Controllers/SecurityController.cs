using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Auth.Jwt;
using WebAPI.Auth.Services;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/{controller}")]
    public class SecurityController : Controller
    {
        private readonly AssignmentPRNContext _dbContext;
        private readonly IUserContextService _userService;

        public SecurityController(AssignmentPRNContext dbContext, IUserContextService userService)
        {
            _dbContext = dbContext;
            _userService = userService;
        }

        [HttpPost("CreateToken")]
        public IActionResult CreateToken([FromBody] LoginCredentials credentials)
        {
            var user = _dbContext.Users.Where(u => u.Email == credentials.Email).FirstOrDefault();
            if (user == null || user.Password != credentials.Password)
            {
                return Unauthorized();
            }

            var token = CustomJwt.GenerateToken(new { userId = user.Id });
            return Ok(token);
        }

        [HttpGet("Test/GetUser")]
        public User GetUser()
        {
            return _userService.GetUser();
        }
    }
}
