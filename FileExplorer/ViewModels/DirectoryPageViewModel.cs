using CommunityToolkit.Mvvm.ComponentModel;
using FileExplorer.Contracts;
using System.Collections.ObjectModel;
using System.IO;

namespace FileExplorer.ViewModels
{
    public partial class DirectoryPageViewModel : ObservableRecipient
    {
        private readonly IPicturesService _picturesService;

        [ObservableProperty]
        private DirectoryInfo currentDirectory = new DirectoryInfo(@"C:\Windows");

        [ObservableProperty]
        private ObservableCollection<FileSystemInfo> directoryItems;


        public DirectoryPageViewModel()
        {
            directoryItems = new ObservableCollection<FileSystemInfo>(currentDirectory.EnumerateFileSystemInfos());
        }

    }
}
