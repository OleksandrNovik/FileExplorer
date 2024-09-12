using FileExplorer.Helpers.General;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using FormsClipboard = System.Windows.Forms.Clipboard;

namespace FileExplorer.Helpers.Application
{
    /// <summary>
    /// Static clipboard manager that helps form and send data to the clipboard
    /// </summary>
    public static class StaticClipboard
    {
        public const string PreferredDropEffect = "Preferred DropEffect";

        /// <summary>
        /// Writes paths to the files into the clipboard with preferred drop operation
        /// </summary>
        /// <param name="paths"> Paths to the files </param>
        /// <param name="dropEffect"> Preferred drop effect for a file operation </param>
        public static void SetFileDropList(ICollection<string> paths, DragDropEffects dropEffect)
        {
            var dataObject = new DataObject();

            var pathsCollection = new StringCollection();
            pathsCollection.AddRange(paths);

            dataObject.SetFileDropList(pathsCollection);

            using var ms = BitConverter.GetBytes((int)dropEffect)
                                 .WriteMemoryStream();

            dataObject.SetData(PreferredDropEffect, ms);

            FormsClipboard.SetDataObject(dataObject, true);
        }

        /// <summary>
        /// Gets files from clipboard and required operation if there is any files
        /// </summary>
        /// <returns> Tuple of enumerations of paths to the files and required operation </returns>
        public static (IEnumerable<string> Files, DragDropEffects Operation)? GetFileDropList()
        {
            if (FormsClipboard.ContainsFileDropList())
            {
                var dataObject = FormsClipboard.GetDataObject();

                if (dataObject is not null && dataObject.GetDataPresent(PreferredDropEffect))
                {
                    var operationMs = (MemoryStream)dataObject.GetData(PreferredDropEffect);
                    var bytes = operationMs.ReadAll();
                    var operation = (DragDropEffects)bytes[0];

                    var strings = FormsClipboard.GetFileDropList();

                    return (strings.OfType<string>(), operation);
                }
            }

            return null;
        }
    }
}
