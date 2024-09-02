using Models.Contracts.Storage;
using Models.Storage.Windows;
using System.Collections.Generic;
using Windows.Storage;

namespace Models.ModelHelpers
{
    public static class KnownFoldersHelper
    {
        public static IReadOnlyList<DirectoryWrapper> Libraries { get; }

        public static IDirectory RecentDirectory { get; }

        static KnownFoldersHelper()
        {
            var currentUserPaths = UserDataPaths.GetDefault();

            Libraries = [
                new DirectoryWrapper(currentUserPaths.Desktop),
                new DirectoryWrapper(currentUserPaths.Downloads),
                new DirectoryWrapper(currentUserPaths.Documents),
                new DirectoryWrapper(currentUserPaths.Pictures),
                new DirectoryWrapper(currentUserPaths.Music),
                new DirectoryWrapper(currentUserPaths.Videos),
            ];

            RecentDirectory = new DirectoryWrapper(currentUserPaths.Recent);
        }
    }
}
