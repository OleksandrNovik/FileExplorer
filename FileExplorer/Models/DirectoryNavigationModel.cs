#nullable enable
using System.IO;

namespace FileExplorer.Models
{
    public record DirectoryNavigationModel
    {
        public string Name { get; }
        public string FullPath { get; }
        public string? ParentPath { get; }

        public DirectoryNavigationModel(string path)
        {
            var dirInfo = new DirectoryInfo(path);
            Name = dirInfo.Name;
            FullPath = path;
            ParentPath = dirInfo.Parent?.FullName;
        }

        public DirectoryNavigationModel(DirectoryInfo info)
        {
            Name = info.Name;
            FullPath = info.FullName;
            var p = info.Parent;
            ParentPath = info.Parent?.FullName;
        }

    }
}