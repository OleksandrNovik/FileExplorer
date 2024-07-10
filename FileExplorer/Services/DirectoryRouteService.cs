using FileExplorer.Contracts;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace FileExplorer.Services
{
    public class DirectoryRouteService : IDirectoryRouteService
    {
        private static readonly FrozenDictionary<string, StorageFolder> SpecialNameToRoute =
            new Dictionary<string, StorageFolder>
            {
                { KnownFolders.DocumentsLibrary.DisplayName, KnownFolders.DocumentsLibrary },
                { KnownFolders.CameraRoll.DisplayName, KnownFolders.CameraRoll },
                { KnownFolders.MediaServerDevices.DisplayName, KnownFolders.MediaServerDevices },
                { KnownFolders.MusicLibrary.DisplayName, KnownFolders.MusicLibrary },
                { KnownFolders.Objects3D.DisplayName, KnownFolders.Objects3D },
                { KnownFolders.PicturesLibrary.DisplayName, KnownFolders.PicturesLibrary },
                { KnownFolders.Playlists.DisplayName, KnownFolders.Playlists },
                { KnownFolders.RecordedCalls.DisplayName, KnownFolders.RecordedCalls },
                { KnownFolders.SavedPictures.DisplayName, KnownFolders.SavedPictures },
                { KnownFolders.VideosLibrary.DisplayName, KnownFolders.VideosLibrary },

            }.ToFrozenDictionary();

        public bool IsSpecialRoute(string route) => SpecialNameToRoute.ContainsKey(route);

        public async Task<StorageFolder> UseNavigationRouteAsync(string route)
        {
            StorageFolder folder;

            if (IsSpecialRoute(route))
            {
                folder = SpecialNameToRoute[route];
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
