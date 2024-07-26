using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace Helpers.StorageHelpers
{
    public static class KnownFoldersHelper
    {
        public static StorageFolder Documents { get; }
        public static StorageFolder Pictures { get; }
        public static StorageFolder Music { get; }
        public static StorageFolder Videos { get; }

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
        }
    }
}
