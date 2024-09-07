﻿using Models.Contracts.Additional;
using Models.Contracts.Storage;
using Models.Storage.Additional;
using System.Threading.Tasks;

namespace Models.Storage.Abstractions
{
    /// <summary>
    /// Abstract class that contains basic properties of storage items and thumbnail of item
    /// </summary>
    /// <param name="thumbnail"> Thumbnail of item </param>
    public abstract class StorageItemProperties(IThumbnail thumbnail) : BasicStorageItemProperties, IThumbnailProvider
    {
        protected StorageItemProperties() : this(new Thumbnail()) { }

        public IThumbnail Thumbnail { get; } = thumbnail;

        public virtual async Task UpdateThumbnailAsync(int size)
        {
            await Thumbnail.UpdateAsync(Path, size);
            OnPropertyChanged(nameof(Thumbnail));
        }

        public virtual void UpdateThumbnail(int size)
        {
            Thumbnail.Update(Path, size);
            OnPropertyChanged(nameof(Thumbnail));
        }
    }
}
