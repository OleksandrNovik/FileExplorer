using FileExplorer.Models.Storage.Abstractions;

namespace FileExplorer.Models.Contracts.Storage.Properties
{
    /// <summary>
    /// Provides basic properties about item
    /// </summary>
    public interface IBasicPropertiesProvider
    {
        /// <summary>
        /// Method that provides item's basic properties
        /// </summary>
        public StorageItemProperties GetBasicProperties();
    }
}
