using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Media;
using System;

namespace Models
{
    public partial class DirectoryItemAdditionalInfo : ObservableObject
    {
        public DateTime ModifiedDate { get; set; }
        public DateTime CreationTime { get; set; }
        public string FullPath { get; set; }
        public string Name { get; set; }

        [ObservableProperty]
        public string titleInfo;
        public ImageSource Thumbnail { get; set; }

    }
}
