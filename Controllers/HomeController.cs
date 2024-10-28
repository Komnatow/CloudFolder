using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CloudFolder.Models;
using CloudFolder.Procedures;

namespace CloudFolder.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        ViewBag.current_path = @"D:\Storage";
        TempData["Current_Path"] = ViewBag.current_path;
        ViewBag.files = Directory_Operations.ListDirectoryFiles(ViewBag.current_path);
        ViewBag.directories = Directory_Operations.ListDirectorySubdirectories(ViewBag.current_path);
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
