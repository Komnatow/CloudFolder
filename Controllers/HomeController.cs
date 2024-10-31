using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CloudFolder.Models;
using CloudFolder.Procedures;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.IO;

namespace CloudFolder.Controllers;

public class HomeController : Controller
{
    private IOptions<GlobalVariablesOptions> _global;

    public HomeController(IOptions<GlobalVariablesOptions> global)
    {
        _global = global;
    }

    public IActionResult Index()
    {
        var global = _global.Value;
        ViewBag.current_path = global.InitialFolder;
        ViewBag.role = global.role;
        ViewBag.files = DirectoryOperations.ListDirectoryFiles(ViewBag.current_path);
        ViewBag.directories = DirectoryOperations.ListDirectorySubdirectories(ViewBag.current_path);
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
