using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FAPortal.Client;
using Microsoft.AspNetCore.Mvc;
using FAPortal.Utils;
using FAPortal.DTOs;
using BusinessObject.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FAPortal.Controllers.Admin
{
    [Route("admin")]
    public class AdminController : Controller
    {
        private readonly HttpHandler _handler;

        public AdminController(HttpHandler handler)
        {
            _handler = handler;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ViewClasses() 
        {
            var token = Request.GetTokenFromCookies();

            var response = await _handler.GetAsync("class", token);
            try
            {
                response = response.EnsureSuccessStatusCode();
            }catch
            {
                // Request failed
                var content = await response.Content.ReadFromJsonAsync<ErrorMessage>();
                if (content == null)
                {
                    ViewData["Error"] = "Login Failed";
                }
                else
                {
                    ViewData["Error"] = content.Message;
                }

                return Redirect("admin");
            }

            var classes = await response.Content.ReadFromJsonAsync<List<Class>>();
            if(classes == null)
            {
                return View(new List<Class>());
            }

            return View(classes);
        }

        [HttpGet]
        public async Task<IActionResult> ViewUsers()
        {
            var token = Request.GetTokenFromCookies();

            var response = await _handler.GetAsync("user", token);
            try
            {
                response = response.EnsureSuccessStatusCode();
            }
            catch
            {
                // Request failed
                var content = await response.Content.ReadFromJsonAsync<ErrorMessage>();
                if (content == null)
                {
                    ViewData["Error"] = "Login Failed";
                }
                else
                {
                    ViewData["Error"] = content.Message;
                }

                return Redirect("admin");
            }

            var users = await response.Content.ReadFromJsonAsync<List<User>>();
            if (users == null)
            {
                return View(new List<User>());
            }

            return View(users);
        }
    }
}

