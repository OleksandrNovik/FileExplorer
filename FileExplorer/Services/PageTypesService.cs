#nullable enable
using FileExplorer.Core.Contracts;
using FileExplorer.Views;
using Models.ModelHelpers;
using Models.StorageWrappers;
using Models.TabRelated;
using System;

namespace FileExplorer.Services
{
    public sealed class PageTypesService : IPageTypesService
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

        public TabModel CreateTabFromDirectory(DirectoryWrapper? directory)
        {
            var tabType = GetPage(directory?.Path);

            if (directory is null)
            {
                //TODO: Fix later
                directory = KnownFoldersHelper.Libraries[2];
            }

            return new TabModel(directory, tabType);
        }
    }
}
