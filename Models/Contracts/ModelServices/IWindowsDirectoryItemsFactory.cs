using Models.Storage.Windows;

namespace Models.Contracts.ModelServices
{
    public interface IWindowsDirectoryItemsFactory
    {
        public DirectoryItemWrapper Create(string path);
    }
}
