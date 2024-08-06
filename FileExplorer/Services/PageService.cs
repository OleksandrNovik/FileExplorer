#nullable enable
using FileExplorer.Core.Contracts;
using FileExplorer.Views;
using Helpers.StorageHelpers;
using Models.StorageWrappers;
using Models.TabRelated;
using System;
using Windows.Storage;

namespace FileExplorer.Services
{
    public sealed class PageService : IPageService
    {
        private static readonly StorageFolder DefaultDirectory = KnownFoldersHelper.Documents;

        public TabModel CreateTabFromDirectory(DirectoryWrapper? dir)
        {
            Type tabType;

            if (dir == null)
            {
                //TODO: Open ThisPC instead
                tabType = typeof(DirectoryPage);
                dir = new DirectoryWrapper(DefaultDirectory.Path);
            }
            else
            {
                tabType = typeof(DirectoryPage);
            }

            return new TabModel(dir, tabType);
        }
    }
}
