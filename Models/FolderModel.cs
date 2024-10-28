using System.ComponentModel.DataAnnotations;

namespace CloudFolder.Models;

public class FolderModel
{
    [Required]
    public string FolderName { get; set; } = "new folder";

    [Required]
    public string CurrentPath { get; set; } = @"D:\Storage";
}