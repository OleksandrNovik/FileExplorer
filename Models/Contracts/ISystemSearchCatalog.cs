namespace Models.Contracts
{
    /// <summary>
    /// Interface that describes named search catalog with path
    /// </summary>
    /// <typeparam name="TElement"> Search element type </typeparam>
    public interface ISystemSearchCatalog<TElement> : ISearchCatalog<TElement>
    {
        /// <summary>
        /// Name of the catalog
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Path that identifies catalog 
        /// </summary>
        public string Path { get; }
    }
}
