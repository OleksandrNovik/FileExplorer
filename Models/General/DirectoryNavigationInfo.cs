#nullable enable
using Models.StorageWrappers;

namespace Models.General
{
    public sealed record DirectoryNavigationInfo
    {
        public string Name { get; }
        public string FullPath { get; }
        public string? ParentPath { get; }
        public CachedSearchResult<DirectoryItemWrapper>? Cache { get; }

        public DirectoryNavigationInfo(CachedSearchResult<DirectoryItemWrapper> cachedSearchResult)
        {
            Name = cachedSearchResult.RootCatalog.SearchCatalog.Name;
            FullPath = cachedSearchResult.RootCatalog.SearchCatalog.Path;
            Cache = cachedSearchResult;
        }

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