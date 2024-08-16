using System.ComponentModel;

namespace Models.Contracts.Storage
{
    public interface IRenameableObject : IEditableObject
    {
        public string Name { get; set; }
        public string Path { get; }
    }
}
