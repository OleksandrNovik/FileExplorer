#nullable enable
using Models.Storage.Windows;
using DirectoryItemWrapper = Models.Storage.Windows.DirectoryItemWrapper;

namespace Models.General
{
    public sealed class DirectoryNavigationInfo : BasicStorageInfo
    {
        public string? ParentPath { get; }
        public CachedSearchResult<DirectoryItemWrapper>? Cache { get; set; }

        public DirectoryNavigationInfo(CachedSearchResult<DirectoryItemWrapper> cachedSearchResult)
            : base(cachedSearchResult.RootCatalog.Name, cachedSearchResult.RootCatalog.Path)
        {
            Cache = cachedSearchResult;
        }

        public DirectoryNavigationInfo(string path) : this(new DirectoryWrapper(path)) { }

        public DirectoryNavigationInfo(DirectoryWrapper folder)
            : base(folder.Name, folder.Path)
        {
            var parent = folder.GetParentDirectory();
            ParentPath = parent?.Path;
        }
    }
}