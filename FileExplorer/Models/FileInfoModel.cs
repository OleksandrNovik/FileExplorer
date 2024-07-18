using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Media;
using System;

namespace FileExplorer.Models
{
    public class FileInfoModel : ObservableObject
    {
        public DateTimeOffset ModifiedDate { get; }
        public DateTimeOffset CreationTime { get; }
        public string FullPath { get; }
        public string Name { get; }
        public string TitleInfo { get; }
        public ImageSource Thumbnail { get; }



    }
}
