using Models.Storage.Abstractions;
using Models.Storage.Drives;

namespace Models.Storage.Additional.Properties
{
    /// <summary>
    /// Contains basic information about drive
    /// </summary>
    public sealed class DriveBasicProperties : BasicStorageItemProperties
    {
        public DriveBasicProperties(string name, string path)
        {
            Name = name;
            Path = path;
        }

        /// <summary>
        /// Information about drive's space
        /// </summary>
        public DriveSpaceInfo SpaceInfo { get; set; }

        /// <summary>
        /// Type of drive in string
        /// </summary>
        public string DriveType { get; set; }

        /// <summary>
        /// File system format of drive
        /// </summary>
        public string DriveFormat { get; set; }

    }
}
