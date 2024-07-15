using FileExplorer.Contracts;
using FileExplorer.Helpers;
using FileExplorer.Models;
using FileExplorer.Views;
using System;
using Windows.Storage;

namespace FileExplorer.Services
{
    public class PageService : IPageService
    {
        private static readonly StorageFolder DefaultDirectory
            = KnownFoldersHelper.SpecialFolders[KnownFolders.DocumentsLibrary.DisplayName];

        public TabModel CreateTabFromDirectory(StorageFolder dir)
        {
            Type tabType;
            StorageFolder tabDirectory;

            if (dir == null)
            {
                //TODO: Open ThisPC instead
                tabType = typeof(DirectoryPage);
                tabDirectory = DefaultDirectory;
            }
            else
            {
                tabType = typeof(DirectoryPage);
                tabDirectory = dir;
            }

            return new TabModel(tabDirectory, tabType);
        }
    }
}
