using System;
using System.IO;

namespace FileExplorer.Models
{
    public sealed class TabModel
    {
        public string Name => TabDirectory.Name;
        public Type TabType { get; }
        public DirectoryInfo TabDirectory { get; }

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
