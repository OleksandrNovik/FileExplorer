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

        public bool TryMove(DirectoryItemModel item, string location)
        {
            if (item.FullInfo is null)
                return false;

            if (item.IsFile)
            {
                File.Move(item.FullInfo.FullName, location);
                // Update information about physical item in directory
                item.FullInfo = new FileInfo(location);
            }
            else
            {
                Directory.Move(item.FullInfo.FullName, location);
                item.FullInfo = new DirectoryInfo(location);
            }
            return true;
        }
    }
}
