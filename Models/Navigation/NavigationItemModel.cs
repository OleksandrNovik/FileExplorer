﻿#nullable enable
using Models.Storage.Abstractions;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Models.Navigation
{
    public sealed class NavigationItemModel : BaseThumbnailProvider
    {
        public string Name { get; }
        public string? Path { get; }
        public ObservableCollection<NavigationItemModel>? SubItems { get; set; }
        public bool NoChildren => SubItems is null || SubItems.Count < 1;
        public bool IsPinned { get; set; }

        public NavigationItemModel(string name, string? path = null)
        {
            Thumbnail.ItemPath = path;
            Path = path;
            Name = name;
        }

        public override async Task UpdateThumbnailAsync(int size)
        {
            if (SubItems is not null && SubItems.Count > 0)
            {
                foreach (var subItem in SubItems)
                {
                    await subItem.UpdateThumbnailAsync(size);
                }
            }

            await base.UpdateThumbnailAsync(size);
        }
    }
}
