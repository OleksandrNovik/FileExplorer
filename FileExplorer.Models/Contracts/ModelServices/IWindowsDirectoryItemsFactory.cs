using FileExplorer.Models.Storage.Windows;

namespace FileExplorer.Models.Contracts.ModelServices
{
    public interface IWindowsDirectoryItemsFactory
    {
        public DirectoryItemWrapper Create(string path);
    }
}
