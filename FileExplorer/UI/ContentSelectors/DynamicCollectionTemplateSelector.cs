using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Models.Messages;
using System;

namespace FileExplorer.UI.ContentSelectors
{
    public sealed class DynamicCollectionTemplateSelector : DataTemplateSelector
    {
        public DataTemplate GridTemplate { get; set; }
        public DataTemplate TableTemplate { get; set; }
        protected override DataTemplate SelectTemplateCore(object item)
        {
            if (item is ViewOptions viewOptions)
            {
                return viewOptions switch
                {
                    ViewOptions.GridView => GridTemplate,
                    ViewOptions.TableView => TableTemplate,
                    _ => throw new ArgumentException("Invalid view options", nameof(item))
                };
            }

            throw new ArgumentException("Invalid view options", nameof(item));
        }
    }
}
