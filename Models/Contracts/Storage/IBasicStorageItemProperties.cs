namespace Models.Contracts.Storage
{
    /// <summary>
    /// Basic properties of every storage item
    /// </summary>
    public interface IBasicStorageItemProperties : IThumbnailProvider
    {
        /// <summary>
        /// Name of directory item
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Path to a directory item
        /// </summary>
        public string Path { get; }
    }
}
