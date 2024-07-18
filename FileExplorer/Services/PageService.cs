#nullable enable
using FileExplorer.Contracts;
using FileExplorer.Helpers.StorageHelpers;
using FileExplorer.Models;
using FileExplorer.Models.StorageWrappers;
using FileExplorer.Views;
using System;
using Windows.Storage;

namespace FileExplorer.Services
{
    public class PageService : IPageService
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
