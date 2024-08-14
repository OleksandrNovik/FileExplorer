using FileExplorer.Views;
using Models.ModelHelpers;
using Models.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FileExplorer.ViewModels.General
{
    public sealed class NavigationPaneViewModel
    {
        public List<NavigationItemModel> NavigationItems { get; }
        public NavigationPaneViewModel()
        {
            //TODO: pinned items should be loaded from file
            var libraries =
                KnownFoldersHelper.Libraries.Select(wrapper => new NavigationItemModel(wrapper.Name, wrapper.Path)).ToList();


            NavigationItems =
            [
                new NavigationItemModel("Home", string.Empty)
                {
                    NavigationPage = typeof(DirectoryPage).FullName,
                    SubItems = null
                },
                new NavigationItemModel("Pinned", string.Empty)
                {
                    NavigationPage = typeof(DirectoryPage).FullName,
                    SubItems = new ObservableCollection<NavigationItemModel>(libraries.Select(item => new NavigationItemModel(item.Name, item.Path)
                    {
                        IsPinned = true
                    }))
                },
                new NavigationItemModel("Drives", "")
                {
                    NavigationPage = typeof(DirectoryPage).FullName,
                    SubItems = null
                },
                new NavigationItemModel("Libraries", string.Empty)
                {
                    NavigationPage = typeof(DirectoryPage).FullName,
                    SubItems = new ObservableCollection<NavigationItemModel>(libraries)
                }
            ];
        }

        public void SetIcons()
        {
            foreach (var item in NavigationItems)
            {
                item.Thumbnail.SetSource(IconHelper.TryGetCachedThumbnail("True"));
            }
        }
    }
}
