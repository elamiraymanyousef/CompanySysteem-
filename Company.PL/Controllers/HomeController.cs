using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Company.PL.Models;
using Microsoft.AspNetCore.Authorization;

namespace Company.PL.Controllers;
//[AllowAnonymous]//Allow for all any requist can acss this  this controller
[Authorize]//this allow for that Authorize only 
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

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
