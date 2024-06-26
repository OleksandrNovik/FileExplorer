using FileExplorer.Contracts;
using System.IO;

namespace FileExplorer.Services
{
    public class DirectoryManager : IDirectoryManager
    {
        public bool TryCreateFile(string dirName, string fileName)
        {
            var fullName = dirName + fileName;

            if (File.Exists(fullName))
                return false;

            return true;
        }
    }
}
