using System.ComponentModel;

namespace Models.Contracts.Storage
{
    /// <summary>
    /// Contract for any directory item that contains logic to rename and locate item
    /// </summary>
    public interface IRenameableObject : IBasicStorageItemProperties, IEditableObject
    {
        public bool IsRenamed { get; set; }

        /// <summary>
        /// Renames item using <see cref="Name"/> property
        /// </summary>
        public void Rename();
    }
}
