#nullable enable
using Microsoft.UI.Xaml.Controls;
using System;

namespace FileExplorer.Core.Contracts.Settings
{
    public interface ISettingsNavigationViewService
    {
        public void Initialize(NavigationView navigationView);
        void UnregisterEvents();
        NavigationViewItem? GetSelectedItem(Type pageType);
    }
}
