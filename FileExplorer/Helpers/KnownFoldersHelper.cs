using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;

namespace FileExplorer.Helpers
{
    public static class KnownFoldersHelper
    {
        public static StorageFolder Documents { get; }
        public static StorageFolder Pictures { get; }
        public static StorageFolder Music { get; }
        public static StorageFolder Videos { get; }

        public static FrozenDictionary<string, StorageFolder> SpecialFolders { get; }

        static KnownFoldersHelper()
        {
            var picturesLibrary = Task.Run(async () => await StorageLibrary.GetLibraryAsync(KnownLibraryId.Pictures));
            var documentLibrary = Task.Run(async () => await StorageLibrary.GetLibraryAsync(KnownLibraryId.Documents));
            var musicLibrary = Task.Run(async () => await StorageLibrary.GetLibraryAsync(KnownLibraryId.Music));
            var videosLibrary = Task.Run(async () => await StorageLibrary.GetLibraryAsync(KnownLibraryId.Videos));

            Task.WaitAll(documentLibrary, picturesLibrary, musicLibrary, videosLibrary);

            Documents = documentLibrary.Result.SaveFolder;
            Pictures = picturesLibrary.Result.SaveFolder;
            Music = musicLibrary.Result.SaveFolder;
            Videos = videosLibrary.Result.SaveFolder;

            SpecialFolders = SpecialFolders = new Dictionary<string, StorageFolder>
            {
                { Documents.DisplayName, Documents },
                { Pictures.DisplayName, Pictures },
                { Music.DisplayName, Music },
                { Videos.DisplayName, Videos },
                { KnownFolders.CameraRoll.DisplayName, KnownFolders.CameraRoll },
                { KnownFolders.Objects3D.DisplayName, KnownFolders.Objects3D },
                { KnownFolders.Playlists.DisplayName, KnownFolders.Playlists },
                { KnownFolders.RecordedCalls.DisplayName, KnownFolders.RecordedCalls },
                { KnownFolders.SavedPictures.DisplayName, KnownFolders.SavedPictures },
            }.ToFrozenDictionary();
        }
    }
}
