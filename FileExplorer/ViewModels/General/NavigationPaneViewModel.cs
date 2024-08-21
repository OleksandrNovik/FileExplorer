using CommunityToolkit.Mvvm.Input;
using Helpers.General;
using Models.ModelHelpers;
using Models.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace FileExplorer.ViewModels.General
{
    public sealed partial class NavigationPaneViewModel
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
                    SubItems = null
                },
                new NavigationItemModel("Pinned")
                {
                    SubItems = new ObservableCollection<NavigationItemModel>(libraries.Select(item => new NavigationItemModel(item.Name, item.Path)
                    {
                        IsPinned = true
                    }))
                },
                new NavigationItemModel("Drives")
                {
                    SubItems = null
                },
                new NavigationItemModel("Libraries")
                {
                    SubItems = new ObservableCollection<NavigationItemModel>(libraries)
                }
            ];
        }

        [RelayCommand]
        public async Task InitializeAsync()
        {
            foreach (var navigationItem in NavigationItems)
            {
                await ThreadingHelper.EnqueueAsync(async () =>
                {
                    await navigationItem.UpdateThumbnailAsync(15);
                });
            }
        }

    }
}
