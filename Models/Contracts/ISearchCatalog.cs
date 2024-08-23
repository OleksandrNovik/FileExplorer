using Models.Storage.Additional;
using System.Threading.Tasks;

namespace Models.Contracts
{
    /// <summary>
    /// Interface to execute search trough item that implements it.
    /// Is used to create abstraction of a search logic, so item that we are searching decides how to search in itself
    /// </summary>
    /// <typeparam name="TElement"> Type of search element </typeparam>
    public interface ISearchCatalog<TElement>
    {
        /// <summary>
        /// An asynchronous search through current catalog with provided options.
        /// </summary>
        /// <param name="searchOptions"> Provided options for this search </param>
        public Task SearchAsync(SearchOptions searchOptions);
    }
}
