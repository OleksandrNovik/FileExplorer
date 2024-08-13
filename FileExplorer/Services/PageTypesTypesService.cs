#nullable enable
using FileExplorer.Core.Contracts;
using FileExplorer.Views;
using Models.ModelHelpers;
using Models.StorageWrappers;
using Models.TabRelated;
using System;

namespace FileExplorer.Services
{
    public sealed class PageTypesTypesService : IPageTypesService
    {
        public Type GetPage(string? path)
        {
            Type pageType;

            if (string.IsNullOrEmpty(path))
            {
                //TODO: Open ThisPC instead
                pageType = typeof(DirectoryPage);
            }
            else
            {
                pageType = typeof(DirectoryPage);
            }

            return pageType;
        }

        public TabModel CreateTabFromDirectory(DirectoryWrapper? dir)
        {
            var tabType = GetPage(dir?.Path);

            if (dir is null)
            {
                //TODO: Fix later
                dir = KnownFoldersHelper.Libraries[2];
            }

            return new TabModel(dir, tabType);
        }
    }
}
