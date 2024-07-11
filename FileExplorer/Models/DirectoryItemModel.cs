#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Media;
using System;
using System.ComponentModel;
using Windows.Storage;

namespace FileExplorer.Models
{
    public partial class DirectoryItemModel : ObservableObject, IEditableObject, ICloneable
    {
        private string backUpName;

        [ObservableProperty]
        private string name;
        public string FullPath => FullInfo?.Path ?? string.Empty;

        [ObservableProperty]
        private IStorageItem? fullInfo;

        [ObservableProperty]
        private bool isRenamed;

        public ImageSource Thumbnail { get; set; }

        public DirectoryItemModel(string name)
        {
            Name = name;
            IsRenamed = false;
        }

        /// <summary>
        /// Constructor for an existing item in directory
        /// Using for viewing existing item in UI
        /// </summary>
        /// <param name="info"> Full information about directory item</param>
        public DirectoryItemModel(IStorageItem info)
        {
            name = info.Name;
            FullInfo = info;
            IsRenamed = false;
        }

        public void BeginEdit()
        {
            if (!IsRenamed)
            {
                backUpName = Name;
                IsRenamed = true;
            }
        }

        public void CancelEdit()
        {
            if (IsRenamed)
            {
                Name = backUpName;
                IsRenamed = false;
            }
        }

        public void EndEdit()
        {
            if (IsRenamed)
            {
                backUpName = Name;
                IsRenamed = false;
            }
        }

        public object Clone()
        {
            var clone = new DirectoryItemModel(Name)
            {
                FullInfo = FullInfo
            };
            return clone;
        }
    }
}
