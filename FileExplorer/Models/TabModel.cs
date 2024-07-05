using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.IO;

namespace FileExplorer.Models
{
    public sealed partial class TabModel : ObservableObject
    {
        public string Name => TabDirectory.Name;
        public Type TabType { get; }

        [ObservableProperty]
        private DirectoryInfo tabDirectory;

        public TabModel(DirectoryInfo directory, Type tabType)
        {
            TabType = tabType;
            TabDirectory = directory;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
