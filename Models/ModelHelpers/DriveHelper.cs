using Models.Storage.Drives;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Models.ModelHelpers
{
    public static class DriveHelper
    {
        public static ObservableDrivesCollection AvailableDrives
        {
            get
            {
                var availableDrives = DriveInfo.GetDrives().Select(drive => new DriveWrapper(drive));
                return new ObservableDrivesCollection(availableDrives);
            }
        }

        private static FrozenDictionary<DriveType, string> TypeToLabelMap = new Dictionary<DriveType, string>
        {
            { DriveType.Fixed, "Local drive" },
            { DriveType.Removable, "Removable drive" },
            { DriveType.CDRom, "CDRom drive"},
            { DriveType.Network, "Network drive"},
            { DriveType.NoRootDirectory, "Drive without root directory"},
            { DriveType.Ram, "Ram drive"},
            { DriveType.Unknown, "Unknown drive"},

        }.ToFrozenDictionary();

        public static string GetStringType(DriveType type)
        {
            return TypeToLabelMap[type];
        }

        public static string GetFriendlyName(this DriveInfo drive)
        {
            string volumeLabel = drive.IsReady ? drive.VolumeLabel : string.Empty;

            string label = string.IsNullOrEmpty(volumeLabel) ? TypeToLabelMap[drive.DriveType] : drive.VolumeLabel;

            return label + $" ({drive.Name.TrimEnd('\\')})";
        }
    }
}
