using Models.Storage.Abstractions;
using Models.Storage.Drives;

namespace Models.Storage.Additional.Properties
{
    /// <summary>
    /// Contains basic information about drive
    /// </summary>
    /// <param name="name"> Friendly name of the drive </param>
    /// <param name="path"> Path to the root directory of the drive </param>
    public sealed class DriveBasicProperties(string name, string path) : BasicStorageItemProperties(name, path)
    {
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
