using Microsoft.UI.Xaml.Data;
using System;

namespace FileExplorer.UI.Converters
{
    /// <summary>
    /// Formats provided long number to a "123123 = 123 123" format
    /// </summary>
    public sealed class NumberFormatter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string result = string.Empty;

            if (value is long number)
            {
                result = $"{number:N0}";
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
