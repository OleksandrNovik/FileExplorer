using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Media;
using System;

namespace FileExplorer.Models
{
    public partial class DirectoryItemAdditionalInfo : ObservableObject
    {
        public DateTimeOffset ModifiedDate { get; set; }
        public DateTimeOffset CreationTime { get; set; }
        public string FullPath { get; set; }
        public string Name { get; set; }

        [ObservableProperty]
        public string titleInfo;
        public ImageSource Thumbnail { get; set; }

    }
}
