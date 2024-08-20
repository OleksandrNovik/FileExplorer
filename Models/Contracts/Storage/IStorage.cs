#nullable enable
using System.Collections.Generic;

namespace Models.Contracts.Storage
{
    /// <summary>
    /// Interface that represents basic information for a storage
    /// </summary>
    /// <typeparam name="TElement"> Type of element that is stored </typeparam>
    public interface IStorage<TElement> : ISearchCatalog<TElement>
    {
        /// <summary>
        /// Name of the catalog
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Path that identifies catalog 
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// Parent storage
        /// </summary>
        public IStorage<TElement>? Parent { get; }


        /// <summary>
        /// Enumerates all items in the storage 
        /// </summary>
        public IEnumerable<TElement> EnumerateItems();

        /// <summary>
        /// Enumerates sub-storages of storage
        /// </summary>
        public IEnumerable<IStorage<TElement>> EnumerateSubDirectories();

    }
}
