using FileExplorer.Models.Contracts.ModelServices;
using FileExplorer.Models.Storage.Windows;
using System.IO;

namespace FileExplorer.Models.Storage.Factories
{
    public sealed class WindowsDirectoryItemsFactory : IWindowsDirectoryItemsFactory
    {
        public DirectoryItemWrapper Create(string path)
        {
            DirectoryItemWrapper wrapper;

            if (File.Exists(path))
            {
                wrapper = new FileWrapper(path);
            }
            else if (Directory.Exists(path))
            {
                wrapper = new DirectoryWrapper(path);
            }
            else
            {
                throw new FileNotFoundException($"Provided path is not windows file.", path);
            }

            return wrapper;
        }
    }
}
