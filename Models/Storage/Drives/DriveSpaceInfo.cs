using CommunityToolkit.Mvvm.ComponentModel;
using Models.Storage.Additional;
using System;
using System.IO;

namespace Models.Storage.Drives
{
    /// <summary>
    /// Class that holds necessary information about drive's space
    /// </summary>
    public sealed class DriveSpaceInfo : ObservableObject
    {
        /// <summary>
        /// Space that is available for user
        /// </summary>
        public ByteSize SpaceAvailableForUser { get; }

        /// <summary>
        /// Total free space on drive (available for all users)
        /// </summary>
        public ByteSize FreeSpace { get; }

        /// <summary>
        /// Size of the drive
        /// </summary>
        public ByteSize TotalSpace { get; }

        /// <summary>
        /// Gets percentage of available space for a drive
        /// </summary>
        public int AvailablePercentage { get; }

        public DriveSpaceInfo(DriveInfo drive)
        {
            if (!drive.IsReady)
                throw new ArgumentException($"{drive.Name} is not ready for use.", nameof(drive));

            SpaceAvailableForUser = new ByteSize(drive.AvailableFreeSpace);
            FreeSpace = new ByteSize(drive.TotalFreeSpace);
            TotalSpace = new ByteSize(drive.TotalSize);

            AvailablePercentage = (int)((TotalSpace.InBytes - FreeSpace.InBytes) * 100 / TotalSpace.InBytes);
        }


    }
}
