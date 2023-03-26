using System.Diagnostics;
using BusinessObject.Models;
using FAPortal.Utils;
using FAPortal.Utils.Guard;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Base.Guard;

namespace FAPortal.Controllers;

[UseGuard(typeof(RoleGuard))]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Login()
    {
        return Redirect("/auth/login");
    }
}

