#nullable enable
using Models.Storage.Abstractions;
using System;

namespace Models.Storage.Additional.Properties
{
    /// <summary>
    /// Contains basic information about directory item
    /// </summary>
    /// <param name="name"> Name of directory item </param>
    /// <param name="path"> Path to the directory item </param>
    public sealed class DirectoryItemBasicProperties(string name, string path) : BasicStorageItemProperties(name, path)
    {
        /// <summary>
        /// Last time directory item was modified
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// When directory item was created 
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// Size of directory item
        /// </summary>
        public ByteSize? Size { get; set; }

        public bool HasSize => Size is not null;
    }
}
