using FileExplorer.Core.Contracts.Factories;
using FileExplorer.Models.Contracts.Storage;
using FileExplorer.Models.ModelHelpers;
using FileExplorer.Models.Storage.Windows;
using System;
using System.IO;

namespace FileExplorer.Core.Services.Factories
{
    public sealed class StorageFactory : IStorageFactory
    {
        /// <inheritdoc />
        public IStorage CreateFromKey(string key)
        {
            IStorage storage;

            if (string.IsNullOrEmpty(key))
            {
                storage = DriveHelper.AvailableDrives;
            }
            else if (DriveHelper.AvailableDrives.TryGetDrive(key, out var drive))
            {
                storage = drive;
            }
            else if (Path.Exists(key))
            {
                storage = new DirectoryWrapper(key);
            }
            else
                throw new ArgumentException($"Cannot create storage from key {key}", nameof(key));

            return storage;
        }
    }
}
