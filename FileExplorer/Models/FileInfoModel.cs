using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Media;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace FileExplorer.Models
{
    public class FileInfoModel : ObservableObject
    {
        public DateTimeOffset ModifiedDate { get; set; }
        public DateTimeOffset CreationTime { get; set; }
        public string FullPath { get; set; }
        public string Name { get; set; }
        public string TitleInfo { get; set; }
        public ImageSource Thumbnail { get; set; }

        public static async Task<FileInfoModel> InitializeAsync(DirectoryItemModel directoryItem)
        {
            var model = new FileInfoModel
            {
                ModifiedDate = directoryItem.FullInfo.DateCreated,
                FullPath = directoryItem.FullInfo.Path,
                Thumbnail = directoryItem.Thumbnail
            };

            var properties = await directoryItem.FullInfo.GetBasicPropertiesAsync();
            model.ModifiedDate = properties.DateModified;

            var storageItemProps = directoryItem.FullInfo as IStorageItemProperties;
            ArgumentNullException.ThrowIfNull(storageItemProps);

            model.Name = storageItemProps.DisplayName;

            if (directoryItem.FullInfo is StorageFile file)
            {
                model.TitleInfo = $"{file.FileType} {file.DisplayType}";
            }
            else
            {
                var fileCount = Directory.EnumerateFiles(model.FullPath, "*", SearchOption.AllDirectories)
                                         .Count();
                var folderCount = Directory.EnumerateDirectories(model.FullPath, "*", SearchOption.AllDirectories)
                                           .Count();
                model.TitleInfo = $"Files: {fileCount}, Folders: {folderCount}";
            }
            return model;
        }
    }
}
