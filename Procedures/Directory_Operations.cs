using System.IO;

namespace CloudFolder.Procedures;

public class Directory_Operations
{
    public static void CheckDirectory(string folderPath)
    {
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
    }

    public static List<string> ListDirectoryFiles(string folderPath)
    {
        List<string> files_list = new List<string>();
        if (Directory.Exists(folderPath))
        {
            string[] files = Directory.GetFiles(folderPath);
            if(files.Length > 0)
            {
                files_list.AddRange(files);
            }
        }
        return files_list;
    }

    public static List<string> ListDirectorySubdirectories(string folderPath)
    {
        List<string> files_list = new List<string>();
        if (Directory.Exists(folderPath))
        {
            string[] directories = Directory.GetDirectories(folderPath);
            if(directories.Length > 0)
            {
                files_list.AddRange(directories);
            }
        }
        return files_list;
    }
}