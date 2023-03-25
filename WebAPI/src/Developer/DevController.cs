using System.Security.Cryptography;
using System.Text;
using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Auth;
using WebAPI.Base.Guard;
using WebAPI.Base.Jwt;
using WebAPI.DTOs;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Route("api/dev")]
    [ApiController]
    public class DevController : Controller
    {
        [Route("exec")]
        [HttpGet]
        public IActionResult ExecTest()
        {
            var password = "testing123";
            return Ok(GetHash(password));
        }

        [Route("token/{id}/{token}")]
        [HttpGet]
        public ActionResult<string> GetDevToken(int id, int role)
        {
            var userJwt = new UserJwt
            {
                UserId = 1,
                Role = (Role)role
            };

            return CustomJwt.GenerateToken<UserJwt>(userJwt);
        }

        [HttpGet("hash/{password}")]
        public ActionResult<string> GetPasswordHash(string password)
        {
            return GetHash(password);
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

