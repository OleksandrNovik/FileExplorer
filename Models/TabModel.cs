using CommunityToolkit.Mvvm.ComponentModel;
using Models;
using Models.StorageWrappers;
using System;

namespace FileExplorer.Models
{
    public sealed partial class TabModel : ObservableObject
    {
        public Type TabType { get; }

        [ObservableProperty]
        private DirectoryWrapper tabDirectory;
        public TabNavigationHistoryModel TabHistory { get; }

        public TabModel(DirectoryWrapper directory, Type tabType)
        {
            TabType = tabType;
            TabDirectory = directory;
            TabHistory = new TabNavigationHistoryModel();
        }
    }
}
