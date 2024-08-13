#nullable enable
using Microsoft.UI.Xaml.Media.Imaging;
using Models.General;

namespace Models.Navigation
{
    public sealed class NavigationItemModel(string name, string path) : BasicStorageInfo(name, path)
    {
        public BitmapImage Thumbnail { get; set; } = new();
    }
}
