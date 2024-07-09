using CommunityToolkit.Mvvm.ComponentModel;
using System;
using Windows.Storage;

namespace FileExplorer.Models
{
    public sealed partial class TabModel : ObservableObject
    {
        public Type TabType { get; }

        [ObservableProperty]
        private StorageFolder tabDirectory;
        public TabNavigationHistoryModel TabHistory { get; }

        public TabModel(StorageFolder directory, Type tabType)
        {
            TabType = tabType;
            TabDirectory = directory;
            TabHistory = new TabNavigationHistoryModel();
        }
    }
}
