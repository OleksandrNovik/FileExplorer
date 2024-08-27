using System.ComponentModel;

namespace Models.Contracts.Storage
{
    /// <summary>
    /// Contract for any directory item that contains logic to rename and locate item
    /// </summary>
    public interface IRenameableObject : IEditableObject
    {
        /// <summary>
        /// Name of directory item
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Path to a directory item
        /// </summary>
        public string Path { get; }

        public bool IsRenamed { get; }

        /// <summary>
        /// Renames item using <see cref="Name"/> property
        /// </summary>
        public void Rename();
    }
}
