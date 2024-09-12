namespace FileExplorer.Models.Contracts.Storage.Properties
{
    /// <summary>
    /// Basic properties of every storage item
    /// </summary>
    public interface IBasicStorageItemProperties
    {
        /// <summary>
        /// Name of directory item
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Path to a directory item
        /// </summary>
        public string Path { get; }
    }
}
