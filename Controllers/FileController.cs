using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CloudFolder.Models;
using CloudFolder.Procedures;

namespace CloudFolder.Controllers;

public class FileController : Controller
{
    public ActionResult FileView(string fileToDetail)
    {
        FileInfo oFileInfo = new FileInfo(fileToDetail);
        ViewBag.isPicture = false;
        ViewBag.isText = false;
        if(oFileInfo.Extension == ".jpg" || oFileInfo.Extension == ".jpeg" || oFileInfo.Extension == ".png" || oFileInfo.Extension == ".webp" )
        {
            ViewBag.isPicture = true;
        }
        if(oFileInfo.Extension == ".txt")
        {
            Console.WriteLine("Weszło do if od tekstu.");
            ViewBag.isPicture = true;
            string fileContent;
            using (var reader = new StreamReader(fileToDetail))
            {
                fileContent = reader.ReadToEnd();
            }

            ViewBag.FileContent = fileContent;
        }
        ViewBag.fileName = oFileInfo.Name;
        DateTime dtCreationTime = oFileInfo.CreationTime;
        ViewBag.fileDate = dtCreationTime.ToString();
        ViewBag.fileExtension = oFileInfo.Extension;
        ViewBag.fileSize = oFileInfo.Length.ToString();
        ViewBag.FileFullName = oFileInfo.FullName;
        ViewBag.CurrentPath = TempData["Current_Path"];
        TempData["Current_Path"] = ViewBag.CurrentPath;
        return View();
    }

    public ActionResult Upload()
    {
        ViewBag.CurrentPath = TempData["Current_Path"];
        TempData["Current_Path"] = ViewBag.CurrentPath;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        Console.WriteLine("Weszło do skryptu Upload.");
        ViewBag.CurrentPath = TempData["Current_Path"];
        TempData["Current_Path"] = ViewBag.CurrentPath;
        if (file == null || file.Length == 0)
        {
            ViewBag.Message = "No file selected";
            return RedirectToAction("Upload", "File");
        }

        // Ustal ścieżkę zapisu pliku na serwerze
        var filePath = Path.Combine(ViewBag.CurrentPath, file.FileName);

        // Zapisywanie pliku
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        ViewBag.Message = "File uploaded successfully";
        return RedirectToAction("Index", "Home");
    }
}