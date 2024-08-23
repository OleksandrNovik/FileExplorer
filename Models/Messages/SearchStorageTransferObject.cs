using Models.Contracts.Storage;
using Models.Storage.Windows;

namespace Models.Messages
{
    public sealed class SearchStorageTransferObject
    {
        public IStorage<DirectoryItemWrapper> Storage { get; }
        public ConcurrentWrappersCollection Source { get; }

        public SearchStorageTransferObject(IStorage<DirectoryItemWrapper> storage, ConcurrentWrappersCollection source)
        {
            Source = source;
            Storage = storage;
        }

    }
}
