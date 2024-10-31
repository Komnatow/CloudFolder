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

    public ActionResult FolderView(string subfolderAdress)
    {
        var global = _global.Value;
        if(subfolderAdress != global.adminFolder ||(subfolderAdress == global.adminFolder && global.role == "admin"))
        {
            ViewBag.current_path = subfolderAdress;
            global.currentFolder = subfolderAdress;
            ViewBag.files = DirectoryOperations.ListDirectoryFiles(ViewBag.current_path);
            ViewBag.directories = DirectoryOperations.ListDirectorySubdirectories(ViewBag.current_path);
            return View();
        }
        else
        {
            return Forbid();
        }
    }

    public ActionResult CreateFolder()
    {
        var global = _global.Value;
        if(global.role == "admin")
        {
            ViewBag.CurrentPath = global.currentFolder;
            return View(new FolderModel());
        }
        else
        {
            return Forbid();
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
        if (model.FolderName.Length >0 && !Directory.Exists(fullPath))
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