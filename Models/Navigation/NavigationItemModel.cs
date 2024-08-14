#nullable enable
using Microsoft.UI.Xaml.Media.Imaging;
using Models.General;
using System.Collections.ObjectModel;

namespace Models.Navigation
{
    public sealed class NavigationItemModel(string name, string? path = null) : BasicStorageInfo(name, path)
    {
        public ObservableCollection<NavigationItemModel>? SubItems { get; set; }
        public bool NoChildren => SubItems is null || SubItems.Count < 1;
        public BitmapImage Thumbnail { get; private set; } = new();
        public bool IsPinned { get; set; }
    }
}
