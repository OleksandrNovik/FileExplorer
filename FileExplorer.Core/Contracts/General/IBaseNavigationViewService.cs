#nullable enable
using Microsoft.UI.Xaml.Controls;
using System;

namespace FileExplorer.Core.Contracts.General
{
    public interface IBaseNavigationViewService
    {
        public void Initialize(NavigationView navigationView);
        void UnregisterEvents();
        NavigationViewItem? GetSelectedItem(Type pageType);
    }
}
