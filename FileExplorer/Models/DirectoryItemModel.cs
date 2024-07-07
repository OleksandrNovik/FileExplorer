#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.ComponentModel;
using System.IO;

namespace FileExplorer.Models
{
    public partial class DirectoryItemModel : ObservableObject, IEditableObject, ICloneable
    {
        private string backUpName;

        [ObservableProperty]
        private string name;
        public string FullPath => FullInfo?.FullName ?? string.Empty;

        [ObservableProperty]
        private FileSystemInfo? fullInfo;
        public bool IsFile { get; }

        [ObservableProperty]
        private bool isRenamed;


        private DirectoryItemModel(bool isFile)
        {
            IsRenamed = false;
            IsFile = isFile;
        }

        public DirectoryItemModel(string name, bool isFile) : this(isFile)
        {
            Name = name;
        }

        /// <summary>
        /// Constructor for an existing item in directory
        /// Using for viewing existing item in UI
        /// </summary>
        /// <param name="info"> Full information about directory item</param>
        /// <param name="isFile"> Type of item </param>
        public DirectoryItemModel(FileSystemInfo info, bool isFile) : this(isFile)
        {
            name = info.Name;
            FullInfo = info;
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
            var clone = new DirectoryItemModel(Name, IsFile)
            {
                FullInfo = FullInfo
            };
            return clone;
        }
    }
}
