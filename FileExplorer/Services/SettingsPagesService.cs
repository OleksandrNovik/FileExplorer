using FileExplorer.Core.Contracts.Settings;
using FileExplorer.Views.Settings.Pages;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;

namespace FileExplorer.Services
{
    public sealed class SettingsPagesService : ISettingsPagesService
    {
        private readonly FrozenDictionary<string, Type> pages = new Dictionary<string, Type>
        {
            { typeof(SettingsPreferencesPage).FullName, typeof(SettingsPreferencesPage) },
            { typeof(SettingsExplorerPage).FullName, typeof(SettingsExplorerPage) },
            { typeof(SettingsAboutPage).FullName, typeof(SettingsAboutPage) },

        }.ToFrozenDictionary();

        public Type GetPage(string key)
        {
            if (!pages.TryGetValue(key, out var pageType))
            {
                throw new ArgumentException($"There is no such a page with name: {key}", nameof(key));
            }

            return pageType;
        }

        public Type GetDefaultPage()
        {
            return pages.First().Value;
        }
    }
}
