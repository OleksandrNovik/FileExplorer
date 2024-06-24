using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.IO;

namespace FileExplorer.ViewModels
{

    public partial class DirectoryPageViewModel : ObservableRecipient
    {
        [ObservableProperty]
        private DirectoryInfo currentDirectory = new DirectoryInfo(@"D:\Навчальння");

        [ObservableProperty]
        private ObservableCollection<FileSystemInfo> directoryItems;


        public DirectoryPageViewModel()
        {
            directoryItems = new ObservableCollection<FileSystemInfo>(currentDirectory.EnumerateFileSystemInfos());
        }

    }
}
