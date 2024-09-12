using FileExplorer.Models.Storage.Abstractions;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace FileExplorer.UI.UserControls.StorageProperties
{
    public abstract class PropertiesUserControl : UserControl
    {
        public static readonly DependencyProperty PropertiesProperty =
            DependencyProperty.Register(nameof(Properties), typeof(StorageItemProperties),
                typeof(DirectoryItemPropertiesViewer), new PropertyMetadata(null, OnPropertiesChanged));

        protected static void OnPropertiesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PropertiesUserControl control)
            {
                control.PropertiesChanged(d, e);
            }
        }

        protected abstract void PropertiesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e);

        public StorageItemProperties Properties
        {
            get => GetValue(PropertiesProperty) as StorageItemProperties;
            set => SetValue(PropertiesProperty, value);

        }
    }
}