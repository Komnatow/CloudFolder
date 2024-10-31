using System.ComponentModel.DataAnnotations;

namespace CloudFolder.Models
{
    public class FileModel
    {
        [Required]
        public string FileName {get; set;}
    }
}