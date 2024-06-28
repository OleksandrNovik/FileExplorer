using FileExplorer.Contracts;
using FileExplorer.Models;
using System.IO;

namespace FileExplorer.Services
{
    public class DirectoryManager : IDirectoryManager
    {
        public void Create(DirectoryItemModel item, string location)
        {
            var fullName = $@"{location}\{item.Name}";

            if (item.IsFile)
            {
                File.Create(fullName);
                item.FullInfo = new FileInfo(fullName);
            }
            else
            {
                Directory.CreateDirectory(fullName);
                item.FullInfo = new DirectoryInfo(fullName);
            }
        }
        public bool TryCreateFile(DirectoryItemModel item, string location)
        {
            if (File.Exists(location))
            {
                return false;
            }
            Create(item, location);
            return true;
        }

        public bool TryCreateDirectory(DirectoryItemModel item, string location)
        {
            if (Directory.Exists(location))
            {
                return false;
            }
            Create(item, location);
            return true;
        }

        public bool TryMove(DirectoryItemModel item, string location)
        {
            if (item.FullInfo is null)
                return false;


            if (item.FullInfo is FileInfo file)
            {
                file.MoveTo(location);
                return true;
            }
            if (item.FullInfo is DirectoryInfo dir)
            {
                dir.MoveTo(location);
                return true;
            }

            return false;
        }
    }
}
