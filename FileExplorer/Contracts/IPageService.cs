using FileExplorer.Models;
using Windows.Storage;

namespace FileExplorer.Contracts
{
    public interface IPageService
    {
        public TabModel CreateTabFromDirectory(StorageFolder dir);
    }
}
