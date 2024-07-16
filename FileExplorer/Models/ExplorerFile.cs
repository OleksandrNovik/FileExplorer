using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace FileExplorer.Models
{
    public class ExplorerFile : BasicExplorerItem
    {
        public ExplorerFile(FileInfo info) : base(info) { }

        public ExplorerFile(string path) : base(new FileInfo(path)) { }

        public override void Copy(string destination)
        {
            File.Copy(Path, destination);
        }

        public override void Move(string destination)
        {
            File.Move(Path, destination);
        }

        public override void Recycle()
        {
            throw new NotImplementedException();
        }

        public override void Delete()
        {
            File.Delete(Path);
        }

        private async Task<StorageFile> AsStorageFileAsync()
        {
            return await StorageFile.GetFileFromPathAsync(Path);
        }
    }
}
