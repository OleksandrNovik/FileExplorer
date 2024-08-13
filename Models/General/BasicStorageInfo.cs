namespace Models.General
{
    /// <summary>
    /// Basic information needed to represent any storage item in os
    /// </summary>
    public abstract class BasicStorageInfo(string name, string path)
    {
        public string Name { get; } = name;
        public string Path { get; } = path;
    }
}
