using System.Collections.Frozen;
using System.Collections.Generic;
using System.IO;

namespace Models.ModelHelpers
{
    public static class DriveHelper
    {
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
            string label = string.IsNullOrEmpty(drive.VolumeLabel) ? TypeToLabelMap[drive.DriveType] : drive.VolumeLabel;

            return label + $" ({drive.Name.TrimEnd('\\')})";
        }
    }
}
