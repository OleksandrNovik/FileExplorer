using Enums;
using FileExplorer.Contracts;
using FileExplorer.Helpers;
using FileExplorer.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using FileAttributes = System.IO.FileAttributes;

namespace FileExplorer.Services
{
    public class DirectoryManager : IDirectoryManager
    {
        private const string CopiedFilesKey = "Copied";

        private readonly IMemoryCache _cache;
        public StorageFolder CurrentDirectory { get; set; }

        public DirectoryManager(IMemoryCache cache)
        {
            _cache = cache;
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

        public void CopyToClipboard(IEnumerable<DirectoryItemModel> items, CopyOperation operation)
        {
            _cache.Set(CopiedFilesKey, items);

            //If files are cut, set attribute Hidden for each item
            if (operation == CopyOperation.Cut)
            {
                foreach (var item in items)
                {
                    var itemAttributes = File.GetAttributes(item.FullPath);
                    File.SetAttributes(item.FullPath, itemAttributes | FileAttributes.Hidden);
                }
            }
        }

        public async Task<IEnumerable<DirectoryItemModel>> PasteFromClipboard(CopyOperation operation)
        {
            var copiedItems = _cache.Get<IEnumerable<DirectoryItemModel>>(CopiedFilesKey).ToArray();

            var items = copiedItems.Select(model => model.FullInfo).ToImmutableArray();

            await items.PasteAsync(CurrentDirectory, operation);

            return copiedItems;
        }

        private async Task DeleteItemAsync(DirectoryItemModel item, StorageDeleteOption deleteOption = StorageDeleteOption.Default)
        {
            ArgumentNullException.ThrowIfNull(item.FullInfo);

            await item.FullInfo.DeleteAsync(deleteOption);
        }

        public async Task MoveToRecycleBinAsync(DirectoryItemModel item) => await DeleteItemAsync(item);

        public async Task DeleteAsync(DirectoryItemModel item) =>
            await DeleteItemAsync(item, StorageDeleteOption.PermanentDelete);
    }
}
