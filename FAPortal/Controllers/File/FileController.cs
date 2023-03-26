using BusinessObject.Models;
using FAPortal.Client;
using FAPortal.DTOs;
using FAPortal.Utils;
using FAPortal.Utils.Guard;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using WebAPI.Base.Guard;

namespace FAPortal.Controllers
{
    [UseGuard(typeof(RoleGuard))]
    public class FileController : Controller
    {
        private readonly HttpHandler _handler;
        public FileController(HttpHandler handler)
        {
            _handler = handler;
        }

        [Roles(Role.Student, Role.Teacher, Role.Admin)]
        public async Task<IActionResult> Index(int classId)
        {
            var response = await _handler.GetAsync($"resource/class/{classId}", Request.GetTokenFromCookies());
            if (!response.IsSuccessStatusCode)
            {
                return Redirect("Home");
            }

            var files = await response.Content.ReadFromJsonAsync<List<ClassFileDto>>();
            var user = Request.GetUserFromToken();
            ViewData["Role"] = user.Role.ToString().ToLowerInvariant();
            return View(files);
        }

        public async Task<IActionResult> Download(int fileId)
        {
            var response = await _handler.GetAsync($"resource/{fileId}", Request.GetTokenFromCookies());
            if (!response.IsSuccessStatusCode)
            {
                return Redirect("");
            }

            var stream = await response.Content.ReadAsStreamAsync();
            var headers = response.Content.Headers;
            return File(stream, headers.ContentType.MediaType, headers.ContentDisposition.FileName);
        }

        public async Task<IActionResult> Delete(int fileId)
        {
            var response = await _handler.GetAsync($"resource/{fileId}", Request.GetTokenFromCookies());

            var files = await response.Content.ReadFromJsonAsync<List<ClassFileDto>>();
            var user = Request.GetUserFromToken();
            ViewData["Role"] = user.Role.ToString().ToLowerInvariant();
            return View(files);
        }

        public async Task<IActionResult> Upload(int classId, IFormFile file, string fileName)
        {
            HttpContent fileStreamContent = new StreamContent(file.OpenReadStream());
            var response = await _handler.PostAsync($"resource/{classId}", fileStreamContent, Request.GetTokenFromCookies());

            var files = await response.Content.ReadFromJsonAsync<List<ClassFileDto>>();
            var user = Request.GetUserFromToken();
            ViewData["Role"] = user.Role.ToString().ToLowerInvariant();
            return View(files);
        }
    }
}
