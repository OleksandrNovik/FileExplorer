#nullable enable
using FileExplorer.Core.Contracts;
using FileExplorer.Views;
using Models.Contracts.Storage;
using System;

namespace FileExplorer.Services
{
    public sealed class PageTypesService : IPageTypesService
    {
        public Type GetPage(StorageContentType key)
        {
            return key == StorageContentType.Drives ? typeof(HomePage) : typeof(DirectoryPage);
        }
    }
}
