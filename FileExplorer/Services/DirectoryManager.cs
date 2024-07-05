using FileExplorer.Contracts;
using FileExplorer.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FileExplorer.Services
{
    public class DirectoryManager : IDirectoryManager
    {
        private const string CopiedFilesKey = "Copied";
        private readonly IMemoryCache _cache;
        public bool HasCopiedFiles { get; private set; }
        public DirectoryInfo CurrentDirectory { get; set; }

        public DirectoryManager(IMemoryCache cache)
        {
            _cache = cache;
        }

        ~DirectoryManager()
        {
            _cache.Dispose();
        }
        public string GetDefaultName(string nameTemplate)
        {
            var itemsCounter = 0;
            var nameBuilder = new StringBuilder(nameTemplate);

            while (Path.Exists($@"{CurrentDirectory.FullName}\{nameBuilder} ({itemsCounter})"))
            {
                itemsCounter++;
            }

            nameBuilder.Append($" ({itemsCounter})");

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
            _cache.Set(CopiedFilesKey, items);
            HasCopiedFiles = true;
        }

        public IEnumerable<DirectoryItemModel> PasteFromClipboard()
        {
            var copiedItems = _cache.Get<IEnumerable<DirectoryItemModel>>(CopiedFilesKey).ToArray();

            foreach (var item in copiedItems)
            {
                if (Path.Exists(item.Name))
                {
                    item.Name = GetDefaultName($"{item.Name} Copy");
                }

                Move(item, @$"{CurrentDirectory.FullName}\{item.Name}");
            }

            return copiedItems;
        }

        public void Delete(DirectoryItemModel item)
        {
            if (item.FullInfo == null)
                throw new FileNotFoundException("Cannot delete file or folder that don't exits!");

            item.FullInfo.Delete();
        }
    }
}
