using FileExplorer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;

namespace FileExplorer.Contracts
{
    public interface IDirectoryManager
    {
        public StorageFolder CurrentDirectory { get; set; }
        public bool HasCopiedFiles { get; }

        public Task<DirectoryItemModel> CreateAsync(bool isFile);

        public Task RenameAsync(DirectoryItemModel item);

        public void CopyToClipboard(IEnumerable<DirectoryItemModel> items);

        public IEnumerable<DirectoryItemModel> PasteFromClipboard();

        public Task DeleteAsync(DirectoryItemModel item);

        public string GetDefaultName(string nameTemplate);

    }
}
