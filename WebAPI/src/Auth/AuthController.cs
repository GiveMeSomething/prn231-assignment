using System;
using System.Security.Cryptography;
using System.Text;
using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Auth;
using WebAPI.Base.Jwt;
using WebAPI.DTOs;
using WebAPI.DTOs.Auth;
using WebAPI.Services;

namespace WebAPI.Controllers
{
	[Route("api/auth")]
	[ApiController]
	public class AuthController
	{
		private readonly AssignmentPRNContext _context;

		public AuthController(AssignmentPRNContext context, IUserService userService)
		{
			_context = context;
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
			// Find user in database

			// Return error if existed

			// If not, hash password and save into database

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

