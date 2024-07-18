#nullable enable
using FileExplorer.Models;
using FileExplorer.Models.StorageWrappers;

namespace FileExplorer.Contracts
{
    public interface IPageService
    {
        public TabModel CreateTabFromDirectory(DirectoryWrapper? dir);
    }
}
