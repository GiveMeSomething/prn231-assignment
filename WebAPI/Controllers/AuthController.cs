using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Auth.Jwt;
using WebAPI.DTOs.Auth;

namespace WebAPI.Controllers
{
	[Route("api/auth")]
	[ApiController]
	public class AuthController
	{
		// TODO: Fix this to our DbContext later
		private readonly DbContext _context;

		public AuthController(DbContext context)
		{
			_context = context;
		}

		[Route("login")]
		[HttpPost]
		public async Task<ActionResult<string>> Login(LoginDTO userInfo)
		{
			// Find user in database

			// Return error if non-existent

			// If not, hash password and compare

			// Return error if wrong password

			// Return token
			var token = CustomJwt.GenerateToken(new
			{
				Email = "testing@test.com",
				FullName = "Hoang Tien Minh"
			});
			return token;
        }

        [Route("signup")]
		[HttpPost]
		public async Task<IActionResult> Signup(SignupDTO userInfo)
		{
            // Find user in database

            // Return error if existed

            // If not, hash password and save into database

            return new OkResult();
        }

        private static string GetHash(string input)
		{
			using(var algo = SHA256.Create())
			{
				var hash = algo.ComputeHash(Encoding.UTF8.GetBytes(input));
				return Encoding.UTF8.GetString(hash);
			}
		}
	}
}

