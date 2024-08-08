using System;

namespace FileExplorer.Core.Contracts.Settings
{
    public interface ISettingsPagesService
    {
        public Type GetPage(string key);

        public Type GetDefaultPage();
    }
}
