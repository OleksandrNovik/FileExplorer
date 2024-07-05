using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.IO;

namespace FileExplorer.Models
{
    public sealed partial class TabModel : ObservableObject
    {
        public Type TabType { get; }

        [ObservableProperty]
        private DirectoryInfo tabDirectory;
        public TabNavigationHistoryModel TabHistory { get; }

        public TabModel(DirectoryInfo directory, Type tabType)
        {
            TabType = tabType;
            TabDirectory = directory;
            TabHistory = new TabNavigationHistoryModel();
        }
    }
}
