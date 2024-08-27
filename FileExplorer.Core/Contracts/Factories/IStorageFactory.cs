using Models.Contracts.Storage;

namespace FileExplorer.Core.Contracts.Factories
{
    /// <summary>
    /// Factory to create <see cref="IStorage{TElement}"/> items 
    /// </summary>
    /// <typeparam name="TElement"> Type of element in storage </typeparam>
    public interface IStorageFactory<TElement> where TElement : IDirectoryItem
    {

        /// <summary>
        /// Creates <see cref="IStorage{TElement}"/> item from provided key
        /// </summary>
        /// <param name="key"> Key that identifies storage </param>
        /// <returns> Resulting storage </returns>
        public IStorage<TElement> CreateFromKey(string key);
    }
}
