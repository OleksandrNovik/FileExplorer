#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using FileExplorer.Contracts;
using FileExplorer.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace FileExplorer.ViewModels
{
    public partial class DirectoryPageViewModel : ObservableRecipient
    {
        private readonly IPicturesService _picturesService;

        [ObservableProperty]
        private DirectoryInfo currentDirectory = new DirectoryInfo(@"D:\Навчальння");

        [ObservableProperty]
        private ObservableCollection<DirectoryItemModel> directoryItems;

        public DirectoryPageViewModel(IPicturesService picturesService)
        {
            _picturesService = picturesService;

            var models = CurrentDirectory.GetFileSystemInfos()
                .Select(info => new DirectoryItemModel(info, info is FileInfo));

            directoryItems = new ObservableCollection<DirectoryItemModel>(models);
        }

    }
}
