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
            // Empty string means no parameter, so default page should be shown (my pc)
            var pageType = string.IsNullOrEmpty(path) ? typeof(DrivesPage) : typeof(DirectoryPage);

            return pageType;
        }

        public TabModel CreateTabFromDirectory(DirectoryWrapper? directory)
        {
            if (directory is null)
            {
                //TODO: Fix later
                directory = KnownFoldersHelper.Libraries[2];
            }

            return new TabModel(directory);
        }
    }
}
