using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace FileExplorer.UI.ContentSelectors
{
    public sealed class DynamicCollectionTemplateSelector : DataTemplateSelector
    {
        public DataTemplate GridTemplate { get; set; }
        public DataTemplate TableTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            //TODO: Fix this later
            return GridTemplate;
        }
    }
}
