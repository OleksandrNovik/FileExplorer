using Models.Contracts.Storage;

namespace Models.Messages
{
    public sealed class SearchStorageTransferObject
    {
        public IStorage<IDirectoryItem> Storage { get; }
        public ConcurrentWrappersCollection Source { get; }

        public SearchStorageTransferObject(IStorage<IDirectoryItem> storage, ConcurrentWrappersCollection source)
        {
            Source = source;
            Storage = storage;
        }

    }
}
