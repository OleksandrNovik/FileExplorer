using Microsoft.UI.Xaml.Data;
using System;
using System.IO;

namespace FileExplorer.UI.Converters
{
    /// <summary>
    /// Converter that returns opacity value based on provided attributes
    /// </summary>
    public sealed class HiddenAttributeToOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            double opacity = 1;

            // Hidden files will be displayed with less opacity
            if (value is FileAttributes attributes && (attributes & FileAttributes.Hidden) != 0)
            {
                opacity = 0.4;
            }

            return opacity;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
