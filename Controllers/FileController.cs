using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CloudFolder.Models;
using Microsoft.Extensions.Options;
using CloudFolder.Procedures;
using System.IO;

namespace CloudFolder.Controllers;

public class FileController : Controller
{
    private IOptions<GlobalVariablesOptions> _global;

    public FileController(IOptions<GlobalVariablesOptions> global)
    {
        _global = global;
    }
    public ActionResult FileView(string fileToDetail)
    {
        var global = _global.Value;
        string fileContent = "";
        FileInfo oFileInfo = new FileInfo(fileToDetail);
        ViewBag.isPicture = false;
        ViewBag.isText = false;
        ViewBag.isFilm = false;
        if(oFileInfo.Extension == ".jpg" || oFileInfo.Extension == ".jpeg" || oFileInfo.Extension == ".png" || oFileInfo.Extension == ".webp" )
        {
            ViewBag.isPicture = true;
        }
        if(oFileInfo.Extension == ".txt")
        {
            ViewBag.isText = true;
            using (var reader = new StreamReader(fileToDetail))
            {
                fileContent = reader.ReadToEnd();
            }

            ViewBag.FileContent = fileContent;
        }
        if(oFileInfo.Extension == ".mp4" || oFileInfo.Extension == ".avi" || oFileInfo.Extension == ".mov" )
        {
            ViewBag.isFilm = true;
        }
        ViewBag.fileName = oFileInfo.Name;
        DateTime dtCreationTime = oFileInfo.CreationTime;
        ViewBag.fileDate = dtCreationTime.ToString();
        ViewBag.fileExtension = oFileInfo.Extension;
        ViewBag.fileSize = oFileInfo.Length.ToString();
        ViewBag.fileFullName = oFileInfo.FullName;
        ViewBag.CurrentPath = global.currentFolder;
        return View();
    }

    public IActionResult GetImage(string fileFullName)
    {

        if (!System.IO.File.Exists(fileFullName))
        {
            return NotFound();
        }

        var image = System.IO.File.ReadAllBytes(fileFullName);
        return File(image, "image/jpeg");
    }

    [HttpGet]
    public IActionResult GetVideo(string fileFullName)
    {
        if (!System.IO.File.Exists(fileFullName))
        {
            return NotFound();
        }
        FileInfo oFileInfo = new FileInfo(fileFullName);

        var stream = new FileStream(fileFullName, FileMode.Open, FileAccess.Read);
        var contentType = "video/.mp4"; 
        return new FileStreamResult(stream, contentType);
    }

    public ActionResult Upload()
    {
        var global = _global.Value;
        ViewBag.CurrentPath = global.currentFolder;
        return View();
    }

    [HttpGet]
    public FileResult DownloadFile(string fileFullName, string fileName)
    {
        byte[] bytes = System.IO.File.ReadAllBytes(fileFullName);
        return File(bytes, "Application/octec-stream", fileName);
    }

    [HttpPost]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        var global = _global.Value;
        ViewBag.CurrentPath = global.currentFolder;
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

        if (ViewBag.CurrentPath != global.InitialFolder)
        {
            return RedirectToAction("FolderView", "Folder", new { subfolderAdress = ViewBag.CurrentPath } );
        }
        else
        {
            return RedirectToAction("Index", "Home");
        }
    }
}