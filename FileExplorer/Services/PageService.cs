#nullable enable
using FileExplorer.Core.Contracts;
using FileExplorer.Views;
using Models.ModelHelpers;
using Models.StorageWrappers;
using Models.TabRelated;
using System;

namespace FileExplorer.Services
{
    public sealed class PageService : IPageService
    {
        private static readonly DirectoryWrapper DefaultDirectory = KnownFoldersHelper.Documents;

        public TabModel CreateTabFromDirectory(DirectoryWrapper? dir)
        {
            Type tabType;

            if (dir == null)
            {
                //TODO: Open ThisPC instead
                tabType = typeof(DirectoryPage);
                dir = DefaultDirectory;
            }
            else
            {
                tabType = typeof(DirectoryPage);
            }

            return new TabModel(dir, tabType);
        }
    }
}
