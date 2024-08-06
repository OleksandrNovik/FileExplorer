namespace Models.Contracts
{
    public interface ISystemSearchCatalog<TElement> : ICachingSearchCatalog<TElement>
    {
        public string Name { get; }
        public string Path { get; }
    }
}
