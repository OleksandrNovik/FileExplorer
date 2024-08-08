#nullable enable
using Windows.Storage;

namespace Helpers.Application
{
    public static class LocalSettings
    {
        public static void WriteSetting(string key, string value)
        {
            ApplicationData.Current.LocalSettings.Values[key] = value;
        }

        public static string? ReadSetting(string key)
        {
            string? setting = null;

            if (ApplicationData.Current.LocalSettings.Values.TryGetValue(key, out var settingValue))
            {
                setting = settingValue?.ToString();
            }

            return setting;
        }
    }
}
