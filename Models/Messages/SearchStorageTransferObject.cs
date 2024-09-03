using Models.Contracts.Storage;

namespace Models.Messages
{
    public sealed class SearchStorageTransferObject
    {
        public IStorage Storage { get; }
        public ConcurrentWrappersCollection Source { get; }

        public SearchStorageTransferObject(IStorage storage, ConcurrentWrappersCollection source)
        {
            Source = source;
            Storage = storage;
        }

    }
}
