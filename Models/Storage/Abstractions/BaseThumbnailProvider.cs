﻿using CommunityToolkit.Mvvm.ComponentModel;
using Models.Contracts.Additional;
using Models.Contracts.Storage;
using Models.Storage.Additional;
using System.Threading.Tasks;

namespace Models.Storage.Abstractions
{
    public abstract class BaseThumbnailProvider : ObservableObject, IThumbnailProvider
    {
        public IThumbnail Thumbnail { get; } = new Thumbnail();

        public virtual async Task UpdateThumbnailAsync(int size)
        {
            await Thumbnail.UpdateAsync(size);
            OnPropertyChanged(nameof(Thumbnail));
        }

        public virtual void UpdateThumbnail(int size)
        {
            Thumbnail.Update(size);
            OnPropertyChanged(nameof(Thumbnail));
        }
    }
}
