using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;

namespace FileExplorer.Helpers
{
    public static class KnownFoldersHelper
    {
        public static FrozenDictionary<string, StorageFolder> SpecialFolders { get; private set; }

        static KnownFoldersHelper()
        {
            var picturesLibrary = Task.Run(async () => await StorageLibrary.GetLibraryAsync(KnownLibraryId.Pictures));
            var documentLibrary = Task.Run(async () => await StorageLibrary.GetLibraryAsync(KnownLibraryId.Documents));
            var musicLibrary = Task.Run(async () => await StorageLibrary.GetLibraryAsync(KnownLibraryId.Music));
            var videosLibrary = Task.Run(async () => await StorageLibrary.GetLibraryAsync(KnownLibraryId.Videos));



            Task.WaitAll(documentLibrary, picturesLibrary, musicLibrary, videosLibrary);

            SpecialFolders = SpecialFolders = new Dictionary<string, StorageFolder>
            {
                { documentLibrary.Result.SaveFolder.DisplayName, documentLibrary.Result.SaveFolder },
                { picturesLibrary.Result.SaveFolder.DisplayName, picturesLibrary.Result.SaveFolder },
                { musicLibrary.Result.SaveFolder.DisplayName, musicLibrary.Result.SaveFolder },
                { videosLibrary.Result.SaveFolder.DisplayName, videosLibrary.Result.SaveFolder },
                { KnownFolders.CameraRoll.DisplayName, KnownFolders.CameraRoll },
                { KnownFolders.Objects3D.DisplayName, KnownFolders.Objects3D },
                { KnownFolders.Playlists.DisplayName, KnownFolders.Playlists },
                { KnownFolders.RecordedCalls.DisplayName, KnownFolders.RecordedCalls },
                { KnownFolders.SavedPictures.DisplayName, KnownFolders.SavedPictures },
            }.ToFrozenDictionary();
        }
    }
}
