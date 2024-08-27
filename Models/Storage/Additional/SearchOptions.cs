using Models.Contracts;
using Models.Contracts.Storage;
using Models.General;
using System.Threading;

namespace Models.Storage.Additional
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

        /// <summary>
        /// Is optimizations enabled for a search
        /// </summary>
        public bool OptimizationsEnabled { get; set; }

        private int maxDirectoriesPerThread;

        /// <summary>
        /// If number of directories for search is greater new thread will be created to search
        /// </summary>
        public int MaxDirectoriesPerThread
        {
            get => maxDirectoriesPerThread;
            set
            {
                maxDirectoriesPerThread = value;

                if (maxDirectoriesPerThread > 0)
                {
                    OptimizationsEnabled = true;
                }
            }
        }
        public SearchOptions(SearchFilter filter, IEnqueuingCollection<IDirectoryItem> destination)
        {
            Filter = filter;
            Destination = destination;
        }

        public static SearchOptions CreateDefault(SearchFilter filter, IEnqueuingCollection<IDirectoryItem> destination)
        {
            return new SearchOptions(filter, destination)
            {
                OptimizationsEnabled = false
            };
        }
    }
}
