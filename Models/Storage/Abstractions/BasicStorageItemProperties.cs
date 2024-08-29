using Models.Contracts.Storage;

namespace Models.Storage.Abstractions
{
    public abstract class BasicStorageItemProperties(string name, string path)
        : BaseThumbnailProvider, IBasicStorageItemProperties
    {
        public string Name { get; set; } = name;
        public string Path { get; } = path;
    }
}
