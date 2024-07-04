using FileExplorer.Contracts;
using FileExplorer.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;

namespace FileExplorer.Services
{
    public class DirectoryManager : IDirectoryManager
    {
        public DirectoryInfo CurrentDirectory { get; set; }

        public string GetDefaultName(bool isFile)
        {
            var itemsCounter = 0;
            var nameBuilder = new StringBuilder("New ");
            nameBuilder.Append(isFile ? "File" : "Folder");

            while (Path.Exists($@"{CurrentDirectory.FullName}\{nameBuilder} {itemsCounter}"))
            {
                itemsCounter++;
            }

            nameBuilder.Append($" {itemsCounter}");

            return nameBuilder.ToString();
        }

        public void Create(DirectoryItemModel item)
        {
            var fullName = $@"{CurrentDirectory.FullName}\{item.Name}";

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
            if (item.FullInfo.FullName == location) return;

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

        public void CopyToClipboard(IEnumerable<DirectoryItemModel> items)
        {
            var pathsCollection = new StringCollection();
            var itemsPaths = items.Select(item => item.FullPath);
            pathsCollection.AddRange(itemsPaths.ToArray());

            //Clipboard.SetFileDropList(pathsCollection);
        }

        public IEnumerable<DirectoryItemModel> PasteFromClipboard()
        {
            throw new NotImplementedException();
        }

        public void Delete(DirectoryItemModel item)
        {
            if (item.FullInfo == null)
                throw new FileNotFoundException("Cannot delete file or folder that don't exits!");

            item.FullInfo.Delete();
        }
    }
}
