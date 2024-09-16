using FileExplorer.ViewModels.General;

namespace FileExplorer.ViewModels
{
    public sealed class ViewModelsLocator
    {
        public ViewModelsLocator()
        {
            StorageNamingViewModel = App.GetService<StorageItemsNamingViewModel>();
        }

        public StorageItemsNamingViewModel StorageNamingViewModel { get; }
    }
}
