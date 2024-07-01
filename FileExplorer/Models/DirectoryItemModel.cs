#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.ComponentModel;
using System.IO;

namespace FileExplorer.Models
{
    public partial class DirectoryItemModel : ObservableObject, IEditableObject
    {
        private string backUpName;

        [ObservableProperty]
        private string name;

        public string FullPath { get; private set; }

        [ObservableProperty]
        private FileSystemInfo? fullInfo;
        public bool IsFile { get; }

        [ObservableProperty]
        private bool isRenamed;

        /// <summary>
        /// Constructor for an empty item with only name and isFile
        /// Using for a new item creation
        /// </summary>
        /// <param name="isFile"> Type of item </param>
        public DirectoryItemModel(bool isFile)
        {
            Name = Guid.NewGuid().ToString();
            IsFile = isFile;
            isRenamed = true;
        }

        public DirectoryItemModel(string name, bool isFile)
        {
            Name = name;
            IsFile = isFile;
            isRenamed = true;
        }

        /// <summary>
        /// Constructor for an existing item in directory
        /// Using for viewing existing item in UI
        /// </summary>
        /// <param name="info"> Full information about directory item</param>
        /// <param name="isFile"> Type of item </param>
        public DirectoryItemModel(FileSystemInfo info, bool isFile)
        {
            name = info.Name;
            FullInfo = info;
            IsFile = isFile;
            isRenamed = false;
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

        partial void OnFullInfoChanged(FileSystemInfo? value)
        {
            if (value != null)
            {
                FullPath = value.FullName;
            }
        }
    }
}
