using BusinessObject.Models;
using FAPortal.Client;
using FAPortal.DTOs;
using FAPortal.Utils;
using FAPortal.Utils.Guard;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Base.Guard;

namespace FAPortal.Controllers
{
    [UseGuard(typeof(RoleGuard))]
    public class ClassController : Controller
    {
        private readonly HttpHandler _handler;
        public ClassController(HttpHandler handler)
        {
            _handler = handler;
        }

        [Roles(Role.Student, Role.Teacher, Role.Admin)]
        public async Task<IActionResult> Index()
        {
            var response = await _handler.GetAsync("class", Request.GetTokenFromCookies());
            if (!response.IsSuccessStatusCode)
            {
                return Redirect("Home");
            }

            var classes = await response.Content.ReadFromJsonAsync<List<ClassDto>>();
            var user = Request.GetUserFromToken();
            var userClasses = classes.Where(c => c.Members.Any(m => m.Id == user.UserId)).ToList();
            if (user.Role == Role.Admin)
            {
                ViewData["IsAdmin"] = true;
            }
            return View(userClasses);
        }
    }
}
