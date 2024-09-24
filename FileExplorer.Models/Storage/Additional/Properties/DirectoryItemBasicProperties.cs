#nullable enable
using FileExplorer.Models.Storage.Abstractions;
using System;

namespace FileExplorer.Models.Storage.Additional.Properties
{
    /// <summary>
    /// Contains basic information about directory item
    /// </summary>
    public sealed class DirectoryItemBasicProperties : StorageItemProperties
    {
        public DirectoryItemBasicProperties(string name, string path)
        {
            Name = name;
            Path = path;
        }

        /// <summary>
        /// Content type of directory item
        /// </summary>
        public string ContentType { get; init; }

        /// <summary>
        /// Last time directory item was modified
        /// </summary>
        public DateTime ModifiedDate { get; init; }

        /// <summary>
        /// When directory item was created 
        /// </summary>
        public DateTime CreationTime { get; init; }
    }
}
