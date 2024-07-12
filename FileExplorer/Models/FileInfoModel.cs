using Microsoft.UI.Xaml.Media;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace FileExplorer.Models
{
    public class FileInfoModel
    {
        public DateTimeOffset ModifiedDate { get; set; }
        public DateTimeOffset CreationTime { get; set; }
        public string FullPath { get; set; }
        public string Name { get; set; }
        public string TitleInfo { get; set; }
        public ImageSource Thumbnail { get; set; }
        public bool IsShown { get; set; }

        public async Task InitializeAsync(DirectoryItemModel directoryItem)
        {
            ModifiedDate = directoryItem.FullInfo.DateCreated;
            FullPath = directoryItem.FullInfo.Path;
            Thumbnail = directoryItem.Thumbnail;

            var properties = await directoryItem.FullInfo.GetBasicPropertiesAsync();
            ModifiedDate = properties.DateModified;

            var storageItemProps = directoryItem.FullInfo as IStorageItemProperties;
            ArgumentNullException.ThrowIfNull(storageItemProps);

            Name = storageItemProps.DisplayName;

            if (directoryItem.FullInfo is StorageFile file)
            {
                TitleInfo = $"{file.FileType} {file.DisplayType}";
            }
            else
            {
                TitleInfo = $"Items: {Directory.EnumerateFileSystemEntries(FullPath).Count()}";
            }

            IsShown = true;
        }
    }
}
