using Models.Contracts.Storage;
using System.Collections.ObjectModel;

namespace Models.General
{
    public sealed class StoragePageCollections
    {
        public ObservableCollection<IDirectoryItem> SelectedItems { get; set; }
        public ConcurrentWrappersCollection Items { get; set; }
    }
}
