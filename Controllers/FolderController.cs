using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CloudFolder.Models;
using CloudFolder.Procedures;

namespace CloudFolder.Controllers;

public class FolderController : Controller
{

    public ActionResult SubfolderView()
    {
        ViewBag.CurrentPath = TempData["Current_Path"];
        TempData["Current_Path"] = ViewBag.CurrentPath;
        return View(new FolderModel());
    }

    public ActionResult CreateFolder()
    {
        ViewBag.CurrentPath = TempData["Current_Path"];
        TempData["Current_Path"] = ViewBag.CurrentPath;
        return View(new FolderModel());
    }

    [HttpPost]
    public ActionResult CreateNewFolder(FolderModel model)
    {
        // Określ ścieżkę, w której chcesz utworzyć folder
        string fullPath = Path.Combine(model.CurrentPath, model.FolderName);
        var forbidden = new [] {@"/", @"<", @">", @":", "\"", @"|", @"?", @"*"};

        // Sprawdź, czy folder już istnieje
        if (model.FolderName.Length >0 && !Directory.Exists(fullPath))
        {
            Directory.CreateDirectory(fullPath);
            return RedirectToAction("Index", "Home");
        }
        else
        {
            TempData["Current_Path"] = model.CurrentPath;
            return RedirectToAction("CreateFolder", "Folder");
        }
    }
}