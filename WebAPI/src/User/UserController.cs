using AutoMapper;
using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using WebAPI.Auth;
using WebAPI.AutoMapper.Models;
using WebAPI.Base.Guard;
using WebAPI.Base.Jwt;
using WebAPI.DTOs;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [UseGuard(typeof(AuthGuard))]
    [UseGuard(typeof(RoleGuard))]
    public class UserController : Controller
    {
        private readonly AssignmentPRNContext _dbContext;
        private readonly IMapper _mapper;

        public UserController(AssignmentPRNContext context, IMapper mapper)
        {
            _dbContext = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetUserList()
        {
            var userList = _dbContext.Users.ToList();
            return Ok(_mapper.Map<List<UserDto>>(userList));
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUser(int userId)
        {
            var requestedUser = await _dbContext.Users.FindAsync(userId);
            if (requestedUser == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<UserDto>(requestedUser));
        }

        [HttpPost]
        [Roles(Role.Admin)]
        public IActionResult AddUser(UserDto2 userDto2)
        {
            if (_dbContext.Users.Any(u => u.Email == userDto2.Email))
            {
                return Conflict("An user with a similar email already exists. User addition failed.");
            }

            var user = _mapper.Map<User>(userDto2);
            user.Password = GetHash(user.Password);
            _dbContext.Add(user);
            _dbContext.SaveChanges();
            return Ok(_mapper.Map<UserDto2>(user));
        }

        [HttpDelete("{userId}")]
        [Roles(Role.Admin)]
        public async Task<IActionResult> RemoveUser(int userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            _dbContext.Remove(user);
            _dbContext.SaveChanges();
            return Ok(_mapper.Map<UserDto2>(user));
        }

        private static string GetHash(string input)
        {
            using (var algo = SHA256.Create())
            {
                var hash = algo.ComputeHash(Encoding.UTF8.GetBytes(input));
                return Convert.ToBase64String(hash);
            }
        }
    }
}
