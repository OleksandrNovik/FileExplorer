using Models.Contracts.Storage;

namespace FileExplorer.Core.Contracts.Factories
{
    /// <summary>
    /// Factory to create <see cref="IStorage"/> items 
    /// </summary>
    public interface IStorageFactory
    {

        /// <summary>
        /// Creates <see cref="IStorage"/> item from provided key
        /// </summary>
        /// <param name="key"> Key that identifies storage </param>
        /// <returns> Resulting storage </returns>
        public IStorage CreateFromKey(string key);
    }
}
