using FileExplorer.Contracts;
using FileExplorer.Models;
using System;
using System.IO;
using System.Text;

namespace FileExplorer.Services
{
    public class DirectoryManager : IDirectoryManager
    {
        private DirectoryInfo _currentDirectory;

        public DirectoryManager(DirectoryInfo currentDirectory)
        {
            _currentDirectory = currentDirectory;
        }

        public string GetDefaultName(bool isFile)
        {
            var itemsCounter = 0;
            var nameBuilder = new StringBuilder("New ");
            nameBuilder.Append(isFile ? "File" : "Folder");

            while (Path.Exists($@"{_currentDirectory.FullName}\{nameBuilder} {itemsCounter}"))
            {
                itemsCounter++;
            }

            nameBuilder.Append($" {itemsCounter}");

            return nameBuilder.ToString();
        }

        public void Create(DirectoryItemModel item)
        {
            var fullName = $@"{_currentDirectory.FullName}\{item.Name}";

            if (item.IsFile)
            {
                using (File.Create(fullName))
                {
                    item.FullInfo = new FileInfo(fullName);
                }
            }
            else
            {
                Directory.CreateDirectory(fullName);
                item.FullInfo = new DirectoryInfo(fullName);
            }
        }

        public void Move(DirectoryItemModel item, string location)
        {
            if (item.FullInfo is null)
                throw new ArgumentNullException(nameof(item.FullInfo), "Wrapper is empty. Consider creating physical object first.");

            // File has not the save location as provided
            if (item.FullInfo.FullName != location)
            {
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
            }
        }

        public bool TryMove(DirectoryItemModel item, string location)
        {
            if (item.FullInfo is null)
                return false;

            if (item.FullInfo.FullName == location)
                return true;

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

        public bool TryDelete(DirectoryItemModel item)
        {
            if (item.FullInfo == null)
                throw new FileNotFoundException("Cannot delete file or folder that don't exits!");
            try
            {
                item.FullInfo.Delete();
            }
            catch (IOException)
            {
                return false;
            }
            return true;
        }

        public void MoveToNewDirectory(DirectoryInfo dir)
        {
            _currentDirectory = dir;
        }
    }
}
