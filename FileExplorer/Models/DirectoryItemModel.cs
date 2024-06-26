﻿#nullable enable
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.IO;

namespace FileExplorer.Models
{
    public partial class DirectoryItemModel : ObservableObject
    {
        [ObservableProperty]
        private string name;

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

    }
}
