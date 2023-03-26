using System.Security.Cryptography;
using System.Text;
using BusinessObject.Models;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Mvc;
using Utils.Jwt;
using WebAPI.Auth;
using WebAPI.Base.Guard;
using WebAPI.Base.Jwt;
using WebAPI.DTOs;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Route("api/dev")]
    [ApiController]
    [UseGuard(typeof(AuthGuard))]
    [UseGuard(typeof(RoleGuard))]
    public class DevController : Controller
    {
        private readonly StorageClient _storageClient;

        private const string _bucketName = "prn231assignment.appspot.com";

        public DevController(StorageClient storageClient)
        {
            _storageClient = storageClient;
        }

        [Roles(Role.Admin)]
        [Route("exec")]
        [HttpPost]
        public async Task<IActionResult> ExecTest()
        {
            return Ok("Hello World");
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

        private async void DemoFirebase(IFormFile file)
        {
            var objectName = "uploads/test2";

            // Upload file
            using (var stream = file.OpenReadStream())
            {
                await _storageClient.UploadObjectAsync(_bucketName, objectName, null, stream);
            }

            var url = $"https://storage.googleapis.com/{_bucketName}/{objectName}";

            // Download file
            using (var stream = new MemoryStream())
            {
                _storageClient.DownloadObject(_bucketName, objectName, stream);

                // Set the position of the stream to the beginning
                stream.Seek(0, SeekOrigin.Begin);

                var reader = new StreamReader(stream);
                var content = reader.ReadToEnd();
            }
        }
    }
}

