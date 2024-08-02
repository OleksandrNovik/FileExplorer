using System.Collections;
using System.Threading;
using System.Threading.Tasks;

namespace Models.Contracts
{
    /// <summary>
    /// Interface to execute search trough item that implements it.
    /// Is used to create abstraction of a search logic, so item that we are searching decides how to search in itself
    /// </summary>
    public interface ISearchable<in TDestination> where TDestination : IList
    {
        /// <summary>
        /// An asynchronous search through current catalog with provided options.
        /// </summary>
        /// <param name="destination"> Destination collection to add found elements </param>
        /// <param name="options"> Search options </param>
        /// <param name="token"> Cancellation token to cancel operation when it is no longer needed </param>
        public Task SearchAsync(TDestination destination, General.SearchOptionsModel options, CancellationToken token);

    }
}
