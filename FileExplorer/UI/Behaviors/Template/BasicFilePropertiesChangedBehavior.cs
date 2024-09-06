using Microsoft.UI.Xaml;
using Models.Storage.Abstractions;

namespace FileExplorer.UI.Behaviors.Template
{
    public sealed class BasicFilePropertiesChangedBehavior : BasePropertyChangeTemplateBehavior
    {
        public static readonly DependencyProperty ItemPropsProperty =
            DependencyProperty.Register(nameof(ItemProps), typeof(BasicStorageItemProperties),
                typeof(BasicFilePropertiesChangedBehavior), new PropertyMetadata(null, OnPropertyChanged));

        public BasicStorageItemProperties ItemProps
        {
            get => (BasicStorageItemProperties)GetValue(ItemPropsProperty);
            set => SetValue(ItemPropsProperty, value);
        }
    }
}
