using Microsoft.CodeAnalysis.CSharp.Syntax;
using NEST.Models;

namespace NEST.Utilities.Extension
{
    public static class File
    {
        public static bool CheckFileType(this IFormFile file,string filetype)
        {
            return file.ContentType.Contains(filetype);
        }
        public static bool CheckFileSize(this IFormFile file,int size)
        {
            return file.Length / 1024 > size;
        }
        public static async Task<string> SaveFileAsync(this IFormFile file,string root,string mainpath)
        {
            string uniqueFileName = Guid.NewGuid().ToString() + "_" +file.FileName;
            string path = Path.Combine(root, "assets", "imgs", mainpath, uniqueFileName);
            FileStream stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);
            return uniqueFileName;

        }
        public static void DeleteFile(this IFormFile file, string root, string mainPath, string fileName)
        {
            string path = Path.Combine(root, "assets", "imgs", mainPath, fileName);

            using FileStream stream = new FileStream(path, FileMode.Open);

            if(System.IO.File.Exists(path))
            {
                stream.Close();
                System.IO.File.Delete(path);
            }

        }
    }
}
