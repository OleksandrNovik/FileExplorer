#nullable enable
using Models.Contracts.Storage.Directory;
using Models.Messages;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FileExplorer.Core.Contracts.Clipboard
{
    /// <summary>
    /// Contract for service that deals with clipboard operations 
    /// </summary>
    public interface IClipboardService
    {
        /// <summary>
        /// True if there are files in clipboard and False if no files are available for copy
        /// </summary>
        public bool HasFiles { get; }

        /// <summary>
        /// Saves files to the clipboard
        /// </summary>
        /// <param name="files"> Files to save to the clipboard </param>
        /// <param name="requiredOperation"> Required operation with files </param>
        public void SetFiles(IReadOnlyCollection<IDirectoryItem> files, DragDropEffects requiredOperation);

        /// <summary>
        /// Gets available files from clipboard's file drop list 
        /// </summary>
        /// <returns> Data about files and operation required </returns>
        public ClipboardFileOperation? GetFiles();

        /// <summary>
        /// Sets provided text to the clipboard
        /// </summary>
        /// <param name="value"> String value to save to the clipboard </param>
        public void SetText(string value);

        /// <summary>
        /// Event to notify listeners that other application changed file drop list
        /// </summary>

        public event EventHandler FileDropListChanged;
    }
}
