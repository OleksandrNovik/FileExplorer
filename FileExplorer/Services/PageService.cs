using FileExplorer.Contracts;
using FileExplorer.Models;
using FileExplorer.Views;
using System;
using System.IO;

namespace FileExplorer.Services
{
    public class PageService : IPageService
    {
        private static readonly DirectoryInfo DefaultDirectory = new(@"D:\");

        public TabModel CreateTabFromDirectory(DirectoryInfo dir)
        {
            Type tabType;
            DirectoryInfo tabDirectory;

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
