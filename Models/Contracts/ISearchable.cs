using FileExplorer.Models;
using System.Linq;

namespace Models.Contracts
{
    /// <summary>
    /// Interface to execute search trough item that implements it.
    /// Is used to create abstraction of a search logic, so item that we are searching decides how to search in itself
    /// </summary>
    public interface ISearchable<T>
    {
        /// <summary>
        /// Initiates search with provided options
        /// </summary>
        /// <param name="options"> SearchParallel options that needs to be applied for a current search </param>
        /// <returns> Enumeration of items that have been found </returns>
        public ParallelQuery<T> SearchParallel(SearchOptionsModel options);
    }
}
