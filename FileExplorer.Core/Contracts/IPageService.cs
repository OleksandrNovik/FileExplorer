#nullable enable
using Models.StorageWrappers;
using Models.TabRelated;

namespace FileExplorer.Core.Contracts
{
    public interface IPageService
    {
        public TabModel CreateTabFromDirectory(DirectoryWrapper? dir);
    }
}
