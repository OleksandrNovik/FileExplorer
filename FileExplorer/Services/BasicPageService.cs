#nullable enable
using FileExplorer.Core.Contracts.General;
using FileExplorer.Views.Settings.Pages;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;

namespace FileExplorer.Services
{
    public sealed class BasicPageService : IBasicPageService<string>
    {
        private readonly FrozenDictionary<string, Type> pages = new Dictionary<string, Type>
        {
            { typeof(SettingsPreferencesPage).FullName, typeof(SettingsPreferencesPage) },
            { typeof(SettingsExplorerPage).FullName, typeof(SettingsExplorerPage) },
            { typeof(SettingsAboutPage).FullName, typeof(SettingsAboutPage) },

        }.ToFrozenDictionary();

        public Type GetPage(string? key)
        {
            Type? pageType;

            if (string.IsNullOrEmpty(key) || !pages.ContainsKey(key))
            {
                pageType = pages.First().Value;
            }
            else
            {
                pageType = pages[key];
            }

            return pageType;
        }

    }
}
