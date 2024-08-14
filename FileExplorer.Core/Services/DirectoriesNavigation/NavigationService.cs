﻿#nullable enable
using FileExplorer.Core.Contracts;
using FileExplorer.Core.Contracts.DirectoriesNavigation;
using FileExplorer.Core.Services.General;

namespace FileExplorer.Core.Services.DirectoriesNavigation
{
    /// <summary>
    /// Class that executes navigation in left navigation pane of the window
    /// </summary>
    public class NavigationService : GenericNavigationService<string>, INavigationService
    {
        public NavigationService(IPageTypesService pageService) : base(pageService) { }
    }
}