#nullable enable
using FileExplorer.Models.StorageWrappers;

namespace FileExplorer.Models
{
    public record DirectoryNavigationInfo
    {
        public string Name { get; }
        public string FullPath { get; }
        public string? ParentPath { get; }

        public DirectoryNavigationInfo(string path) : this(new DirectoryWrapper(path)) { }

        public DirectoryNavigationInfo(DirectoryWrapper folder)
        {
            Name = folder.Name;
            FullPath = folder.Path;
            var parent = folder.GetParentDirectory();
            ParentPath = parent?.Path;
        }
    }
}