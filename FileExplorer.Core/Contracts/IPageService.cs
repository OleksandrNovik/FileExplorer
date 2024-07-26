#nullable enable
using FileExplorer.Models;
using Models.StorageWrappers;

namespace FileExplorer.Core.Contracts
{
    public interface IPageService
    {
        public TabModel CreateTabFromDirectory(DirectoryWrapper? dir);
    }
}
