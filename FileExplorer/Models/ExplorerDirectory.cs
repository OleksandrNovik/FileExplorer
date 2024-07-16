using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace FileExplorer.Models
{
    public class ExplorerDirectory : BasicExplorerItem
    {
        public ExplorerDirectory(DirectoryInfo info) : base(info) { }
        public ExplorerDirectory(string path) : base(new DirectoryInfo(path)) { }
        public ExplorerDirectory(StorageFolder folder) : this(folder.Path) { }

        public IEnumerable<ExplorerFile> EnumerateFiles(string pattern = "*", SearchOption option = SearchOption.TopDirectoryOnly)
        {
            return Directory.EnumerateFiles(Path, pattern, option).Select(path => new ExplorerFile(path));
        }

        public IEnumerable<ExplorerDirectory> EnumerateFolders(string pattern = "*", SearchOption option = SearchOption.TopDirectoryOnly)
        {
            return Directory.EnumerateDirectories(Path, pattern, option).Select(path => new ExplorerDirectory(path));
        }

        public IEnumerable<BasicExplorerItem> EnumerateItems(string pattern = "*", SearchOption option = SearchOption.TopDirectoryOnly)
        {
            foreach (var itemPath in Directory.EnumerateFileSystemEntries(Path))
            {
                if (File.Exists(itemPath))
                    yield return new ExplorerFile(itemPath);
                else
                    yield return new ExplorerDirectory(itemPath);
            }
        }

        public override void Copy(string destination)
        {
            throw new NotImplementedException();
        }

        public override void Move(string destination)
        {
            Directory.Move(Path, destination);
        }

        public override void Recycle()
        {
            throw new NotImplementedException();
        }

        public override void Delete()
        {
            Directory.Delete(Path, true);
        }

        private async Task<StorageFolder> AsStorageFolderAsync()
        {
            return await StorageFolder.GetFolderFromPathAsync(Path);
        }
    }
}
