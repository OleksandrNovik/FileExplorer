#nullable enable
using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace FileExplorer.Models
{
    public record DirectoryNavigationModel
    {
        public string Name { get; private set; }
        public string FullPath { get; private set; }
        public string? ParentPath { get; private set; }

        public async Task InitializeDataAsync(string path)
        {
            var folder = await StorageFolder.GetFolderFromPathAsync(path);
            await InitializeDataAsync(folder);
        }

        public async Task InitializeDataAsync(StorageFolder folder)
        {
            Name = folder.DisplayName;
            //TODO: Fix path for special folders
            FullPath = string.IsNullOrEmpty(folder.Path) ? folder.DisplayName : folder.Path;
            var parent = await folder.GetParentAsync();
            ParentPath = parent?.Path;
        }
    }
}