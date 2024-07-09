using FileExplorer.Contracts;
using FileExplorer.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace FileExplorer.Services
{
    public class DirectoryManager : IDirectoryManager
    {
        private const string CopiedFilesKey = "Copied";
        private readonly IMemoryCache _cache;
        public bool HasCopiedFiles { get; private set; }
        public StorageFolder CurrentDirectory { get; set; }

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

            while (Path.Exists($@"{CurrentDirectory.Path}\{nameBuilder} ({itemsCounter})"))
            {
                itemsCounter++;
            }

            nameBuilder.Append($" ({itemsCounter})");

            return nameBuilder.ToString();
        }


        public async Task<DirectoryItemModel> CreateAsync(bool isFile)
        {
            IStorageItem item;

            if (isFile)
            {
                item = await CurrentDirectory.CreateFileAsync("New File",
                    CreationCollisionOption.GenerateUniqueName);
            }
            else
            {
                item = await CurrentDirectory.CreateFolderAsync("New Folder",
                    CreationCollisionOption.GenerateUniqueName);
            }
            return new DirectoryItemModel(item);
        }

        public async Task RenameAsync(DirectoryItemModel item)
        {
            ArgumentNullException.ThrowIfNull(item.FullInfo);

            await item.FullInfo.RenameAsync(item.Name, NameCollisionOption.GenerateUniqueName);

            item.Name = item.FullInfo.Name;
        }

        public void CopyToClipboard(IEnumerable<DirectoryItemModel> items)
        {
            _cache.Set(CopiedFilesKey, items);
            HasCopiedFiles = true;
        }

        public IEnumerable<DirectoryItemModel> PasteFromClipboard()
        {
            var copiedItems = _cache.Get<IEnumerable<DirectoryItemModel>>(CopiedFilesKey).ToArray();

            return copiedItems;
        }

        public async Task DeleteAsync(DirectoryItemModel item)
        {
            ArgumentNullException.ThrowIfNull(item.FullInfo);

            await item.FullInfo.DeleteAsync(StorageDeleteOption.PermanentDelete);
        }
    }
}
