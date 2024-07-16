#nullable enable
using FileExplorer.Models;

namespace FileExplorer.Contracts
{
    public interface IPageService
    {
        public TabModel CreateTabFromDirectory(DirectoryWrapper? dir);
    }
}
