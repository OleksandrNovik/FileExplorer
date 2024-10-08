﻿using FileExplorer.Models.Contracts.Storage.Directory;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FileExplorer.Models.Messages
{
    public sealed class ClipboardFileOperation
    {
        public IEnumerable<IDirectoryItem> DirectoryItems { get; set; }
        public DragDropEffects Operation { get; set; }
    }
}
