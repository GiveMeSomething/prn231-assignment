using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FAPortal.DTOs;
using Microsoft.AspNetCore.Mvc;
using FAPortal.Utils;
using System.Text;
using FAPortal.Client;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FAPortal.Controllers
{
    [Route("auth")]
    public class AuthController : Controller
    {
        private readonly HttpHandler _handler;

        public AuthController(HttpHandler handler)
        {
            _handler = handler;
        }

        [Route("login")]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginToServer(LoginCredentials credentials)
        {
            var response = await _handler.PostAsync("auth/login", credentials);
            try
            {
                response = response.EnsureSuccessStatusCode();
            } catch
            {
                // Login failed
                var content = await response.Content.ReadFromJsonAsync<ErrorMessage>();
                if (content == null)
                {
                    ViewData["Error"] = "Login Failed";
                } else
                {
                    ViewData["Error"] = content.Message;
                }
                return View("Login");
            }

            var token = await response.Content.ReadAsStringAsync();

            Response.Cookies.Append("jwtToken", token, new CookieOptions
            {
                Expires = DateTime.Now.AddDays(1),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });

            return Redirect("/");
        }
    }
}

