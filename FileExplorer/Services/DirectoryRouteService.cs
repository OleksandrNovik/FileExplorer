using FileExplorer.Contracts;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileExplorer.Services
{
    public class DirectoryRouteService : IDirectoryRouteService
    {
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
