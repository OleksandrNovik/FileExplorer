using System.Collections.Generic;
using DirectoryItemWrapper = Models.Storage.Windows.DirectoryItemWrapper;

namespace FileExplorer.Core.Contracts
{
    public interface IDirectoryRouteService
    {
        /// <summary>
        /// Joins together all path parts to form full path that will be navigated later
        /// </summary>
        /// <param name="pathParts"> Parts of the path, that are formed from names of each directory in the path </param>
        /// <returns> Full path ready to be navigated</returns>
        public string CreatePathFrom(IEnumerable<string> pathParts);

        /// <summary>
        /// Extracts from path each name for a directory
        /// </summary>
        /// <param name="route"> Route that we need to get each folder name in it </param>
        /// <returns> Enumeration of path parts that can be used to display all folders </returns>
        public IEnumerable<string> ExtractRouteItems(string route);

        public DirectoryItemWrapper UseNavigationRoute(string route);
    }
}
