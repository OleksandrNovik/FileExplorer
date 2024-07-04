using FileExplorer.Models;
using System.IO;

namespace FileExplorer.Contracts
{
    public interface IPageService
    {
        public TabModel CreateTabFromDirectory(DirectoryInfo dir);
    }
}
