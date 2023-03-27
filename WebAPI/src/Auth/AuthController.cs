using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Utils.Jwt;
using WebAPI.Auth;
using WebAPI.AutoMapper.Models;
using WebAPI.Base.Guard;
using WebAPI.DTOs;

namespace WebAPI.Controllers
{
    [Route("api/auth")]
	[ApiController]
	[UseGuard(typeof(RoleGuard))]
	public class AuthController
	{
		private readonly AssignmentPRNContext _context;
		private readonly IMapper _mapper;

		public AuthController(AssignmentPRNContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		[Route("login")]
		[HttpPost]
		public ActionResult<string> Login(LoginDTO userInfo)
		{
			// Find user in database
			var foundUser = _context.Users.FirstOrDefault(u => u.Email == userInfo.Email);
			if(foundUser == null)
			{
				return new BadRequestObjectResult(new
				{
					Message = "Wrong email and password combination. Please try again"
				});
			}

			// If not, hash password and compare
			var hashPassword = GetHash(userInfo.Password);
			if(foundUser.Password != hashPassword)
			{
                return new BadRequestObjectResult(new
                {
                    Message = "Wrong email and password combination. Please try again"
                });
            }

			// Return token
			return CustomJwt.GenerateToken<UserJwt>(new UserJwt
			{
				UserId = foundUser.Id,
				Role = foundUser.Role
			});
        }

        [Route("signup")]
		[HttpPost]
		[Roles(Role.Admin)]
		public IActionResult Signup(SignupDTO userInfo)
		{
			var adminSignup = _mapper.Map<AdminSignupDTO>(userInfo);

			// Find user in database
			var foundUser = _context.Users.FirstOrDefault(u => u.Email == adminSignup.Email);
			if(foundUser != null)
			{
				return new BadRequestObjectResult(new
				{
					Message = "This email had been registered with another account"
				});
			}

			// If not, hash password and save into database
			var hashPwd = GetHash(adminSignup.Password);
			_context.Users.Add(new User
			{
				Name = userInfo.Name,
				Email = userInfo.Email,
				Password = hashPwd,
				Role = adminSignup.Role
			});
			_context.SaveChanges();
			return new OkResult();
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

