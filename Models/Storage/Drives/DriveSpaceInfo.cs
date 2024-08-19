using CommunityToolkit.Mvvm.ComponentModel;
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
        public long SpaceAvailableForUser { get; }

        /// <summary>
        /// Total free space on drive (available for all users)
        /// </summary>
        public long FreeSpace { get; }

        /// <summary>
        /// Size of the drive
        /// </summary>
        public long TotalSpace { get; }

        /// <summary>
        /// Gets percentage of available space for a drive
        /// </summary>
        public int AvailablePercentage { get; }

        public DriveSpaceInfo(DriveInfo drive)
        {
            if (!drive.IsReady)
                throw new ArgumentException($"{drive.Name} is not ready for use.", nameof(drive));

            SpaceAvailableForUser = drive.AvailableFreeSpace;
            FreeSpace = drive.TotalFreeSpace;
            TotalSpace = drive.TotalSize;

            AvailablePercentage = (int)((TotalSpace - FreeSpace) * 100 / TotalSpace);
        }


    }
}
