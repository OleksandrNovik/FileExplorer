#nullable enable
using FileExplorer.Core.Contracts.Clipboard;
using FileExplorer.Models.Contracts.ModelServices;
using FileExplorer.Models.Contracts.Storage.Directory;
using FileExplorer.Models.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Windows.ApplicationModel.DataTransfer;
using FormsClipboard = System.Windows.Forms.Clipboard;
using StaticClipboard = FileExplorer.Helpers.Application.StaticClipboard;
using UWPClipboard = Windows.ApplicationModel.DataTransfer.Clipboard;


namespace FileExplorer.Core.Services.Clipboard
{
    /// <summary>
    /// Service that provides necessary functionality to interact with clipboard
    /// </summary>
    public sealed class ClipboardService : IClipboardService
    {
        /// <summary>
        /// Factory to create windows items from paths in the clipboard's file drop list
        /// </summary>
        private readonly IWindowsDirectoryItemsFactory fileFactory;

        /// <inheritdoc />
        public bool HasFiles => FormsClipboard.ContainsFileDropList();

        public ClipboardService(IWindowsDirectoryItemsFactory factory)
        {
            fileFactory = factory;
            UWPClipboard.ContentChanged += OnClipboardContentChanged;
        }

        /// <summary>
        /// Method to handle changes inside the clipboard
        /// and notify listeners if new files has been put into the clipboard
        /// </summary>
        private void OnClipboardContentChanged(object? sender, object e)
        {
            // If some other application set clipboard file drop list 
            if (FormsClipboard.ContainsFileDropList())
            {
                // Notify all the listeners inside this application
                FileDropListChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <inheritdoc />
        public void SetText(string value)
        {
            var data = new DataPackage();
            data.SetText(value);
            UWPClipboard.SetContent(data);
        }

        /// <inheritdoc />
        public void SetFiles(IReadOnlyCollection<IDirectoryItem> files, DragDropEffects requiredOperation)
        {
            var paths = files.Select(file => file.Path)
                                     .ToArray();

            StaticClipboard.SetFileDropList(paths, requiredOperation);
        }

        /// <inheritdoc />
        public ClipboardFileOperation? GetFiles()
        {
            ClipboardFileOperation? operationResult = default;
            var clipboardData = StaticClipboard.GetFileDropList();

            if (clipboardData is not null)
            {
                operationResult = new ClipboardFileOperation
                {
                    DirectoryItems = clipboardData.Value.Files.Select(fileFactory.Create),
                    Operation = clipboardData.Value.Operation
                };

                var parentDirectory = operationResult.DirectoryItems.FirstOrDefault()?.Directory;

                ArgumentNullException.ThrowIfNull(parentDirectory);

                if ((clipboardData.Value.Operation & DragDropEffects.Move) != 0)
                {
                    var arg = new CutOperationData(parentDirectory, clipboardData.Value.Files.ToArray());
                    CutOperationStarted?.Invoke(this, arg);
                }
            }

            return operationResult;
        }

        public void Clear()
        {
            FormsClipboard.Clear();
        }

        /// <inheritdoc />
        public event EventHandler FileDropListChanged;

        public event EventHandler<CutOperationData> CutOperationStarted;
    }
}
