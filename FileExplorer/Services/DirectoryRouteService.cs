using FileExplorer.Contracts;
using FileExplorer.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace FileExplorer.Services
{
    public class DirectoryRouteService : IDirectoryRouteService
    {
        public bool IsSpecialRoute(string route) => KnownFoldersHelper.SpecialFolders.ContainsKey(route);

        public async Task<StorageFolder> UseNavigationRouteAsync(string route)
        {
            StorageFolder folder;

            if (IsSpecialRoute(route))
            {
                folder = KnownFoldersHelper.SpecialFolders[route];
            }
            else
            {
                folder = await StorageFolder.GetFolderFromPathAsync(route);
            }
            return folder;
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
