using FileExplorer.Models.Contracts;
using FileExplorer.Models.Contracts.Storage.Directory;
using FileExplorer.Models.General;
using System.Threading;

namespace FileExplorer.Models.Storage.Additional
{
    public sealed class SearchOptions
    {
        /// <summary>
        /// Search filter for operation that is required
        /// </summary>
        public SearchFilter Filter { get; }

        /// <summary>
        /// Destination collection
        /// </summary>
        public IEnqueuingCollection<IDirectoryItem> Destination { get; }

        /// <summary>
        /// Cancellation token if needed
        /// </summary>
        public CancellationToken Token { get; set; }


        public SearchOptions(SearchFilter filter, IEnqueuingCollection<IDirectoryItem> destination)
        {
            Filter = filter;
            Destination = destination;
        }

        //public static SearchOptions CreateDefault(SearchFilter filter, IEnqueuingCollection<IDirectoryItem> destination)
        //{
        //    return new SearchOptions(filter, destination)
        //    {
        //        OptimizationsEnabled = false
        //    };
        //}
    }
}
