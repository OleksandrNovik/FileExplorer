#nullable enable
using Models.Storage.Windows;
using DirectoryItemWrapper = Models.Storage.Windows.DirectoryItemWrapper;

namespace Models.General
{
    public sealed class DirectoryNavigationInfo
    {
        public string Name { get; }
        public string? Path { get; }
        public string? ParentPath { get; }
        public CachedSearchResult<DirectoryItemWrapper>? Cache { get; set; }

        public DirectoryNavigationInfo(CachedSearchResult<DirectoryItemWrapper> cachedSearchResult)
        {
            Cache = cachedSearchResult;
        }

        public DirectoryNavigationInfo(string name, string? path)
        {
            Name = name;
            Path = path;
        }

        public DirectoryNavigationInfo(string path) : this(new DirectoryWrapper(path)) { }

        public DirectoryNavigationInfo(DirectoryWrapper folder) : this(folder.Name, folder.Path)
        {
            var parent = folder.GetParentDirectory();
            ParentPath = parent?.Path;
        }
    }
}