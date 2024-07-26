using FileExplorer.Contracts;
using Models.StorageWrappers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DirectoryItemWrapper = FileExplorer.Models.StorageWrappers.DirectoryItemWrapper;

namespace FileExplorer.Core.Services
{
    public class DirectoryRouteService : IDirectoryRouteService
    {
        public DirectoryItemWrapper UseNavigationRoute(string route)
        {
            DirectoryItemWrapper wrapper;

            if (Path.HasExtension(route))
            {
                wrapper = new FileWrapper(route);
            }
            else
            {
                wrapper = new DirectoryWrapper(route);
            }
            return wrapper;
        }

        public string CreatePathFrom(IEnumerable<string> pathParts)
        {
            return string.Join(Path.DirectorySeparatorChar, pathParts);
        }

        public IEnumerable<string> ExtractRouteItems(string route)
        {
            return route.Split(Path.DirectorySeparatorChar)
                        .Where(s => !string.IsNullOrEmpty(s));
        }
    }
}
