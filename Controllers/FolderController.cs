using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CloudFolder.Models;
using Microsoft.Extensions.Options;
using CloudFolder.Procedures;

namespace CloudFolder.Controllers;

public class FolderController : Controller
{
    private IOptions<GlobalVariablesOptions> _global;

    public FolderController(IOptions<GlobalVariablesOptions> global)
    {
        _global = global;
    }

    [HttpGet]
    public ActionResult FolderView(string subfolderAdress)
    {
        if (Request.Headers.TryGetValue("role", out var roleHeader))
        {
            string role = roleHeader.FirstOrDefault();
            var global = _global.Value;
            if(subfolderAdress != global.adminFolder ||(subfolderAdress == global.adminFolder && role == "admin"))
            {
                ViewBag.current_path = subfolderAdress;
                global.currentFolder = subfolderAdress;
                ViewBag.role = role;
                ViewBag.files = DirectoryOperations.ListDirectoryFiles(ViewBag.current_path);
                ViewBag.directories = DirectoryOperations.ListDirectorySubdirectories(ViewBag.current_path);
                return View();
            }
            else
            {
                return StatusCode(403, "You do not have permission to access this resource.");
            }
        }
        else
        {
            return StatusCode(500, "Server communications error.");
        }
    }

    [HttpGet]
    public ActionResult CreateFolder()
    {
        if (Request.Headers.TryGetValue("role", out var roleHeader))
        {
            string role = roleHeader.FirstOrDefault();
            var global = _global.Value;
            if(role == "admin")
            {
                ViewBag.CurrentPath = global.currentFolder;
                return View(new FolderModel());
            }
            else
            {
                return StatusCode(403, "You do not have permission to access this resource.");
            }
        }
        else
        {
            return StatusCode(500, "Server communications error.");
        }
    }

    [HttpPost]
    public ActionResult CreateNewFolder(FolderModel model)
    {
        var global = _global.Value;
        // Określ ścieżkę, w której chcesz utworzyć folder
        string fullPath = Path.Combine(model.CurrentPath, model.FolderName);
        var forbidden = new [] {@"/", @"<", @">", @":", "\"", @"|", @"?", @"*"};

        // Sprawdź, czy folder już istnieje
        if (model.FolderName.Length >0 && !Directory.Exists(fullPath) && !forbidden.Any(c => model.FolderName.Contains(c)))
        {
            Directory.CreateDirectory(fullPath);
            if (model.CurrentPath != global.InitialFolder)
            {
                return RedirectToAction("FolderView", "Folder", new { subfolderAdress = model.CurrentPath } );
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        else
        {
            return RedirectToAction("CreateFolder", "Folder");
        }
    }
}