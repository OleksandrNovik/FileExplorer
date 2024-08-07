namespace Models.Contracts
{
    public interface ISystemSearchCatalog<TElement> : ISearchCatalog<TElement>
    {
        public string Name { get; }
        public string Path { get; }
    }
}
