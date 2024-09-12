#nullable enable
using Microsoft.UI.Xaml;
using System;
using System.Linq;
using Windows.Storage;

namespace FileExplorer.Helpers.Application
{
    public static class LocalSettings
    {
        public static class Keys
        {
            public const string Theme = "Theme";
            public const string ShowConfirmationMessage = "ShowConfirmationMessage";
            public const string OpenFolderInNewTab = "OpenFolderInNewTab";
            public const string Language = "Language";

            public const string ShowHiddenFiles = "ShowHiddenFiles";
            public const string HideSystemFiles = "HideSystemFiles";
            public const string ShowFileExtensions = "ShowFileExtensions";

            public const string ViewOptions = "ViewOptions";
        }

        //private static FrozenDictionary<string, DateTimeFormatter> dateFormatterFromDateOption =
        //    new Dictionary<string, DateTimeFormatter>
        //    {
        //        { "Compact Date and Time",       new DateTimeFormatter("shortdate shorttime") },
        //        { "Full Date and Time",          new DateTimeFormatter("longdate longtime") },
        //        { "Compact Date with Full Time", new DateTimeFormatter("shortdate longtime") },
        //        { "Full Date with Compact Time", new DateTimeFormatter("longdate shorttime") },
        //        { "Detailed Date and Time",      new DateTimeFormatter("full") }

        //    }.ToFrozenDictionary();

        public static string[] Themes { get; }
        public static string[] DateFormats { get; }

        static LocalSettings()
        {
            Themes = Enum.GetValues(typeof(ElementTheme))
                .Cast<ElementTheme>()
                .Select(value => value.ToString())
                .ToArray();

            //DateFormats = dateFormatterFromDateOption.Keys.ToArray();
            DateFormats = ["Default"];
        }

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


        //public static DateTimeFormatter GetSelectedTimeFormatter(string formatterOption)
        //{
        //    bool found = dateFormatterFromDateOption.TryGetValue(formatterOption, out var dateTimeFormatter);

        //    if (!found)
        //    {
        //        dateTimeFormatter = dateFormatterFromDateOption.First().Value;
        //    }

        //    return dateTimeFormatter;
        //}
    }
}
