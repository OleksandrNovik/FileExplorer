#nullable enable
using FileExplorer.Models.Contracts.Storage.Directory;
using FileExplorer.Models.Enums;
using System.Collections.Generic;
using System.IO;

namespace FileExplorer.Models.Contracts.Storage
{

    /// <summary>
    /// Interface created to represent storage of <see cref="IDirectoryItem"/>
    /// </summary>
    public interface IStorage : ISearchCatalog<IDirectoryItem>
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
        /// Directory storage
        /// </summary>
        public IStorage? Parent { get; }

        /// <summary>
        /// Content type is used to identify what kind of page is suitable to show storage
        /// </summary>
        public StorageContentType ContentType { get; }


        /// <summary>
        /// Enumerates all items in the storage 
        /// </summary>
        /// <param name="rejectedAttributes"> Attributes that should be skipped (by default none files are skipped) </param>
        public IEnumerable<IDirectoryItem> EnumerateItems(FileAttributes rejectedAttributes = 0);

        /// <summary>
        /// Enumerates sub-storages of storage
        /// </summary>
        public IEnumerable<IStorage> EnumerateSubDirectories();
    }
}
