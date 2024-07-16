using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Media;
using System;
using System.Threading.Tasks;

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

        public static async Task<FileInfoModel> InitializeAsync(DirectoryItemWrapper directoryItem)
        {
            await Task.Delay(100);
            //TODO: Fix this
            return default;
        }
    }
}
