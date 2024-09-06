﻿#nullable enable
using FileExplorer.Core.Contracts;
using FileExplorer.Views;
using Models.Enums;
using System;

namespace FileExplorer.Services
{
    /// <summary>
    /// Service that determines page types in application's Directory pages navigation
    /// </summary>
    public sealed class PageTypesService : IPageTypesService
    {
        /// <inheritdoc />
        public Type GetPage(StorageContentType key)
        {
            return key == StorageContentType.Drives ? typeof(HomePage) : typeof(DirectoryPage);
        }
    }
}
