﻿using FileExplorer.Core.Contracts.Factories;
using Models.Contracts.Storage;
using Models.Storage.Drives;
using Models.Storage.Windows;
using System;
using System.IO;

namespace FileExplorer.Core.Services.Factories
{
    public sealed class StorageFactory : IStorageFactory<DirectoryItemWrapper>
    {
        /// <inheritdoc />
        public IStorage<DirectoryItemWrapper> CreateFromKey(string key)
        {
            IStorage<DirectoryItemWrapper> storage;

            if (string.IsNullOrEmpty(key))
            {
                storage = new ObservableDrivesCollection();
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