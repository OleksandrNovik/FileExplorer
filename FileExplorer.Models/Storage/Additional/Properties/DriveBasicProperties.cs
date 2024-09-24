using FileExplorer.Models.Storage.Abstractions;
using FileExplorer.Models.Storage.Drives;

namespace FileExplorer.Models.Storage.Additional.Properties
{
    /// <summary>
    /// Contains basic information about drive
    /// </summary>
    public sealed class DriveBasicProperties : StorageItemProperties
    {
        public DriveBasicProperties(string name, string path)
        {
            Name = name;
            Path = path;
        }

        /// <summary>
        /// Information about drive's space
        /// </summary>
        public DriveSpaceInfo SpaceInfo { get; init; }

        /// <summary>
        /// Type of drive in string
        /// </summary>
        public string DriveType { get; init; }

        /// <summary>
        /// File system format of drive
        /// </summary>
        public string DriveFormat { get; init; }

    }
}
