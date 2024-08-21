#nullable enable
using FileExplorer.Core.Contracts;
using FileExplorer.Views;
using System;

namespace FileExplorer.Services
{
    public sealed class PageTypesService : IPageTypesService
    {
        public Type GetPage(string? path)
        {
            // Empty string means no parameter, so default page should be shown (my pc)
            var pageType = string.IsNullOrEmpty(path) ? typeof(HomePage) : typeof(DirectoryPage);

            return pageType;
        }
    }
}
