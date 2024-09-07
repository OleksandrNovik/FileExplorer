using System.ComponentModel;
using Models.Contracts.Storage.Properties;

namespace Models.Contracts.Storage
{
    /// <summary>
    /// Contract for any directory item that contains logic to rename and locate item
    /// </summary>
    public interface IRenameableObject : IBasicStorageItemProperties, IEditableObject
    {
        public bool IsRenamed { get; set; }

        /// <summary>
        /// Property that shows if we can rename item, or it is read only
        /// </summary>
        public bool CanRename { get; }

        /// <summary>
        /// Renames item using <see cref="Name"/> property
        /// </summary>
        public void Rename();
    }
}
