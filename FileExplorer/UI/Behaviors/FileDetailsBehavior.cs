using FileExplorer.Models;
using FileExplorer.UI.UserControls;
using Microsoft.UI.Xaml;
using Microsoft.Xaml.Interactivity;
using System;

namespace FileExplorer.UI.Behaviors
{
    public class FileDetailsBehavior : Behavior<FileDetails>
    {
        public FileInfoModel Details
        {
            get => GetValue(DetailsProperty) as FileInfoModel;
            set => SetValue(DetailsProperty, value);
        }

        public static DependencyProperty DetailsProperty =
            DependencyProperty.Register(nameof(Details), typeof(FileInfoModel),
                typeof(FileDetailsBehavior), new PropertyMetadata(null, OnDetailsChanged));

        private static void OnDetailsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FileDetailsBehavior behavior)
            {
                var info = e.NewValue as FileInfoModel;

                ArgumentNullException.ThrowIfNull(info);

                behavior.AssociatedObject.Visibility = info.IsShown ? Visibility.Visible : Visibility.Collapsed;
            }
        }
    }
}
