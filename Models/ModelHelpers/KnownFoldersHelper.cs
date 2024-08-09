using Models.StorageWrappers;
using Windows.Storage;

namespace Models.ModelHelpers
{
    public static class KnownFoldersHelper
    {
        public static DirectoryWrapper Documents { get; }
        public static DirectoryWrapper Pictures { get; }
        public static DirectoryWrapper Music { get; }
        public static DirectoryWrapper Videos { get; }
        public static DirectoryWrapper Downloads { get; }
        public static DirectoryWrapper Desktop { get; }

        static KnownFoldersHelper()
        {
            var currentUserPaths = UserDataPaths.GetDefault();

            Documents = new DirectoryWrapper(currentUserPaths.Documents);
            Pictures = new DirectoryWrapper(currentUserPaths.Pictures);
            Music = new DirectoryWrapper(currentUserPaths.Music);
            Videos = new DirectoryWrapper(currentUserPaths.Videos);
            Downloads = new DirectoryWrapper(currentUserPaths.Downloads);
            Desktop = new DirectoryWrapper(currentUserPaths.Desktop);
        }
    }
}
