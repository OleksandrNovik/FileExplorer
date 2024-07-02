using System.IO;

namespace FileExplorer.Models
{
    public record DirectoryNavigationModel
    {
        public string Name { get; }
        public string FullPath { get; }
        public bool HasParent { get; }

        public DirectoryNavigationModel(string path)
        {
            var dirInfo = new DirectoryInfo(path);
            Name = dirInfo.Name;
            FullPath = path;
            HasParent = dirInfo.Parent != null;
        }

        public DirectoryNavigationModel(DirectoryInfo info)
        {
            Name = info.Name;
            FullPath = info.FullName;
            HasParent = info.Parent != null;
        }

    }
}