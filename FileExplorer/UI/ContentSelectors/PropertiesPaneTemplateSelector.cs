using FileExplorer.Models.Storage.Additional.Properties;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace FileExplorer.UI.ContentSelectors
{
    public sealed class PropertiesPaneTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DriveTemplate { get; set; }
        public DataTemplate DirectoryItemTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            DataTemplate resultingTemplate;

            if (item is DriveBasicProperties)
            {
                resultingTemplate = DriveTemplate;
            }
            else if (item is DirectoryItemBasicProperties)
            {
                resultingTemplate = DirectoryItemTemplate;
            }
            else
            {
                throw new ArgumentException("Provided item is not property of storage item", nameof(item));
            }

            return resultingTemplate;
        }
    }
}
